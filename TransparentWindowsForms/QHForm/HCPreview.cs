using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Configuration;



/*=================================================================
* Author: QUJUN
* CreatedTime: 2019/1/9 14:26:23
* Description: 视频主窗体(主窗体可替换其他窗体)
*
* Dear maintainer:
* 
* Once you are done trying to 'optimize' this routine,
* and have realized what a terrible mistake that was,
* please increment the following counter as a warning
* to the next guy:
* 
* total_hours_wasted_here = 42
*
===================================================================*/

namespace TransparentWindowsForms
{

    public partial class HCPreview : Form
    {

        // ****************************  海康 ****************************

        private bool m_bInitSDK = false;
        private bool m_bRecord = false;
        private uint iLastErr = 0;
        private Int32 m_lUserID = -1;
        private Int32 m_lRealHandle = -1;
        private string str1;
        private string str2;
        private Int32 i = 0;
        private Int32 m_lTree = 0;
        private string str;
        private long iSelIndex = 0;
        private uint dwAChanTotalNum = 0;
        private uint dwDChanTotalNum = 0;
        private Int32 m_lPort = -1;
        private IntPtr m_ptrRealHandle;
        private int[] iIPDevID = new int[96];
        private int[] iChannelNum = new int[96];

        private CHCNetSDK.REALDATACALLBACK RealData = null;
        private CHCNetSDK.DRAWFUN m_drawFun = null;
        public CHCNetSDK.NET_DVR_DEVICEINFO_V30 DeviceInfo;
        public CHCNetSDK.NET_DVR_IPPARACFG_V40 m_struIpParaCfgV40;
        public CHCNetSDK.NET_DVR_STREAM_MODE m_struStreamMode;
        public CHCNetSDK.NET_DVR_IPCHANINFO m_struChanInfo;
        public CHCNetSDK.NET_DVR_IPCHANINFO_V40 m_struChanInfoV40;
        private PlayCtrl.DECCBFUN m_fDisplayFun = null;
        private PlayCtrl.DRAWFUN m_play_drawFun = null;
        public delegate void MyDebugInfo(string str);
        private static object o = new object();
        private static Queue<MonitorPoint> _points { get; set; }

        // ****************************  海康 ****************************


        private TransparentDCFrom _tranForm = null;  //  监控点窗体

        private IconFrom _iconForm = null; //  图层控制窗体

        private MonitorControlForm _monitorForm = null; // 监控菜单窗体

        private bool _isShowInfo = false;

        private bool _isShowRec = false;

        //  *******************  事件监听  *******************

        public event Action OnClose;

        public event Action OnMinimize;

        public event Action OnMaximize;

        public event Action OnNormal;

        public HCPreview()
        {
            InitializeComponent();

            this.Load += HCPreview_Load;

            this.SizeChanged += HCPreview_SizeChanged;

            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;

            Rectangle ScreenArea = Screen.GetWorkingArea(this);

            this.Size = new Size(ScreenArea.Width, ScreenArea.Height);//设置为屏幕高度

            this.Location = new Point(0, 0);

            showTransparentForm();

            showIconForm();

            showMonitorForm();

            loginHcMonitor();

        }

        private void HCPreview_SizeChanged(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Normal)
            {
                if (OnNormal != null)
                    OnNormal();
                return;
            }

            if (this.WindowState == FormWindowState.Maximized)
            {
                if (OnMaximize != null)
                    OnMaximize();
                return;
            }

            if (this.WindowState == FormWindowState.Minimized)
            {
                if (OnMinimize != null)
                    OnMinimize();
                return;
            }
        }

        private void HCPreview_Load(object sender, EventArgs e)
        {
            _tranForm.Size = this.Size;

            _tranForm.Location = this.Location;

            _iconForm.Size = this.Size;

            _iconForm.Location = this.Location;

            _monitorForm.Size = this.Size;

            _monitorForm.Location = this.Location;
        }


        private void loginHcMonitor()
        {
            m_bInitSDK = CHCNetSDK.NET_DVR_Init();

            if (m_bInitSDK == false)
            {
                MessageBox.Show("NET_DVR_Init error!");
                return;
            }
            else
            {
                //保存SDK日志 To save the SDK log
                CHCNetSDK.NET_DVR_SetLogToFile(3, "C:\\SdkLog\\", true);

                for (int i = 0; i < 64; i++)
                {
                    iIPDevID[i] = -1;
                    iChannelNum[i] = -1;
                }
            }


            btnLogin_Click();
        }


