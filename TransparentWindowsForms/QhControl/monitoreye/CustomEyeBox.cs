
using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using TransparentWindowsForms.QhControl.entity;
using TransparentWindowsForms.Properties;


/************************************************************************
    * Copyright (c) 2018 All Rights Reserved.
    * 创建人： QuJun
    * 创建时间：2018/12/27
    * 描述： 自定义可视控件  显示监控信息
************************************************************************/

namespace TransparentWindowsForms.QhControl.monitoreye
{
    public partial class CustomEyeBox : UserControl
    {

        protected Image _eyebox_circle = Resources.eyebox_monitor_circle;

        protected Color _eyebox_line_color = Color.DeepSkyBlue;

        protected Image _eyebox_text_bg = Resources.eyebox_monitor_text_bg;

        protected Image _eyebox_text_icon = Resources.eyebox_monitor_text_type_1;

        protected string _content = "";

        protected int _text_bg_height = 30;

        protected int _text_bg_width = 0;

        protected int _text_bg_x = 0;

        protected int _text_bg_y = 0;

        protected int _slash_len = 20;// 斜线长度

        protected int _horizontal_line = 80;// 横线长度

        protected Font _text_font = new Font("微软雅黑", 10, FontStyle.Bold);

        protected int _type = 1; // 0:向右 1:想左  (默认右侧,若边缘则向左侧)

        public event Action<EyeBoxClickInfo> OnEyeBoxClick;
        public CustomEyeBox()
        {
            InitializeComponent();

            ResizeControl();
        }


        public void ResizeControl()
        {

            this.Height = _eyebox_circle.Height + _slash_len + _text_bg_height + 10;
        }

        public Color EyeBoxLineColor
        {
            get { return _eyebox_line_color; }
            set
            {
                _eyebox_line_color = value;
            }
        }

        public Image EyeBoxTextIcon
        {
            get { return _eyebox_text_icon; }
            set
            {
                _eyebox_text_icon = value;
            }
        }

        public int EyeBoxType
        {
            get { return _type; }
            set
            {
                _type = value;
            }
        }

