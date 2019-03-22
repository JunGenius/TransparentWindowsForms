using System;

using System.Drawing;
using System.Windows.Forms;
using TransparentWindowsForms.Properties;

namespace TransparentWindowsForms.QhControl
{
    public partial class EyeBoxDetailView : UserControl
    {


        private Image _rec_bg = Resources.picture_menu_list_detail_bg;

        private PictureBox _pic_box;

        private int _diff = 5;

        private int _bottom_height = 32;

        protected Font _text_font = new Font("微软雅黑", 6, FontStyle.Bold);

        SolidBrush sbrush = new SolidBrush(Color.Snow);

        private EyeBoxDetailInfo _detail_info = null;


        private Image _image = null;

        public EyeBoxDetailView(Image image)
        {
            InitializeComponent();

            this._image = image;

            this.Load += EyeBoxDetailView_Load;

            this.BackColor = Color.FromArgb(17, 30, 57);

            this.MouseEnter += EyeBox_OnMouseEnter;

            this.MouseLeave += EyeBox_OnMouseLeave;
        }

        public EyeBoxDetailInfo EyeBoxDetailInfo
        {
            get { return _detail_info; }
            set
            {
                _detail_info = value;
                Invalidate();
            }
        }

        private void init()
        {
            _pic_box = new PictureBox();

            _pic_box.Location = new Point(_diff, _diff);

            _pic_box.BackColor = Color.DarkGray;

            _pic_box.Size = new Size(this.Width - _diff * 2, this.Height - _diff - _bottom_height);

            _pic_box.Image = _image;

            _pic_box.BackgroundImageLayout = ImageLayout.None;

            // 实现鼠标浮动效果
            _pic_box.MouseEnter += EyeBox_OnMouseEnter;

            _pic_box.MouseLeave += EyeBox_OnMouseLeave;

            this.Controls.Add(_pic_box);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            StringFormat format = new StringFormat();
            format.Alignment = StringAlignment.Far;
            //表示字符串的垂直对齐方式
            format.LineAlignment = StringAlignment.Center;

            SizeF z_title = e.Graphics.MeasureString(_detail_info.Title, _text_font);

            e.Graphics.DrawImage(_rec_bg, new Rectangle(new Point(0, 0), new Size(this.Width, this.Height)));

            e.Graphics.DrawString(_detail_info.Title, _text_font, sbrush, new PointF(5 + z_title.Width, this.Height - _bottom_height + 8), format);

            SizeF z_subtitle = e.Graphics.MeasureString(_detail_info.SubTitle, _text_font);

            e.Graphics.DrawString(_detail_info.SubTitle, _text_font, sbrush, new PointF(this.Width - 5, this.Height - _bottom_height + 8), format);

            SizeF z_info = e.Graphics.MeasureString(_detail_info.Info, _text_font);

            e.Graphics.DrawString(_detail_info.Info, _text_font, sbrush, new PointF(2 + z_info.Width, this.Height - _bottom_height + 10 + 10), format);

        }

        private void EyeBoxDetailView_Load(object sender, EventArgs e)
        {
            init();
        }

        protected void EyeBox_OnMouseEnter(object sender, EventArgs e)
        {
            _pic_box.Size = new Size(this.Width - 4, this.Height - _bottom_height - 4);

            _pic_box.Location = new Point(2, 2);

            Invalidate();

        }

        protected void EyeBox_OnMouseLeave(object sender, EventArgs e)
        {


            _pic_box.Size = new Size(this.Width - _diff * 2, this.Height - _diff - _bottom_height);

            _pic_box.Location = new Point(_diff, _diff);

            Invalidate();

        }

    }
}
