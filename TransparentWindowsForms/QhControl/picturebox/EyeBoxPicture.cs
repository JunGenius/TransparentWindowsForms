using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using TransparentWindowsForms.Properties;

namespace TransparentWindowsForms.QhControl
{


    /************************************************************************
        * Copyright (c) 2018 All Rights Reserved.
        * 创建人： QuJun
        * 创建时间：2018/12/27
        * 描述： 带边框的picturebox  中间播放icon
    ************************************************************************/

    public partial class EyeBoxPicture : UserControl
    {

        private Image _rec_image_bg = Resources.picturebox_play_bg;

        private EyePictureBox _pic_box = null;

        private int _rec_bound_width = 2; // 边框宽度
        public EyeBoxPicture()
        {
            InitializeComponent();

            init();
        }

        public PictureBox PicBox
        {
            get { return _pic_box; }
        }

        public void init()
        {
            _pic_box = new EyePictureBox();

            _pic_box.BackColor = this.BackColor;

            this.Controls.Add(_pic_box);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            e.Graphics.DrawImage(_rec_image_bg, new Rectangle(new Point(0, 0), new Size(this.Width, this.Height)));
        }

        private void EyeBoxPicture_Load(object sender, EventArgs e)
        {
            _pic_box.Location = new Point(_rec_bound_width, _rec_bound_width);
            _pic_box.Size = new Size(this.Width - _rec_bound_width * 2, this.Height - _rec_bound_width * 2);
        }
    }
}
