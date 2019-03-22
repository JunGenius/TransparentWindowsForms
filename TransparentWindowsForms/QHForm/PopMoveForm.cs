
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using TransparentWindowsForms.QhControl;
using TransparentWindowsForms.Properties;
using DSkin.Forms;
using DSkin.Controls;
using TransparentWindowsForms.QhControl.entity;



/************************************************************************
    * Copyright (c) 2018 All Rights Reserved.
    * 创建人： QuJun
    * 创建时间：2018/12/27
    * 描述： 播放视频小窗口 (可拖拽、放大、缩小)
    * 绘制边框边角、底部视频边框线条，放大、缩小、关闭按钮，防止拖拽失真
    * 头部背景图放大过程中会稍微出现失真情况。
    * 视频需加载在ControlHost控件中，否则无法播放。
************************************************************************/

namespace TransparentWindowsForms
{


    public partial class PopMoveForm : DSkinForm
    {


        private delegate void myDelegate(string str);//声明委托

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

        

        private Image _top_left_image = Resources.top_left_icon;

        private Image _top_right_image = Resources.top_right_icon;

        private Image _close = Resources.full_monitor_close;

        private Image _minimize = Resources.full_monitor_maxmize_back;

        private Image _maxmize = Resources.full_monitor_maxmize;

        private Image _cur_icon_image = null;

        private Image _bottom_left_image = Resources.bottom_left_icon;

        private Image _bottom_right_image = Resources.bottom_right_icon;

        private Image _line = Resources.full_monitor_line;

        private Image _top_panel_bg = Resources.full_monitor_top_bg;

        private Image _bottom_panel_bg = Resources.picture_menu_list_detail_bg;

        private int _rec_width = 1; // 矩形框边框宽度

        private Pen _bottom_rec_pen = null;

        private int _rec_dif = 6; // 边框距离

        private int _icon_dif = 10; // 图标间距 (最大化 ,还原，关闭)

        private EyePictureBox _eye_box_pic = null;

        private ControlHost _control_host = null;

        private int _width = 0;

        private int _height = 0;

        private int _close_point_x = 0;

        private int _close_point_y = 0;

        private int _minimize_point_x = 0;

        private int _minimize_point_y = 0;

        private int _pic_box_dif = 1;

        private FormWindowState _form_state = FormWindowState.Normal;

        public event Action<PopMoveLocationInfo> OnRemove;

        private PopMoveLocationInfo _location_info = null;

        public PopMoveForm()
        {
            InitializeComponent();

            this.Load += PopForm_Load;

            this.LocationChanged += PopMoveForm_LocationChanged;

            this.Resize += PopMoveForm_Resize;

            this.MouseDown += PopMoveForm_MouseDown;

            this.ShowInTaskbar = false;

            init();
        }


        public PopMoveLocationInfo PopLocationInfo
        {
            get { return _location_info; }
            set
            {
                _location_info = value;
            }
        }


        private void PopMoveForm_Resize(object sender, EventArgs e)
        {
            _width = this.Width;

            _height = this.Height;

            _control_host.Size = new Size(_width - _rec_dif * 2 - _rec_width, _height - _rec_dif * 2 - _top_panel_bg.Height - _rec_width);

            _eye_box_pic.Size = _control_host.Size;

            _eye_box_pic.Location = new Point(0, 0);
        }



        private void init()
        {
            this.BackColor = Color.Transparent;

            _bottom_rec_pen = new Pen(Color.FromArgb(25, 98, 186), _rec_width);


            _control_host = new ControlHost();

            _control_host.Location = new Point(_rec_dif + _pic_box_dif, _rec_dif + _top_panel_bg.Height + _pic_box_dif);

            _control_host.BackColor = Color.Aqua;

            _eye_box_pic = new EyePictureBox();

            _eye_box_pic.BackColor = Color.DarkGray;

            _control_host.Controls.Add(_eye_box_pic);

            this.Controls.Add(_control_host);
        }

