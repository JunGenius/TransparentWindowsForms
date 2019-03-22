
using System;
using System.Drawing;
using System.Windows.Forms;
using TransparentWindowsForms.Properties;



/************************************************************************
    * Copyright (c) 2018 All Rights Reserved.
    * 创建人： QuJun
    * 创建时间：2018/12/27
    * 描述：  中间带图标的 picturebox
************************************************************************/

namespace TransparentWindowsForms.QhControl
{
    class EyePictureBox : PictureBox
    {
        private Image _cannot_play = Resources.picturebox_cannot_play;

        private bool _isShowIcon = true;

        public EyePictureBox()
        {

        }


        public bool IsShowIcon
        {
            get { return _isShowIcon; }
            set
            {
                _isShowIcon = value;              
            }
        }


        protected override void OnPaint(PaintEventArgs e)
        {
            if (_isShowIcon)
                e.Graphics.DrawImage(_cannot_play, new Rectangle(new Point((this.Width - _cannot_play.Width) / 2, (this.Height - _cannot_play.Height) / 2), new Size(_cannot_play.Width, _cannot_play.Height)));
        }
    }
}
