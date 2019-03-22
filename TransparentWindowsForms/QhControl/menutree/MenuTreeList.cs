using DSkin.Controls;
using System;
using System.Drawing;
using System.Windows.Forms;
using TransparentWindowsForms.Properties;


/************************************************************************
    * Copyright (c) 2018 All Rights Reserved.
    * 创建人： QuJun
    * 创建时间：2018/12/27
    * 描述：  树形结构
************************************************************************/


namespace TransparentWindowsForms.QhControl.menutree
{
    public partial class MenuTreeList : DSkinUserControl
    {

        private int _menu_button_point_x = 0;

        private int _menu_button_point_y = 0;

        private bool _isShowMenu = false;

        private bool _isShowMenuButton = true;

        private Image _bg_image = Resources.menutree_bg;

        private Image _menu_left_button = Resources.menutree_left_button;

        private Image _menu_left_arrow = Resources.menutree_left_close;

        private int _menu_left_point_x = 0;

        private int _menu_left_point_y = 0;

        private string _menu_title = "设备列表";

        private DSkinTreeView _tree_view = null;

        private DSkinPanel _menu_tree = null;

        private DSkinPanel _left_menu_button = null;

        private DSkinLabel _label = null;

        private DSkinLabel _close = null;

        private DSkinPanel _left_tab = null;

        private DSkinPanel _right_tab = null;

        public MenuTreeList()
        {
            InitializeComponent();

            this.Load += MenuTreeList_OnLoad;

            this.MouseDown += Form_MouseDown;

            init();
        }


        public string Menu_Title
        {
            get { return _menu_title; }
            set
            {
                _menu_title = value;
            }
        }

        int wid = 135;

        private void MenuTreeList_OnLoad(object sender, EventArgs e)
        {
            _left_menu_button.Location = new Point(_menu_button_point_x, _menu_button_point_y);

            _menu_tree.Location = new Point(_menu_left_point_x, _menu_left_point_y);

            _label.Location = new Point(20, 35);

            _close.Location = new Point(_menu_left_point_x + (_bg_image.Width - _menu_left_arrow.Width - 12), 35);

            _tree_view.Location = new Point(20, 135);

            _left_tab.Location = new Point(15, 70);

            _right_tab.Location = new Point(wid + 16, 70);

        }

        private void init()
        {

            _left_menu_button = new DSkinPanel();

            _left_menu_button.Size = new Size(_menu_left_button.Width, _menu_left_button.Height);

            _left_menu_button.BackgroundImage = _menu_left_button;

            _left_menu_button.BackColor = Color.Transparent;

            _left_menu_button.BackgroundImageLayout = ImageLayout.Center;

            _left_menu_button.Visible = true;

            _left_menu_button.MouseDown += Menu_button_OnClick;

            this.Controls.Add(_left_menu_button);


            _menu_tree = new DSkinPanel();

            _menu_tree.Size = new Size(_bg_image.Width, _bg_image.Height);

            _menu_tree.BackgroundImage = _bg_image;

            _menu_tree.Visible = false;

            _menu_tree.BackColor = Color.Transparent;

            _menu_tree.BackgroundImageLayout = ImageLayout.Center;


            _label = new DSkinLabel();

            _label.Location = new Point(30, 35);

            _label.Size = new Size(200, 30);

            _label.ForeColor = Color.Snow;

            _label.Text = "设备列表";

            _label.Font = new Font("微软雅黑", 11, FontStyle.Bold);

            _menu_tree.Controls.Add(_label);

            _close = new DSkinLabel();

            _close.Size = _menu_left_arrow.Size;

            _close.BackgroundImage = _menu_left_arrow;

            _close.BackColor = Color.Transparent;

            _close.BackgroundImageLayout = ImageLayout.Center;

            _close.MouseDown += _close_MouseDown;

            _menu_tree.Controls.Add(_close);



            _left_tab = new DSkinPanel();

            _left_tab.Size = new Size(wid, 45);

            _left_tab.BackColor = Color.Transparent;

            _left_tab.BackgroundImage = Resources.menu_tree_tab_left;

            _left_tab.BackgroundImageLayout = ImageLayout.Center;


            DSkinLabel _left_label = new DSkinLabel();

            _left_label.BackColor = Color.Transparent;

            _left_label.ForeColor = Color.Snow;

            _left_label.Text = "组 织";

            _left_label.Location = new Point(10, 17);

            _left_tab.Controls.Add(_left_label);

            _menu_tree.Controls.Add(_left_tab);

            _right_tab = new DSkinPanel();

            _right_tab.Size = new Size(wid, 45);

            _right_tab.BackColor = Color.Transparent;

            _right_tab.BackgroundImage = Resources.menu_tree_tab_right;

            _right_tab.BackgroundImageLayout = ImageLayout.Center;


            DSkinLabel _right_label = new DSkinLabel();

            _right_label.BackColor = Color.Transparent;

            _right_label.ForeColor = Color.Snow;

            _right_label.Text = "历 史";

            _right_label.Location = new Point(10, 17);

            _right_tab.Controls.Add(_right_label);

            _menu_tree.Controls.Add(_right_tab);


            _tree_view = new DSkinTreeView();

            _tree_view.Size = new Size(200, _bg_image.Height - 10 - 130);

            setTreeNode();

            _tree_view.ShowCheckBox = false;

            _tree_view.ForeColor = Color.Snow;

            _tree_view.ItemSelectedBorderColor = Color.Transparent;

            _tree_view.ItemBorderColorHover = Color.Transparent;

            _tree_view.ItemBackColorHover = Color.FromArgb(68, 93, 131);

            _tree_view.ItemSelectedBackColor = Color.FromArgb(68, 93, 131);

            _tree_view.InnerScrollBar.Visible = false;

            _tree_view.IconSize = new Size(12, 12);

            _menu_tree.Controls.Add(_tree_view);

            //_menu_tree.MouseDown += _menu_tree_MouseDown;

            this.Controls.Add(_menu_tree);

        }