        private void showTransparentForm()
        {

            if (_tranForm != null)
                return;

            _tranForm = new TransparentDCFrom(this);

            _tranForm.Owner = this;

            _tranForm.Show();

            _tranForm.BringToFront();
        }

        private void showIconForm()
        {
            if (_iconForm != null)
                return;

            _iconForm = new IconFrom(this);

            _iconForm.Owner = this;

            _iconForm.Show();

            _iconForm.BringToFront();
        }


        private void showMonitorForm()
        {

            if (_monitorForm != null)
                return;

            _monitorForm = new MonitorControlForm(this);

            _monitorForm.Owner = this;

            _monitorForm.IsSelectInfo += MonitorForm_IsSelectInfo;

            _monitorForm.Show();

            _monitorForm.BringToFront();
        }


        //  TODO  下拉列表回调事件 （未完善）
        private void MonitorForm_IsSelectInfo(QhControl.scrollcombox.SelectListBoxInfo obj)
        {
            if (obj == null)
                return;

            if (obj.Info == "信息栏")
            {
                _isShowInfo = obj.IsCheck;
                return;
            }

            if (obj.Info == "矩形框")
            {
                _isShowRec = obj.IsCheck;
                return;
            }

            _tranForm.SetIsShowMonitor(obj.IsCheck, obj.Info);
            return;

        }


        public void DebugInfo(string str)
        {
            if (str.Length > 0)
            {
                str += "\n";
                Console.WriteLine(str);
            }
        }

        private void btnLogin_Click()
        {
            if (m_lUserID < 0)
            {
                string DVRIPAddress = "192.168.2.53"; //设备IP地址或者域名 Device IP
                Int16 DVRPortNumber = 8000;//设备服务端口号 Device Port
                string DVRUserName = "admin";//设备登录用户名 User name to login
                string DVRPassword = "hik12345";//设备登录密码 Password to login


                //登录设备 Login the device
                m_lUserID = CHCNetSDK.NET_DVR_Login_V30(DVRIPAddress, DVRPortNumber, DVRUserName, DVRPassword, ref DeviceInfo);
                if (m_lUserID < 0)
                {
                    iLastErr = CHCNetSDK.NET_DVR_GetLastError();
                    str = "NET_DVR_Login_V30 failed, error code= " + iLastErr; //登录失败，输出错误号 Failed to login and output the error code
                    DebugInfo(str);
                    return;
                }
                else
                {
                    //登录成功
                    DebugInfo("NET_DVR_Login_V30 succ!");

                    dwAChanTotalNum = (uint)DeviceInfo.byChanNum;
                    dwDChanTotalNum = (uint)DeviceInfo.byIPChanNum + 256 * (uint)DeviceInfo.byHighDChanNum;
                    if (dwDChanTotalNum > 0)
                    {
                        InfoIPChannel();
                    }
                    else
                    {
                        for (i = 0; i < dwAChanTotalNum; i++)
                        {
                            ListAnalogChannel(i + 1, 1);
                            iChannelNum[i] = i + (int)DeviceInfo.byStartChan;
                        }
                    }

                    btnPreview_Click();
                }

            }
            else
            {
                //注销登录 Logout the device
                if (m_lRealHandle >= 0)
                {
                    DebugInfo("Please stop live view firstly"); //登出前先停止预览 Stop live view before logout
                    return;
                }

                if (!CHCNetSDK.NET_DVR_Logout(m_lUserID))
                {
                    iLastErr = CHCNetSDK.NET_DVR_GetLastError();
                    str = "NET_DVR_Logout failed, error code= " + iLastErr;
                    DebugInfo(str);
                    return;
                }
                DebugInfo("NET_DVR_Logout succ!");
                m_lUserID = -1;
            }
            return;
        }