        protected override void OnLayeredPaint(PaintEventArgs e)
        {
            // 顶部背景
            e.Graphics.DrawImage(_top_panel_bg, new Rectangle(new Point(_rec_dif, _rec_dif), new Size(_width - _rec_dif * 2, _top_panel_bg.Height)));

            _close_point_x = _width - _rec_dif - _icon_dif - _close.Width - 5;

            _close_point_y = _rec_dif + (_top_panel_bg.Height - _close.Height) / 2 + 5;

            // 关闭
            e.Graphics.DrawImage(_close, new Rectangle(new Point(_close_point_x, _close_point_y), _close.Size));

            _minimize_point_x = _close_point_x - _icon_dif - _minimize.Width;

            _minimize_point_y = _close_point_y;

            _cur_icon_image = _form_state == FormWindowState.Normal ? _maxmize : _minimize;

            // 最大化
            e.Graphics.DrawImage(_cur_icon_image, new Rectangle(new Point(_minimize_point_x, _minimize_point_y), _cur_icon_image.Size));

            // 上下边框
            e.Graphics.DrawLine(_bottom_rec_pen, new Point(_rec_dif, _rec_dif + _top_panel_bg.Height), new Point(_width - _rec_dif, _rec_dif + _top_panel_bg.Height));

            e.Graphics.DrawLine(_bottom_rec_pen, new Point(_rec_dif, _height - _rec_dif), new Point(_width - _rec_dif, _height - _rec_dif));

            // 左右边框
            e.Graphics.DrawLine(_bottom_rec_pen, new Point(_rec_dif, _rec_dif + _top_panel_bg.Height), new Point(_rec_dif, _height - _rec_dif));

            e.Graphics.DrawLine(_bottom_rec_pen, new Point(_width - _rec_dif, _rec_dif + _top_panel_bg.Height), new Point(_width - _rec_dif, _height - _rec_dif));

            // 边角框
            e.Graphics.DrawImage(_top_left_image, new Rectangle(new Point(0, 0), new Size(_top_left_image.Width, _top_left_image.Height)));

            e.Graphics.DrawImage(_top_right_image, new Rectangle(new Point(this.Width - _top_left_image.Width, 0), new Size(_top_right_image.Width, _top_right_image.Height)));

            e.Graphics.DrawImage(_bottom_left_image, new Rectangle(new Point(0, this.Height - _bottom_left_image.Height), new Size(_bottom_left_image.Width, _bottom_left_image.Height)));

            e.Graphics.DrawImage(_bottom_right_image, new Rectangle(new Point(this.Width - _bottom_right_image.Width, this.Height - _bottom_right_image.Height), new Size(_bottom_right_image.Width, _bottom_right_image.Height)));

        }

        public void SetMinimize()
        {
            this.WindowState = FormWindowState.Minimized;
        }

        public void SetMaximize()
        {
            this.WindowState = FormWindowState.Maximized;
        }

        public void SetNoraml()
        {
            this.WindowState = FormWindowState.Normal;
        }

        private void PopMoveForm_LocationChanged(object sender, EventArgs e)
        {

        }

        private void PopMoveForm_MouseDown(object sender, MouseEventArgs e)
        {
            // 关闭
            if (e.X > _close_point_x && e.X < _close_point_x + _close.Width && e.Y > _close_point_y && e.Y < _close_point_y + _close.Height)
            {
                btn_Exit_Click();

                if (OnRemove != null)
                    OnRemove(_location_info);

                this.Close();
                return;
            }

            // 最大化
            if (e.X > _minimize_point_x && e.X < _minimize_point_x + _cur_icon_image.Width && e.Y > _minimize_point_y && e.Y < _minimize_point_y + _cur_icon_image.Height)
            {
                switch (_form_state)
                {
                    case FormWindowState.Normal:
                        _form_state = FormWindowState.Maximized;
                        break;
                    case FormWindowState.Maximized:
                        _form_state = FormWindowState.Normal;
                        break;
                }

                this.WindowState = _form_state;
                return;
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

        public void DebugInfo(string str)
        {
            if (str.Length > 0)
            {
                str += "\n";
                Console.WriteLine(str);
            }
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
                lpPreviewInfo.hPlayWnd = _eye_box_pic.Handle;//预览窗口 live view window
                //lpPreviewInfo.hPlayWnd = _control_host.Handle;
                lpPreviewInfo.lChannel = iChannelNum[(int)iSelIndex];//预览的设备通道 the device channel number
                lpPreviewInfo.dwStreamType = 1;//码流类型：0-主码流，1-子码流，2-码流3，3-码流4，以此类推
                lpPreviewInfo.dwLinkMode = 0;//连接方式：0- TCP方式，1- UDP方式，2- 多播方式，3- RTP方式，4-RTP/RTSP，5-RSTP/HTTP 
                lpPreviewInfo.bBlocked = true; //0- 非阻塞取流，1- 阻塞取流
                lpPreviewInfo.dwDisplayBufNum = 15; //播放库显示缓冲区最大帧数


                IntPtr pUser = IntPtr.Zero;//用户数据 user data 

                //打开预览 Start live view 
                m_lRealHandle = CHCNetSDK.NET_DVR_RealPlay_V40(m_lUserID, ref lpPreviewInfo, null/*RealData*/, pUser);



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

                    _eye_box_pic.IsShowIcon = false;
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
                //_eye_box_pic.GetPicBox().Invalidate();//刷新窗口 refresh the window
                _control_host.Invalidate();

            }
            return;
        }


        int _mSelectedIndex = 1;

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

        private void PopForm_Load(object sender, EventArgs e)
        {
            Thread t = new Thread(() =>
            {

                Action<int> action = (data) =>
                {
                    loginMonitor();
                };
                Invoke(action, i);

            });
            t.Start();
        }


        private void loginMonitor()
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

    }
}