        public string EyeBoxText
        {
            get { return _content; }
            set
            {
                _content = value;
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {

            e.Graphics.InterpolationMode = InterpolationMode.NearestNeighbor;
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;  //使绘图质量最高，即消除锯齿
            e.Graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
            e.Graphics.CompositingQuality = CompositingQuality.HighQuality;

            // 根据内容设置长度
            StringFormat format = new StringFormat();
            format.Alignment = StringAlignment.Far;
            //表示字符串的垂直对齐方式
            format.LineAlignment = StringAlignment.Center;
            SolidBrush sbrush = new SolidBrush(Color.Snow);

            SizeF z = e.Graphics.MeasureString(_content, _text_font);

            int text_width = 0;
            if (_content == "")
                text_width = _eyebox_text_bg.Width;
            else
                text_width = (int)z.Width + _eyebox_text_icon.Width + 10 + 10 + 10;

            this.Width = _eyebox_circle.Width + _slash_len + _horizontal_line + text_width + 10;


            Brush circle_bush = new SolidBrush(Color.SkyBlue);//填充的颜色

            Brush circle_outer_bush = new SolidBrush(Color.DarkBlue);//填充的颜色

            if (_type == 0)
            {

                int circle_point_x = 10;

                int circle_point_y = this.Height - _eyebox_circle.Height;

                // 绘制圆圈
                e.Graphics.DrawImage(_eyebox_circle, new Rectangle(new Point(circle_point_x, circle_point_y), new Size(_eyebox_circle.Width, _eyebox_circle.Height)));

                int line_point_x = circle_point_x + _eyebox_circle.Width - 1;

                int line_point_y = circle_point_y + 1;

                int line_point_end_x = line_point_x + 20;

                int line_point_end_y = line_point_y - 20;
                // 绘制线
                e.Graphics.DrawLine(new Pen(_eyebox_line_color, 2), new Point(line_point_x, line_point_y), new Point(line_point_end_x, line_point_end_y));


                int line_point_v_x = line_point_end_x + 80;

                int line_point_v_y = line_point_end_y;

                e.Graphics.DrawLine(new Pen(_eyebox_line_color, 2), new Point(line_point_end_x, line_point_end_y), new Point(line_point_v_x, line_point_v_y));


                int text_bg_point_x = line_point_v_x;

                int text_bg_point_y = line_point_v_y - _text_bg_height / 2;

                _text_bg_x = text_bg_point_x;

                _text_bg_y = text_bg_point_y;

                _text_bg_width = text_width;

                // 文字背景
                e.Graphics.DrawImage(_eyebox_text_bg, new Rectangle(new Point(text_bg_point_x, text_bg_point_y), new Size(text_width, _text_bg_height)));

                int icon_point_x = text_bg_point_x + 10;

                int icon_point_y = text_bg_point_y + (_text_bg_height - _eyebox_text_icon.Height) / 2;
                // icon 图标
                e.Graphics.DrawImage(_eyebox_text_icon, new Rectangle(new Point(icon_point_x, icon_point_y), new Size(_eyebox_text_icon.Width, _eyebox_text_icon.Height)));

                float content_point_x = icon_point_x + _eyebox_text_icon.Width + z.Width;

                float content_point_y = icon_point_y + _eyebox_text_icon.Height / 2 + 3;

                e.Graphics.DrawString(_content, _text_font, sbrush, new PointF(content_point_x, content_point_y), format);

                return;

            }

            if (_type == 1)
            {

                int text_bg_point_x = 10;

                int text_bg_point_y = 10;

                _text_bg_x = text_bg_point_x;

                _text_bg_y = text_bg_point_y;

                _text_bg_width = text_width;

                // 文字背景
                e.Graphics.DrawImage(_eyebox_text_bg, new Rectangle(new Point(text_bg_point_x, text_bg_point_y), new Size(text_width, _text_bg_height)));

                int icon_point_x = text_bg_point_x + 10;

                int icon_point_y = text_bg_point_y + (_text_bg_height - _eyebox_text_icon.Height) / 2;
                // icon 图标
                e.Graphics.DrawImage(_eyebox_text_icon, new Rectangle(new Point(icon_point_x, icon_point_y), new Size(_eyebox_text_icon.Width, _eyebox_text_icon.Height)));

                float content_point_x = icon_point_x + _eyebox_text_icon.Width + z.Width;

                float content_point_y = icon_point_y + _eyebox_text_icon.Height / 2 + 3;

                e.Graphics.DrawString(_content, _text_font, sbrush, new PointF(content_point_x, content_point_y), format);

                int line_point_x = text_bg_point_x + text_width;

                int line_point_y = text_bg_point_y + _text_bg_height / 2;

                int line_point_end_x = line_point_x + 80;

                int line_point_end_y = line_point_y;

                e.Graphics.DrawLine(new Pen(_eyebox_line_color, 2), new Point(line_point_x, line_point_y), new Point(line_point_end_x, line_point_end_y));

                int line_point_v_x = line_point_end_x + 20;

                int line_point_v_y = line_point_end_y + 20;
                // 绘制线
                e.Graphics.DrawLine(new Pen(_eyebox_line_color, 2), new Point(line_point_end_x - 1, line_point_end_y), new Point(line_point_v_x - 1, line_point_v_y));

                int circle_point_x = line_point_v_x - 2;

                int circle_point_y = line_point_v_y - 2;
                // 绘制圆圈
                e.Graphics.DrawImage(_eyebox_circle, new Rectangle(new Point(circle_point_x, circle_point_y), new Size(_eyebox_circle.Width, _eyebox_circle.Height)));

                return;
            }
        }


        protected override void OnMouseClick(MouseEventArgs e)
        {
            base.OnMouseClick(e);

            if (e.X > _text_bg_x && e.X < _text_bg_x + _text_bg_width
                   && e.Y > _text_bg_y && e.Y < _text_bg_y + _text_bg_height)
            {
                if (OnEyeBoxClick != null)
                {
                    if (_type == 0)
                        OnEyeBoxClick(new EyeBoxClickInfo(this.Bounds.X + this.Width, this.Bounds.Y + _text_bg_y, _content, _type));
                    else
                        OnEyeBoxClick(new EyeBoxClickInfo(this.Bounds.X, this.Bounds.Y + _text_bg_y, _content, _type));
                }
            }
        }
    }
}