        public void InfoIPChannel()
        {
            uint dwSize = (uint)Marshal.SizeOf(m_struIpParaCfgV40);

            IntPtr ptrIpParaCfgV40 = Marshal.AllocHGlobal((Int32)dwSize);
            Marshal.StructureToPtr(m_struIpParaCfgV40, ptrIpParaCfgV40, false);

            uint dwReturn = 0;
            int iGroupNo = 0;  //该Demo仅获取第一组64个通道，如果设备IP通道大于64路，需要按组号0~i多次调用NET_DVR_GET_IPPARACFG_V40获取

            if (!CHCNetSDK.NET_DVR_GetDVRConfig(m_lUserID, CHCNetSDK.NET_DVR_GET_IPPARACFG_V40, iGroupNo, ptrIpParaCfgV40, dwSize, ref dwReturn))
            {
                iLastErr = CHCNetSDK.NET_DVR_GetLastError();
                str = "NET_DVR_GET_IPPARACFG_V40 failed, error code= " + iLastErr;
                //获取IP资源配置信息失败，输出错误号 Failed to get configuration of IP channels and output the error code
                DebugInfo(str);
            }
            else
            {
                DebugInfo("NET_DVR_GET_IPPARACFG_V40 succ!");

                m_struIpParaCfgV40 = (CHCNetSDK.NET_DVR_IPPARACFG_V40)Marshal.PtrToStructure(ptrIpParaCfgV40, typeof(CHCNetSDK.NET_DVR_IPPARACFG_V40));

                for (i = 0; i < dwAChanTotalNum; i++)
                {
                    ListAnalogChannel(i + 1, m_struIpParaCfgV40.byAnalogChanEnable[i]);
                    iChannelNum[i] = i + (int)DeviceInfo.byStartChan;
                }

                byte byStreamType = 0;
                uint iDChanNum = 64;

                if (dwDChanTotalNum < 64)
                {
                    iDChanNum = dwDChanTotalNum; //如果设备IP通道小于64路，按实际路数获取
                }

                for (i = 0; i < iDChanNum; i++)
                {
                    iChannelNum[i + dwAChanTotalNum] = i + (int)m_struIpParaCfgV40.dwStartDChan;
                    byStreamType = m_struIpParaCfgV40.struStreamMode[i].byGetStreamType;

                    dwSize = (uint)Marshal.SizeOf(m_struIpParaCfgV40.struStreamMode[i].uGetStream);
                    switch (byStreamType)
                    {
                        //目前NVR仅支持直接从设备取流 NVR supports only the mode: get stream from device directly
                        case 0:
                            IntPtr ptrChanInfo = Marshal.AllocHGlobal((Int32)dwSize);
                            Marshal.StructureToPtr(m_struIpParaCfgV40.struStreamMode[i].uGetStream, ptrChanInfo, false);
                            m_struChanInfo = (CHCNetSDK.NET_DVR_IPCHANINFO)Marshal.PtrToStructure(ptrChanInfo, typeof(CHCNetSDK.NET_DVR_IPCHANINFO));

                            //列出IP通道 List the IP channel
                            ListIPChannel(i + 1, m_struChanInfo.byEnable, m_struChanInfo.byIPID);
                            iIPDevID[i] = m_struChanInfo.byIPID + m_struChanInfo.byIPIDHigh * 256 - iGroupNo * 64 - 1;

                            Marshal.FreeHGlobal(ptrChanInfo);
                            break;
                        case 6:
                            IntPtr ptrChanInfoV40 = Marshal.AllocHGlobal((Int32)dwSize);
                            Marshal.StructureToPtr(m_struIpParaCfgV40.struStreamMode[i].uGetStream, ptrChanInfoV40, false);
                            m_struChanInfoV40 = (CHCNetSDK.NET_DVR_IPCHANINFO_V40)Marshal.PtrToStructure(ptrChanInfoV40, typeof(CHCNetSDK.NET_DVR_IPCHANINFO_V40));

                            //列出IP通道 List the IP channel
                            ListIPChannel(i + 1, m_struChanInfoV40.byEnable, m_struChanInfoV40.wIPID);
                            iIPDevID[i] = m_struChanInfoV40.wIPID - iGroupNo * 64 - 1;

                            Marshal.FreeHGlobal(ptrChanInfoV40);
                            break;
                        default:
                            break;
                    }
                }
            }
            Marshal.FreeHGlobal(ptrIpParaCfgV40);

        }
        public void ListIPChannel(Int32 iChanNo, byte byOnline, int byIPID)
        {
            str1 = String.Format("IPCamera {0}", iChanNo);
            m_lTree++;

            if (byIPID == 0)
            {
                str2 = "X"; //通道空闲，没有添加前端设备 the channel is idle                  
            }
            else
            {
                if (byOnline == 0)
                {
                    str2 = "offline"; //通道不在线 the channel is off-line
                }
                else
                    str2 = "online"; //通道在线 The channel is on-line
            }

            // listViewIPChannel.Items.Add(new ListViewItem(new string[] { str1, str2 }));//将通道添加到列表中 add the channel to the list
        }
        public void ListAnalogChannel(Int32 iChanNo, byte byEnable)
        {
            str1 = String.Format("Camera {0}", iChanNo);
            m_lTree++;

            if (byEnable == 0)
            {
                str2 = "Disabled"; //通道已被禁用 This channel has been disabled               
            }
            else
            {
                str2 = "Enabled"; //通道处于启用状态 This channel has been enabled
            }

            // listViewIPChannel.Items.Add(new ListViewItem(new string[] { str1, str2 }));//将通道添加到列表中 add the channel to the list
        }

