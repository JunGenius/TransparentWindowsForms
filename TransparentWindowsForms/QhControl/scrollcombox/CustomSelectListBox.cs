using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using TransparentWindowsForms.QhControl.entity;
using TransparentWindowsForms.QhControl.tool;


/************************************************************************
    * Copyright (c) 2018 All Rights Reserved.
    * 创建人： QuJun
    * 创建时间：2018/12/27
    * 描述： 带滚动条下拉列表
************************************************************************/

namespace TransparentWindowsForms.QhControl.scrollcombox
{
    public partial class CustomSelectListBox : UserControl
    {

        private CustomScrollbar _scorll = null; // 自定义滑动模块

        private UserListBox _combox; // 自定义列表

        private float _scroll_per = 0; // 滑动比例

        private List<ListBoxItem> _items = null;

        public event Action<ListEventItemInfo> OnListBoxItemClick;
        public CustomSelectListBox(List<ListBoxItem> items, int width, int height)
        {
            InitializeComponent();

            _items = items;

            this.Height = height;

            this.Width = width;

            initControl();
        }


        public void initControl()
        {
            _combox = new UserListBox();

            _combox.Location = new System.Drawing.Point(this.Bounds.X, this.Bounds.Y);

            _combox.Size = new Size(this.Width - 20, this.Height);

            _combox.BorderStyle = BorderStyle.None;

            _combox.HorizontalScrollbar = false;

            _combox.ForeColor = Color.Snow;

            _combox.MouseWheel += _combox_MouseWheel;

            _combox.OnListItemClick += OnListItemClick;

            if (_items != null)
            {
                for (int i = 0; i < _items.Count; i++)
                {
                    _combox.Items.Add(_items[i]);
                }
            }

            if (_combox != null && _combox.GetItemHeight() * _items.Count > this.Height)
            {
                SCROLLINFO scroll = tvImageListScrollInfo;

                _scorll = new CustomScrollbar(_combox.Items.Count, _combox.GetItemHeight());

                _scorll.Location = new System.Drawing.Point(this.Bounds.X + 110, this.Bounds.Y);

                _scorll.Size = new Size(20, this.Height);

                _scorll.Scroll += customScrollbar1_Scroll;

                _scorll.BringToFront();

                this.Controls.Add(_scorll);
            }

            this.Controls.Add(_combox);

        }


        // 滑动模块滑动事件
        private void customScrollbar1_Scroll(object sender, EventArgs m)
        {
            if (_scorll == null)
                return;

            SCROLLINFO info = tvImageListScrollInfo;

            _scroll_per = 9 / ((info.nMax - info.nMin) / 10.0f);

            info.nPos = (int)(_scorll.Value / _scroll_per);

            Win32API.SetScrollInfo(_combox.Handle, (int)ScrollBarDirection.SB_VERT, ref info, true);

            Win32API.PostMessage(_combox.Handle, Win32API.WM_VSCROLL, Win32API.MakeLong((short)Win32API.SB_THUMBTRACK, (short)(info.nPos)), 0);

            _combox.Invalidate();
        }

        // 列表滑动事件
        private void _combox_MouseWheel(object sender, MouseEventArgs m)
        {
            if (_scorll == null)
                return;

            if (m.Delta > 0)
            {
                SetImageListScroll(1);
                return;
            }

            SetImageListScroll(0);

        }
        // 设置列表滑动 （type 0: 向下滑动 1;向上滑动）
        private void SetImageListScroll(int type)
        {

            SCROLLINFO info = tvImageListScrollInfo;

            //  有待优化 ( 滑动监听的滚动位置第一次总是0,无法准确计算滑动位置)
            if (info.nMax > 0)
            {
                int pos = info.nPos;

                if (type == 0)
                {
                    pos = pos >= info.nMax ? info.nMax : pos + 3;
                }
                else
                {
                    pos = pos < 0 ? 0 : pos - 3;
                }

                int v = (int)((type == 0 ? pos >= info.nMax - 2 ? info.nMax : pos : pos <= 2 ? 0 : pos) * (_scroll_per));

                _scorll.Value = v;
            }
        }

        // 获取滚动信息
        public SCROLLINFO tvImageListScrollInfo
        {

            get
            {

                SCROLLINFO si = new SCROLLINFO();

                si.cbSize = (uint)Marshal.SizeOf(si);

                si.fMask = (int)(ScrollInfoMask.SIF_ALL);

                Win32API.GetScrollInfo(_combox.Handle, (int)ScrollBarDirection.SB_VERT, ref si);

                return si;

            }

        }

        private void OnListItemClick(ListEventItemInfo info)
        {
            if (OnListBoxItemClick != null)
                OnListBoxItemClick(info);
        }

    }
}
