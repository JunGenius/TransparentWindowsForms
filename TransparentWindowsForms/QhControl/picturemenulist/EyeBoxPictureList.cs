using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using TransparentWindowsForms.Properties;
using DSkin.Controls;


/*=================================================================
* Author: QUJUN
* CreatedTime: 2019/1/9 14:26:23
* Description:  右侧图片列表菜单
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


namespace TransparentWindowsForms.QhControl
{
    public partial class EyeBoxPictureList : DSkinUserControl
    {

        private Image _menu_button = Resources.menu_right_button; // 菜单按钮开关

        private Image _list_bg = Resources.picture_menu_list_bg;  // 菜单背景

        private Image _close_button = Resources.right_menu_close; // 关闭图标

        private Image _setting_button = Resources.picture_menu_list_setting; // 设置图标

        private int dif = 25; // 列表左右侧大小

        private int top_height = 25;  //图片距顶部高度

        private int start_point_x = 0;// 图片框坐标

        private int start_point_y = 0;  // 图片框坐标

        private int start_bound_w = 0;  //图片框宽度

        private int box_dif = 10; // 列表间大小

        private int start_bound_h = 0;//图片框高度

        private int _menu_button_point_x = 0;//菜单按钮坐标

        private int _menu_button_point_y = 0;// 菜单按钮坐标

        private bool _isShowMenu = false;

        private bool _isShowMenuButton = true;

        private List<EyeBoxDetailView> _eyeBoxViews = new List<EyeBoxDetailView>();

        private DSkinPanel _menu_picture = null;

        private DSkinPanel _right_menu_button = null;

        private IconFrom _iconForm = null;

        public EyeBoxPictureList(IconFrom _iconForm)
        {
            InitializeComponent();

            this._iconForm = _iconForm;

            this.Load += EyeBoxListPicture_OnLoad;

            this.MouseDown += Form_MouseDown;

            this.LocationChanged += EyeBoxPictureList_LocationChanged;

            init();
        }

        private void EyeBoxPictureList_LocationChanged(object sender, EventArgs e)
        {
            _menu_button_point_x = this.Width - _menu_button.Width;

            _right_menu_button.Location = new Point(_menu_button_point_x, _menu_button_point_y);
        }

        private void init()
        {

            _menu_button_point_y = 0;

            _right_menu_button = new DSkinPanel();

            _right_menu_button.Size = new Size(_menu_button.Width, _menu_button.Height);

            _right_menu_button.BackgroundImage = _menu_button;

            _right_menu_button.BackColor = Color.Transparent;

            _right_menu_button.BackgroundImageLayout = ImageLayout.Center;

            _right_menu_button.Visible = true;

            _right_menu_button.MouseDown += Menu_button_OnClick;

            this.Controls.Add(_right_menu_button);


            _menu_picture = new DSkinPanel();

            _menu_picture.Location = new Point(0, 0);

            _menu_picture.Size = new Size(_list_bg.Width, _list_bg.Height);

            _menu_picture.BackgroundImage = _list_bg;

            _menu_picture.Visible = false;

            _menu_picture.BackColor = Color.Transparent;

            _menu_picture.BackgroundImageLayout = ImageLayout.Stretch;

            DSkinLabel _close = new DSkinLabel();

            _close.Location = new Point(15, 12);

            _close.Size = new Size(15, 15);

            _close.BackgroundImage = _close_button;

            _close.BackColor = Color.Transparent;

            _close.BackgroundImageLayout = ImageLayout.Center;

            _close.MouseDown += _close_MouseDown;

            _menu_picture.Controls.Add(_close);


            DSkinLabel _setting = new DSkinLabel();

            _setting.Location = new Point(150, 12);

            _setting.Size = new Size(15, 15);

            _setting.BackgroundImage = _setting_button;

            _setting.BackColor = Color.Transparent;

            _setting.BackgroundImageLayout = ImageLayout.Center;

            _setting.MouseDown += _setting_MouseDown;

            _menu_picture.Controls.Add(_setting);


            start_point_x = dif;
            start_point_y = top_height;
            start_bound_w = _list_bg.Width  - dif * 2;
            start_bound_h = (_list_bg.Height - top_height) / 4 - box_dif * 2;

            for (int i = 0; i < 4; i++)
            {
                EyeBoxDetailView detail_view = new EyeBoxDetailView(Resources.picture_box_car_1);

                detail_view.Location = new Point(start_point_x, start_point_y + start_bound_h * (i) + box_dif + box_dif * (i + 1));

                detail_view.Size = new Size(start_bound_w, start_bound_h);

                detail_view.EyeBoxDetailInfo = new EyeBoxDetailInfo("辽B12345", "99:99:00", "火车东站第一扎道监控");

                _menu_picture.Controls.Add(detail_view);

                _eyeBoxViews.Add(detail_view);
            }

            this.Controls.Add(_menu_picture);
        }

        private void _setting_MouseDown(object sender, MouseEventArgs e)
        {
            
        }

        protected override void OnPaint(PaintEventArgs e)
        {
        }

        private void EyeBoxListPicture_OnLoad(object sender, EventArgs e)
        {
        }
        private void Form_MouseDown(object sender, MouseEventArgs e)
        {

            // 菜单按钮点击
            if (e.X > _menu_button_point_x && e.X < _menu_button_point_x + _menu_button.Width && e.Y > _menu_button_point_y && e.Y < _menu_button_point_y + _menu_button.Height)
            {

                if (_isShowMenuButton)
                    showPictureBoxList();

                return;
            }

            if (e.X > 0 && e.X < 20 && e.Y > 0 && e.Y < 20)
            {

                if (_isShowMenu)
                    closePictureBoxList();

                return;
            }
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


        private void showPictureBoxList()
        {

            _isShowMenuButton = false;

            _isShowMenu = true;

            _menu_picture.Visible = true;

            _right_menu_button.Visible = false;
        }

        private void closePictureBoxList()
        {

            _isShowMenuButton = true;

            _isShowMenu = false;

            _menu_picture.Visible = false;

            _right_menu_button.Visible = true;
        }

       
    }
}