        private void listViewIPChannel_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {
        }

        //解码回调函数
        private void DecCallbackFUN(int nPort, IntPtr pBuf, int nSize, ref PlayCtrl.FRAME_INFO pFrameInfo, int nReserved1, int nReserved2)
        {
            // 将pBuf解码后视频输入写入文件中（解码后YUV数据量极大，尤其是高清码流，不建议在回调函数中处理）
            if (pFrameInfo.nType == 3) //#define T_YV12	3
            {
            }
        }

        public void RealDataCallBack(Int32 lRealHandle, UInt32 dwDataType, IntPtr pBuffer, UInt32 dwBufSize, IntPtr pUser)
        {
            //下面数据处理建议使用委托的方式
            MyDebugInfo AlarmInfo = new MyDebugInfo(DebugInfo);
            switch (dwDataType)
            {
                case CHCNetSDK.NET_DVR_SYSHEAD:     // sys head

                    if (dwBufSize > 0)
                    {
                        if (m_lPort >= 0)
                        {
                            return; //同一路码流不需要多次调用开流接口
                        }

                        //获取播放句柄 Get the port to play
                        if (!PlayCtrl.PlayM4_GetPort(ref m_lPort))
                        {
                            iLastErr = PlayCtrl.PlayM4_GetLastError(m_lPort);
                            str = "PlayM4_GetPort failed, error code= " + iLastErr;
                            this.BeginInvoke(AlarmInfo, str);
                            break;
                        }

                        //设置流播放模式 Set the stream mode: real-time stream mode
                        if (!PlayCtrl.PlayM4_SetStreamOpenMode(m_lPort, PlayCtrl.STREAME_REALTIME))
                        {
                            iLastErr = PlayCtrl.PlayM4_GetLastError(m_lPort);
                            str = "Set STREAME_REALTIME mode failed, error code= " + iLastErr;
                            this.BeginInvoke(AlarmInfo, str);
                        }

                        //打开码流，送入头数据 Open stream
                        if (!PlayCtrl.PlayM4_OpenStream(m_lPort, pBuffer, dwBufSize, 2 * 1024 * 1024))
                        {
                            iLastErr = PlayCtrl.PlayM4_GetLastError(m_lPort);
                            str = "PlayM4_OpenStream failed, error code= " + iLastErr;
                            this.BeginInvoke(AlarmInfo, str);
                            break;
                        }

                        //设置显示缓冲区个数 Set the display buffer number
                        if (!PlayCtrl.PlayM4_SetDisplayBuf(m_lPort, 15))
                        {
                            iLastErr = PlayCtrl.PlayM4_GetLastError(m_lPort);
                            str = "PlayM4_SetDisplayBuf failed, error code= " + iLastErr;
                            this.BeginInvoke(AlarmInfo, str);
                        }

                        //设置显示模式 Set the display mode
                        if (!PlayCtrl.PlayM4_SetOverlayMode(m_lPort, 0, 0/* COLORREF(0)*/)) //play off screen 
                        {
                            iLastErr = PlayCtrl.PlayM4_GetLastError(m_lPort);
                            str = "PlayM4_SetOverlayMode failed, error code= " + iLastErr;
                            this.BeginInvoke(AlarmInfo, str);
                        }

                        //设置解码回调函数，获取解码后音视频原始数据 Set callback function of decoded data
                        //m_fDisplayFun = new PlayCtrl.DECCBFUN(DecCallbackFUN);
                        //if (!PlayCtrl.PlayM4_SetDecCallBackEx(m_lPort, m_fDisplayFun, IntPtr.Zero, 0))
                        //{
                        //    this.BeginInvoke(AlarmInfo, "PlayM4_SetDisplayCallBack fail");
                        //}


                        m_play_drawFun = new PlayCtrl.DRAWFUN(DrawFunCallBack);

                        if (!PlayCtrl.PlayM4_RegisterDrawFun(m_lPort, m_play_drawFun, m_lUserID))
                        {
                            iLastErr = PlayCtrl.PlayM4_GetLastError(m_lPort);
                        }

                        //开始解码 Start to play                       
                        if (!PlayCtrl.PlayM4_Play(m_lPort, m_ptrRealHandle))
                        {
                            iLastErr = PlayCtrl.PlayM4_GetLastError(m_lPort);
                            str = "PlayM4_Play failed, error code= " + iLastErr;
                            this.BeginInvoke(AlarmInfo, str);
                            break;
                        }



                    }
                    break;
                case CHCNetSDK.NET_DVR_STREAMDATA:     // video stream data

                    if (dwBufSize > 0 && m_lPort != -1)
                    {
                        for (int i = 0; i < 999; i++)
                        {
                            //送入码流数据进行解码 Input the stream data to decode
                            if (!PlayCtrl.PlayM4_InputData(m_lPort, pBuffer, dwBufSize))
                            {
                                iLastErr = PlayCtrl.PlayM4_GetLastError(m_lPort);
                                str = "PlayM4_InputData failed, error code= " + iLastErr;
                                Thread.Sleep(2);
                            }
                            else
                            {
                                break;
                            }
                        }


                    }
                    break;
                default:
                    if (dwBufSize > 0 && m_lPort != -1)
                    {
                        //送入其他数据 Input the other data
                        for (int i = 0; i < 999; i++)
                        {
                            if (!PlayCtrl.PlayM4_InputData(m_lPort, pBuffer, dwBufSize))
                            {
                                iLastErr = PlayCtrl.PlayM4_GetLastError(m_lPort);
                                str = "PlayM4_InputData failed, error code= " + iLastErr;
                                Thread.Sleep(2);
                            }
                            else
                            {
                                break;
                            }
                        }
                    }
                    break;
            }
        }

