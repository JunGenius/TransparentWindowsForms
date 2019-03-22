
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using TransparentWindowsForms.QhControl.entity;
using TransparentWindowsForms.Properties;

/************************************************************************
    * Copyright (c) 2018 All Rights Reserved.
    * 创建人： QuJun
    * 创建时间：2018/12/27
    * 描述： 自定义下拉列表（配合滚动条使用）
************************************************************************/

namespace TransparentWindowsForms.QhControl.scrollcombox
{
    class UserListBox : ListBox
    {
        private readonly ListBoxItemCollection m_Items;

        public ListBoxItem mouseItem;


        private int _pos = -1;

        public event Action<ListEventItemInfo> OnListItemClick;

        private int _mItemHeight = 30;
        public UserListBox()
        {

            m_Items = new ListBoxItemCollection(this);

            base.DrawMode = DrawMode.OwnerDrawVariable;
            SetStyle(ControlStyles.UserPaint, true);
            SetStyle(ControlStyles.DoubleBuffer, true); // 双缓冲
            SetStyle(ControlStyles.OptimizedDoubleBuffer, true); // 双缓冲   
            SetStyle(ControlStyles.ResizeRedraw, true); // 调整大小时重绘
            SetStyle(ControlStyles.AllPaintingInWmPaint, true); // 禁止擦除背景. 
            SetStyle(ControlStyles.SupportsTransparentBackColor, true); // 开启控件透明
        }

        public new ListBoxItemCollection Items
        {
            get { return m_Items; }
        }

        public int GetItemHeight()
        {
            return _mItemHeight;
        }

        internal ObjectCollection OldItemSource
        {
            get { return base.Items; }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics g = e.Graphics;


            Image image = Resources.combox_scroll_list_bg;
            Rectangle r = new Rectangle((int)0, (int)0, this.Width, this.Height);
            g.DrawImage(image, r);
            

            // you can set SeletedItem background
            if (Focused && SelectedItem != null)
            {
            }

            for (int i = 0; i < Items.Count; i++)
            {
                Rectangle bounds = GetItemRectangle(i);

                if (mouseItem == Items[i])
                {
                    Color leftColor = Color.FromArgb(200, 192, 224, 248);
                    using (var brush = new SolidBrush(leftColor))
                    {
                        g.FillRectangle(brush, new Rectangle(bounds.X, bounds.Y, bounds.Width, bounds.Height));
                    }
                }

                int fontLeft = bounds.Left + 45;
                var font = new Font("微软雅黑", 10);

                g.DrawString(Items[i].Mac, font, new SolidBrush(Color.Snow), fontLeft,
                             bounds.Top + 5);

                g.InterpolationMode = InterpolationMode.HighQualityBilinear;
                g.DrawImage(Items[i].IsCheck ? Resources.combox_scroll_check : Resources.combox_scroll_un_check, new Rectangle(bounds.X + 15, (bounds.Height - 20) / 2 + bounds.Top, 20, 20));

            }


            base.OnPaint(e);
        }

        protected override void OnMeasureItem(MeasureItemEventArgs e)
        {
            base.OnMeasureItem(e);
            if (Items.Count > 0)
            {
                ListBoxItem item = Items[e.Index];
                e.ItemHeight = _mItemHeight;
            }
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            for (int i = 0; i < Items.Count; i++)
            {
                Rectangle bounds = GetItemRectangle(i);
                var deleteBounds = new Rectangle(bounds.X, bounds.Y, bounds.Width, bounds.Height);
                if (bounds.Contains(e.X, e.Y))
                {
                    if (Items[i] != mouseItem)
                    {
                        mouseItem = Items[i];
                    }

                    if (deleteBounds.Contains(e.X, e.Y))
                    {
                        mouseItem.IsFocus = true;
                        Cursor = Cursors.Hand;
                        _pos = i;
                    }
                    else
                    {
                        mouseItem.IsFocus = false;
                        Cursor = Cursors.Arrow;
                    }

                    Invalidate();
                    break;
                }
            }
        }

        protected override void OnMouseClick(MouseEventArgs e)
        {
            base.OnMouseClick(e);

            if (mouseItem.IsFocus)
            {
                ListBoxItem deleteItem = mouseItem;

                if (deleteItem.IsCheck)
                {
                    deleteItem.IsCheck = false;
                    //deleteItem.Image = Resources.un_check;

                    if (OnListItemClick != null)
                        OnListItemClick(new ListEventItemInfo(_pos, false));
                }
                else
                {
                    deleteItem.IsCheck = true;
                    //deleteItem.Image = Resources.check_box;

                    if (OnListItemClick != null)
                        OnListItemClick(new ListEventItemInfo(_pos, true));
                }

                Invalidate();

            }
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);
            mouseItem = null;
            Invalidate();
        }
    }
}
