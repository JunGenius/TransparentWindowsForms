
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using TransparentWindowsForms.QhControl.entity;
using TransparentWindowsForms.QhControl.monitoreye;
using TransparentWindowsForms.QhControl.scrollcombox;
using TransparentWindowsForms.Properties;
using DSkin.Forms;
using DSkin.Controls;


/*=================================================================
* Author: QUJUN
* CreatedTime: 2019/1/9 14:26:23
* Description:  透明窗体 (监控点信息)
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
    public partial class TransparentDCFrom : DSkinForm
    {


        private List<CustomDSEyeBox> _videoMonitorEyeBoxs = new List<CustomDSEyeBox>();

        private List<CustomDSEyeBox> _kaKouEyeBoxs = new List<CustomDSEyeBox>();

        private List<CustomDSEyeBox> _roadEyeBoxs = new List<CustomDSEyeBox>();

        private List<CustomDSEyeBox> _ydpEyeBoxs = new List<CustomDSEyeBox>();

        private List<CustomDSEyeBox> _dxpEyeBoxs = new List<CustomDSEyeBox>();

        private List<CustomDSEyeBox> _jzwEyeBoxs = new List<CustomDSEyeBox>();

        private List<PopLocationInfo> _pops = new List<PopLocationInfo>();

        private List<PopMoveLocationInfo> _movePops = new List<PopMoveLocationInfo>();

        private HCPreview _hc = null;
        public TransparentDCFrom(HCPreview _hc)
        {
            InitializeComponent();

            this._hc = _hc;

            this._hc.OnClose += _hc_OnClose;

            this._hc.OnMinimize += _hc_OnMinimize;

            this._hc.OnMaximize += _hc_OnMaximize;

            this._hc.OnNormal += _hc_OnNormal; ;

            this.LocationChanged += TransparentForm_LocationChanged;

            this.Load += TransparentDCFrom_Load;

            this.SizeChanged += TransparentDCFrom_SizeChanged;

            this.Size = _hc.Size;

            this.ShowInTaskbar = false;

            initCombox();
        }



        private void TransparentDCFrom_SizeChanged(object sender, EventArgs e)
        {
        }


        private void TransparentDCFrom_Load(object sender, EventArgs e)
        {
        }

        private void _hc_OnMaximize()
        {

        }

        private void _hc_OnMinimize()
        {
            foreach (PopMoveLocationInfo form in _movePops)
            {
                form.PopForm.SetMinimize();
            }
        }

        private void _hc_OnNormal()
        {
            foreach (PopMoveLocationInfo form in _movePops)
            {
                form.PopForm.SetNoraml();
            }
        }

        private void _hc_OnClose()
        {
            foreach (PopMoveLocationInfo form in _movePops)
            {
                form.PopForm.Close();
            }
        }

        private void initCombox()
        {


            addVideoMonitor("视频监控", Resources.eyebox_monitor_text_type_1, 5, 0, true);

            addVideoMonitor("卡口监控", Resources.eyebox_monitor_text_type_2, 5, 1, false);

            addVideoMonitor("道路施工", Resources.eyebox_monitor_text_type_3, 5, 2, false);

            addVideoMonitor("诱导屏", Resources.eyebox_monitor_text_type_4, 5, 3, false);

            addVideoMonitor("导向屏", Resources.eyebox_monitor_text_type_5, 5, 4, false);

            addVideoMonitor("建筑物", Resources.eyebox_monitor_text_type_6, 5, 5, false);

        }


        private void addVideoMonitor(string info, Image icon, int value, int type, bool isVisible)
        {
            for (int i = 0; i < value; i++)
            {
                int x = getRandomLocationX(type, i);
                int y = getRandomLocationY(type, i);



                int direction = 0;

                if (this.Width - x < 50)
                    direction = 1;

                CustomDSEyeBox point = getEyeBox(info + (i + 1), direction, icon, x, y, isVisible);
                this.Controls.Add(point);

                switch (type)
                {
                    case 0:
                        _videoMonitorEyeBoxs.Add(point);
                        break;
                    case 1:
                        _kaKouEyeBoxs.Add(point);
                        break;
                    case 2:
                        _roadEyeBoxs.Add(point);
                        break;
                    case 3:
                        _ydpEyeBoxs.Add(point);
                        break;
                    case 4:
                        _dxpEyeBoxs.Add(point);
                        break;
                    case 5:
                        _jzwEyeBoxs.Add(point);
                        break;
                }
            }
        }


        private CustomDSEyeBox getEyeBox(string text, int direction, Image image, int x, int y, bool isVisible)
        {
            CustomDSEyeBox box = new CustomDSEyeBox();

            box.Location = new Point(x, y);

            box.EyeBoxText = text;

            box.EyeBoxDirection = direction;

            box.EyeBoxTextIcon = image;

            box.Visible = isVisible;

            box.OnEyeBoxClick += OnEyeBoxClick;

            return box;
        }



        private int getRandomLocationX(int type, int i)
        {
            int x = 0;
            switch (type)
            {
                case 0:
                    x = 60 + 100 * (i + 1);
                    break;
                case 1:
                    x = 80 + 200 * (i + 1);
                    break;
                case 2:
                    x = 100 + 180 * (i + 1);
                    break;
                case 3:
                    x = 100 + 260 * (i + 1);
                    break;
                case 4:
                    x = 145 + 190 * (i + 1);
                    break;
                case 5:
                    x = 100 + 180 * (i + 1);
                    break;
            }
            return x > 1100 ? 1100 : x;
        }

        private int getRandomLocationY(int type, int i)
        {
            int y = 0;
            switch (type)
            {
                case 0:
                    y = 100 + 100 * i;
                    break;
                case 1:
                    y = 120 + 100 * i;
                    break;
                case 2:
                    y = 140 + 190 * i;
                    break;
                case 3:
                    y = 110 + 65 * i;
                    break;
                case 4:
                    y = 153 + 33 * i;
                    break;
                case 5:
                    y = 167 + 56 * i;
                    break;
            }
            return y > 600 ? 600 : y;
        }

        int i = 0;
        private void OnEyeBoxClick(EyeBoxClickInfo info)
        {
            if (i % 2 == 0)
                showPopForm(info);
            else
                showPopMoveForm(info);

            i++;
        }



        private void showPopMoveForm(EyeBoxClickInfo info)
        {

            PopMoveForm pop = new PopMoveForm();

            Point point = new Point(info.X, info.Y);

            pop.Location = point;

            pop.Owner = this;

            pop.Size = new Size(500, 300);

            pop.BringToFront();

            pop.WindowState = this.WindowState;

            pop.OnRemove += PopMove_OnRemove;

            pop.Show();

            PopMoveLocationInfo popInfo = new PopMoveLocationInfo(point, pop);

            _movePops.Add(popInfo);
        }

        private void PopMove_OnRemove(PopMoveLocationInfo obj)
        {
            _movePops.Remove(obj);
        }

        private void showPopForm(EyeBoxClickInfo info)
        {

            PopForm pop = new PopForm();

            Point point;

            if (info.Type == 0)
                point = new Point(info.X, info.Y);
            else
                point = new Point(info.X - pop.Width, info.Y);

            PopLocationInfo popInfo = new PopLocationInfo(point, pop);

            pop.SetPopLocationInfo(popInfo);

            pop.Owner = this;

            pop.Size = this.Size;

            pop.Dock = DockStyle.Fill;

            pop.OnRemove += Pop_OnRemove;

            pop.BringToFront();

            pop.Show();

            _pops.Add(popInfo);
        }

        private void Pop_OnRemove(PopLocationInfo obj)
        {
            _pops.Remove(obj);
        }


        private void TransparentForm_LocationChanged(object sender, EventArgs e)
        {

            foreach (PopLocationInfo info in _pops)
            {
                info.PopForm.Location = this.Location;
            }
        }



        public void SetIsShowMonitor(bool isShow, string info)
        {
            switch (info)
            {
                case "视频监控":
                    setMonitorVisible(_videoMonitorEyeBoxs, isShow);
                    break;
                case "卡口监控":
                    setMonitorVisible(_kaKouEyeBoxs, isShow);
                    break;
                case "道路施工":
                    setMonitorVisible(_roadEyeBoxs, isShow);
                    break;
                case "诱导屏":
                    setMonitorVisible(_ydpEyeBoxs, isShow);
                    break;
                case "导向屏":
                    setMonitorVisible(_dxpEyeBoxs, isShow);
                    break;
                case "建筑物":
                    setMonitorVisible(_jzwEyeBoxs, isShow);
                    break;
            }

        }

        private void setMonitorVisible(List<CustomDSEyeBox> collection, bool isShow)
        {

            if (collection == null)
                return;

            foreach (CustomDSEyeBox box in collection)
            {
                box.Visible = isShow;
            }
        }

    }
}