        Boolean _flag = true;

        Boolean _isInit = false;

        private void btnPreview_Click()
        {
            if (m_lUserID < 0)
            {
                MessageBox.Show("Please login the device firstly!");
                return;
            }

            if (m_bRecord)
            {
                MessageBox.Show("Please stop recording firstly!");
                return;
            }

            if (m_lRealHandle < 0)
            {
                CHCNetSDK.NET_DVR_PREVIEWINFO lpPreviewInfo = new CHCNetSDK.NET_DVR_PREVIEWINFO();
                lpPreviewInfo.hPlayWnd = RealPlayWnd.Handle;//预览窗口 live view window
                lpPreviewInfo.lChannel = iChannelNum[(int)iSelIndex];//预览的设备通道 the device channel number
                lpPreviewInfo.dwStreamType = 1;//码流类型：0-主码流，1-子码流，2-码流3，3-码流4，以此类推
                lpPreviewInfo.dwLinkMode = 0;//连接方式：0- TCP方式，1- UDP方式，2- 多播方式，3- RTP方式，4-RTP/RTSP，5-RSTP/HTTP 
                lpPreviewInfo.bBlocked = true; //0- 非阻塞取流，1- 阻塞取流
                lpPreviewInfo.dwDisplayBufNum = 15; //播放库显示缓冲区最大帧数


                IntPtr pUser = IntPtr.Zero;//用户数据 user data 

                if (_mSelectedIndex == 0)
                {
                    //打开预览 Start live view 
                    m_lRealHandle = CHCNetSDK.NET_DVR_RealPlay_V40(m_lUserID, ref lpPreviewInfo, null/*RealData*/, pUser);

                }
                else
                {
                    lpPreviewInfo.hPlayWnd = IntPtr.Zero;//预览窗口 live view window
                    m_ptrRealHandle = RealPlayWnd.Handle;
                    RealData = new CHCNetSDK.REALDATACALLBACK(RealDataCallBack);//预览实时流回调函数 real-time stream callback function                   
                    m_lRealHandle = CHCNetSDK.NET_DVR_RealPlay_V40(m_lUserID, ref lpPreviewInfo, RealData, pUser);

                }

                if (m_lRealHandle < 0)
                {
                    iLastErr = CHCNetSDK.NET_DVR_GetLastError();
                    str = "NET_DVR_RealPlay_V40 failed, error code= " + iLastErr; //预览失败，输出错误号 failed to start live view, and output the error code.
                    DebugInfo(str);
                    return;
                }
                else
                {
                    //预览成功
                    DebugInfo("NET_DVR_RealPlay_V40 succ!");


                    // 配置文件  是否开启人脸识别
                    string is_face = ConfigurationManager.AppSettings["FaceRecognition"].ToString();

                    if (is_face == "1")
                        start();
                }

            }
            else
            {
                //停止预览 Stop live view 
                if (!CHCNetSDK.NET_DVR_StopRealPlay(m_lRealHandle))
                {
                    iLastErr = CHCNetSDK.NET_DVR_GetLastError();
                    str = "NET_DVR_StopRealPlay failed, error code= " + iLastErr;
                    DebugInfo(str);
                    return;
                }

                if ((_mSelectedIndex == 1) && (m_lPort >= 0))
                {
                    if (!PlayCtrl.PlayM4_Stop(m_lPort))
                    {
                        iLastErr = PlayCtrl.PlayM4_GetLastError(m_lPort);
                        str = "PlayM4_Stop failed, error code= " + iLastErr;
                        DebugInfo(str);
                    }
                    if (!PlayCtrl.PlayM4_CloseStream(m_lPort))
                    {
                        iLastErr = PlayCtrl.PlayM4_GetLastError(m_lPort);
                        str = "PlayM4_CloseStream failed, error code= " + iLastErr;
                        DebugInfo(str);
                    }
                    if (!PlayCtrl.PlayM4_FreePort(m_lPort))
                    {
                        iLastErr = PlayCtrl.PlayM4_GetLastError(m_lPort);
                        str = "PlayM4_FreePort failed, error code= " + iLastErr;
                        DebugInfo(str);
                    }
                    m_lPort = -1;
                }

                DebugInfo("NET_DVR_StopRealPlay succ!");
                m_lRealHandle = -1;
                //btnPreview.Text = "Live View";
                RealPlayWnd.Invalidate();//刷新窗口 refresh the window

                stop();
            }
            return;
        }