        private void setTreeNode()
        {
            for (int i = 0; i < 5; i++)
            {
                DSkinTreeViewNode dNode = new DSkinTreeViewNode();

                if (i == 0)
                    dNode.Text = "  南宁交警支队 (2/3)";
                else if (i == 1)
                    dNode.Text = "  展厦景湾 (2/3)";
                else if (i == 2)
                    dNode.Text = "  警用设备";
                else
                    dNode.Text = "  东方明珠全景监控";
                dNode.IsCustom = true;
                dNode.ShowCheckBox = false;
                dNode.ForeColor = Color.White;
                dNode.Icon = Resources.menutree_structure;
                dNode.ItemBorderColorHover = Color.Transparent;
                dNode.ItemBackColorHover = Color.FromArgb(68, 93, 131);
                dNode.IconSize = new Size(12, 12);
                _tree_view.Nodes.Add(dNode);

                if (i >= 2)
                    continue;

                for (int j = 0; j < 2; j++)
                {
                    DSkinTreeViewNode nodenode = new DSkinTreeViewNode();
                    nodenode.Text = "  奇辉全景相机" + j.ToString();

                    if (j % 2 == 0)
                        nodenode.Icon = Resources.menutree_videocamera;
                    else
                        nodenode.Icon = Resources.menutree_videocamera_online;
                    //nodenode.IconSize = new Size(10, 10);
                    dNode.Nodes.Add(nodenode);

                }

            }
            _tree_view.LayoutContent();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            //_menu_button_point_x = 0;

            //_menu_button_point_y = 0;

            //if (_isShowMenuButton)
            //    e.Graphics.DrawImage(_menu_left_button, new Rectangle(new Point(_menu_button_point_x, _menu_button_point_y), new Size(_menu_left_button.Width, _menu_left_button.Height)));

            //if (_isShowMenu)
            //{
            //    e.Graphics.DrawImage(_bg_image, new Rectangle(new Point(0, 0), new Size(this.Width, this.Height)));

            //    _menu_left_point_x = this.Width - _menu_left_arrow.Width - 10;
            //    _menu_left_point_y = 40;

            //    e.Graphics.DrawImage(_menu_left_arrow, new Rectangle(new Point(_menu_left_point_x, _menu_left_point_y), new Size(_menu_left_arrow.Width, _menu_left_arrow.Height)));

            //    StringFormat format = new StringFormat();
            //    format.Alignment = StringAlignment.Far;
            //    //表示字符串的垂直对齐方式
            //    format.LineAlignment = StringAlignment.Center;

            //    Brush bush = new SolidBrush(Color.SkyBlue);//填充的颜色

            //    Font font = new Font("微软雅黑", 11, FontStyle.Bold);

            //    SizeF z = e.Graphics.MeasureString(Menu_Title, font);

            //    e.Graphics.DrawString(Menu_Title, font, bush, new PointF(20 + z.Width, _menu_left_point_y + 10), format);
            // }


            //_menu_left_point_x = this.Width - _menu_left_arrow.Width - 10;
            //_menu_left_point_y = 10;


            //StringFormat format = new StringFormat();
            //format.Alignment = StringAlignment.Far;
            ////表示字符串的垂直对齐方式
            //format.LineAlignment = StringAlignment.Center;

            //Brush bush = new SolidBrush(Color.SkyBlue);//填充的颜色

            //Font font = new Font("微软雅黑", 11, FontStyle.Bold);

            //SizeF z = e.Graphics.MeasureString(Menu_Title, font);

            //e.Graphics.DrawString(Menu_Title, font, bush, new PointF(20 + z.Width, _menu_left_point_y + 10), format);
        }


        private void Menu_button_OnClick(object sender, MouseEventArgs e)
        {
            if (_isShowMenuButton)
                showPictureBoxList();
        }

        private void _close_MouseDown(object sender, MouseEventArgs e)
        {
            if (_isShowMenu)
                closePictureBoxList();
        }


        private void Form_MouseDown(object sender, MouseEventArgs e)
        {

            //实现点击开关菜单
            if (e.X > _menu_button_point_x && e.X < _menu_button_point_x + _menu_left_button.Width && e.Y > _menu_button_point_y && e.Y < _menu_button_point_y + _menu_left_button.Height)
            {

                if (_isShowMenuButton)
                    showPictureBoxList();

                return;
            }

            if (e.X >= _menu_left_point_x && e.X <= _menu_left_point_x + _menu_left_arrow.Width && e.Y >= _menu_left_point_y && e.Y <= _menu_left_point_y + _menu_left_arrow.Height)
            {

                if (_isShowMenu)
                    closePictureBoxList();

                return;
            }
        }

        private void showPictureBoxList()
        {

            _isShowMenuButton = false;

            _isShowMenu = true;

            _left_menu_button.Visible = false;

            _menu_tree.Visible = true;

        }

        private void closePictureBoxList()
        {

            _isShowMenuButton = true;

            _isShowMenu = false;

            _menu_tree.Visible = false;

            _left_menu_button.Visible = true;

        }
    }
}