        public void DrawFunCallBack(int port, IntPtr hDc, int pUser)
        {

            Graphics g = Graphics.FromHdc(hDc);

            lock (o)
            {
                if (_points != null && _points.Count > 0)
                {

                    Pen m_pen = new Pen(Color.Red, 3);

                    foreach (MonitorPoint _po in _points)
                    {

                        float _x = float.Parse(_po.X.ToString());
                        float _y = float.Parse(_po.Y.ToString());
                        float _w = float.Parse(_po.W.ToString());
                        float _h = float.Parse(_po.H.ToString());

                        if (_isShowRec)
                        {
                            g.DrawRectangle(m_pen, _x, _y, _w, _h);
                        }


                        if (!_isShowInfo)
                            return;

                        //Image image = Properties.Resources.menutree_bg;
                        //Rectangle r = new Rectangle((int)_x, (int)_y, image.Size.Width, image.Size.Height / 2);
                        //g.DrawImage(image, r);

                        //String str1 = "你快跑呀";
                        //Font font = new Font("微软雅黑", 16);
                        //SolidBrush sbrush1 = new SolidBrush(Color.White);

                        //StringFormat format1 = new StringFormat();
                        ////指定字符串的水平对齐方式
                        //format1.Alignment = StringAlignment.Far;
                        ////表示字符串的垂直对齐方式
                        //format1.LineAlignment = StringAlignment.Center;

                        //g.DrawString(str1, font, sbrush1, new PointF((int)_x + 125, (int)_y + 50), format1);
                    }
                }
            }
        }

        private void start()
        {
            _flag = true;
            Thread _thread = new Thread(startBmp);
            _thread.Start();

        }

        private void stop()
        {
            _flag = false;

            btn_Exit_Click();
        }

        private void btn_Exit_Click()
        {
            //停止预览
            if (m_lRealHandle >= 0)
            {
                CHCNetSDK.NET_DVR_StopRealPlay(m_lRealHandle);
                m_lRealHandle = -1;
            }

            //注销登录
            if (m_lUserID >= 0)
            {
                CHCNetSDK.NET_DVR_Logout(m_lUserID);
                m_lUserID = -1;
            }

            CHCNetSDK.NET_DVR_Cleanup();
        }

        int _mSelectedIndex = 1;


        private void startBmp()
        {
            while (_flag)
            {

                if (m_lRealHandle < 0)
                {
                    return;
                }

                int _width = 0;
                int nID = 0;

                int _height = 0;

                if (!PlayCtrl.PlayM4_GetPictureSize(m_lPort, ref _width, ref _height))
                {
                    iLastErr = PlayCtrl.PlayM4_GetLastError(m_lPort);
                    str = "PlayM4_GetPictureSize failed, error code= " + iLastErr;
                    //DebugInfo(str);
                    continue;
                }


                uint iActualSize = 0;

                uint nBufSize = (uint)(_width * _height) * 5;

                byte[] pBitmap = new byte[nBufSize];

                if (!PlayCtrl.PlayM4_GetBMP(m_lPort, pBitmap, nBufSize, ref iActualSize))
                {
                    iLastErr = PlayCtrl.PlayM4_GetLastError(m_lPort);
                    str = "PlayM4_GetBMP failed, error code= " + iLastErr;
                }
                else
                {

                    if (!_isInit)
                    {
                        //DetectHelper.QH_Init(1920, 1080);
                        byte[] cfgfile = System.Text.Encoding.Default.GetBytes(System.Environment.CurrentDirectory + "/data/detect.cfg");
                        byte[] weigthfile = System.Text.Encoding.Default.GetBytes(System.Environment.CurrentDirectory + "/data/detect.weights");
                        nID = QH_DNN_Helper.InitNet_DNN(new byte[10], cfgfile, weigthfile, (float)0.05, (float)0.5, 416, 416, -1);
                        _isInit = true;
                    }

                    Bitmap _bm = SizeImage(BytesToBitmap(pBitmap), this.RealPlayWnd.Width, this.RealPlayWnd.Height);

                    if (_bm == null)
                        continue;

                    byte[] _bm1 = Bitmap2Byte(_bm);



                    int size = Marshal.SizeOf(typeof(RESULT_CPLUS)) * 100;
                    byte[] bytesi = new byte[size];
                    //IntPtr pBuff = Marshal.AllocHGlobal(size);
                    //int _res = DetectHelper.QHGSForDetect(_bm1, _bm1.Length, pBuff);




                    GSALGITEMIMGEX_DNN[] ret = new GSALGITEMIMGEX_DNN[150];

                    int count = QH_DNN_Helper.QHGSSharp_DNN(_bm1, _bm1.Length, ret, nID);

                    //GSALGITEMIMGEX_DNN[] pClass = new GSALGITEMIMGEX_DNN[count];
                    List<RESULT_CPLUS> pClass = new List<RESULT_CPLUS>();
                    for (int i = 0; i < count; i++)
                    {
                        //IntPtr ptr = new IntPtr(pBuff.ToInt64() + Marshal.SizeOf(typeof(RESULT_CPLUS)) * i);
                        //IntPtr ptr = new IntPtr(pBuff.ToInt64() + Marshal.SizeOf(typeof(GSALGITEMIMGEX_DNN)) * i);
                        //pClass[i] = (GSALGITEMIMGEX_DNN)Marshal.PtrToStructure(ptr, typeof(GSALGITEMIMGEX_DNN));
                        //pClass[i] = (RESULT_CPLUS)Marshal.PtrToStructure(ptr, typeof(RESULT_CPLUS));


                        pClass.Add(new RESULT_CPLUS
                        {
                            x = ret[i].roi.left,
                            y = ret[i].roi.top,
                            w = ret[i].roi.right,
                            h = ret[i].roi.bottom,
                            objID = ret[i].nObjId,
                            score = ret[i].fConfidenc
                        });
                    }
                    // Marshal.FreeHGlobal(pBuff); // 释放内存 

                    // TODO  GSALGITEMIMGEX_DNN 转换   RESULT_CPLUS

                    sendMsg(pClass.ToArray());

                }
            }

        }

        public Bitmap SizeImage(Bitmap srcImage, int width, int height)
        {
            if (srcImage.Width == width && srcImage.Height == height)
            {
                return new Bitmap(srcImage);
            }
            Graphics g = null;
            try
            {

                // 要保存到的图片 
                Bitmap b = new Bitmap(width, height);
                g = Graphics.FromImage(b);
                // 插值算法的质量 
                g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                g.DrawImage(srcImage, new Rectangle(0, 0, width, height), new Rectangle(0, 0, srcImage.Width, srcImage.Height), GraphicsUnit.Pixel);
                return b;
            }
            catch (Exception)
            {
                return null;
            }
            finally
            {
                if (g != null)
                {
                    g.Dispose();
                }
            }
        }


        public Bitmap BytesToBitmap(byte[] Bytes)
        {
            MemoryStream stream = null;
            try
            {
                stream = new MemoryStream(Bytes);
                return new Bitmap((Image)new Bitmap(stream));
            }
            catch (ArgumentNullException ex)
            {
                throw ex;
            }
            catch (ArgumentException ex)
            {
                throw ex;
            }
            finally
            {
                stream.Close();
            }
        }


        public byte[] Bitmap2Byte(Bitmap bitmap)
        {


            using (MemoryStream stream = new MemoryStream())
            {
                bitmap.Save(stream, ImageFormat.Jpeg);
                byte[] data = new byte[stream.Length];
                stream.Seek(0, SeekOrigin.Begin);
                stream.Read(data, 0, Convert.ToInt32(stream.Length));
                return data;
            }
        }


        private byte[] test(string path)
        {
            FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read); //将图片以文件流的形式进行保存
            BinaryReader br = new BinaryReader(fs);
            byte[] imgBytesIn = br.ReadBytes((int)fs.Length); //将流读入到字节数组中
            return imgBytesIn;
        }

        private void sendMsg(RESULT_CPLUS[] _plus)
        {
            List<MonitorPoint> _points = new List<MonitorPoint>();
            foreach (RESULT_CPLUS _p in _plus)
            {

                if (_p.score < 0.3)
                    continue;

                MonitorPoint _point = new MonitorPoint();
                _point.X = _p.x;
                _point.Y = _p.y;
                _point.W = _p.w;
                _point.H = _p.h;
                _point.S = _p.score;
                _points.Add(_point);
            }

            form2_MyEvent(_points);
        }

        void form2_MyEvent(List<MonitorPoint> points)
        {
            if (_points == null)
                _points = new Queue<MonitorPoint>();
            // 画图

            lock (o)
            {
                _points.Clear();

                foreach (MonitorPoint _point in points)
                {
                    _points.Enqueue(_point);
                }

            }
        }

        private void RealPlayWnd_Click(object sender, EventArgs e)
        {

        }



        // 窗体位置监听(用于将多个窗体保持一致性)
        private void HCPreview_LocationChanged(object sender, EventArgs e)
        {
            if (_tranForm != null)
            {
                this._tranForm.Location = this.Location;
            }

            if (_iconForm != null)
            {
                this._iconForm.Location = this.Location;
            }

            if (_monitorForm != null)
            {
                this._monitorForm.Location = this.Location;
            }
        }

        // 窗体大小监听(用于将多个窗体保持一致性)
        private void HCPreview_Resize(object sender, EventArgs e)
        {
            if (this._tranForm != null)
            {

                this._tranForm.Size = this.Size;

            }

            if (this._iconForm != null)
            {

                this._iconForm.Size = this.Size;

            }

            if (this._monitorForm != null)
            {

                this._monitorForm.Size = this.Size;

            }
        }

        //Windows系统消息，winuser.h文件中有WM_...的定义
        //十六进制数字，0x是前导符后面是真正的数字
        const int WM_SYSCOMMAND = 0x0112;
        //winuser.h文件中有SC_...的定义
        const int SC_CLOSE = 0xF060;


        protected override void WndProc(ref Message msg)
        {
            // 监听关闭窗体事件   
            if (msg.Msg == WM_SYSCOMMAND && ((int)msg.WParam == SC_CLOSE))
            {
                if (OnClose != null)
                    OnClose();

                if (_iconForm != null)
                    _iconForm.Close();

                if (_tranForm != null)
                    _tranForm.Close();

                if (_monitorForm != null)
                    _monitorForm.Close();

                this.Close();
            }
            base.WndProc(ref msg);
        }
    }
}
