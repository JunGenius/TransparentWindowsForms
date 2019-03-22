
using DSkin.Controls;
using DSkin.Forms;
using System;
using System.Drawing;
using System.Windows.Forms;
using TransparentWindowsForms.Properties;
using TransparentWindowsForms.QhControl.scrollcombox;


/*=================================================================
* Author: QUJUN
* CreatedTime: 2019/1/9 14:26:23
* Description:  透明窗体 (图层控制窗体)
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
    public partial class MonitorControlForm : DSkinForm
    {

        private HCPreview _hc = null;

        private CustomDCSelectListBox _select_listbox = null;

        private DSkinPanel _monitor_button = null; // 标题

        private DSkinLabel _monitor_label = null;

        private DSkinTextBox _query_text = null;

        private DSkinPanel _query_text_pannel = null;

        private Image _monitor_bg = Resources.monitor_bg;

        private Image _combox_list_bg = Resources.combox_list_bg;

        private Image _query_text_box_bg = Resources.query_text_box_bg;

        public event Action<SelectListBoxInfo> IsSelectInfo;

        public MonitorControlForm(HCPreview _hc)
        {
            InitializeComponent();

            this._hc = _hc;

            this.LocationChanged += IconFrom_LocationChanged;

            this.ShowInTaskbar = false;

            initControl();

        }

        private void IconFrom_LocationChanged(object sender, EventArgs e)
        {

            _monitor_button.Location = new Point(50, 50);

            _monitor_label.Location = new Point(10, 0);

            _monitor_label.Size = _monitor_button.Size;

            _select_listbox.Location = new Point(240, 50);

            _query_text_pannel.Location = new Point(this.Width - _query_text_box_bg.Width - 30, 50);

            _query_text.Location = new Point(10, 10);

            _query_text.Size = new Size(245 , 20);

        }

        private void initControl()
        {

            _monitor_button = new DSkinPanel();

            _monitor_button.Size = new Size(_monitor_bg.Width, _monitor_bg.Height);

            _monitor_button.BackColor = Color.Transparent;

            _monitor_button.BackgroundImage = _monitor_bg;

            _monitor_button.BackgroundImageLayout = ImageLayout.Center;

            _monitor_label = new DSkinLabel();

            _monitor_label.BackColor = Color.Transparent;

            _monitor_label.ForeColor = Color.Snow;

            _monitor_label.TextAlign = ContentAlignment.MiddleLeft;

            _monitor_label.Text = "大连奇辉全景相机";

            _monitor_button.Controls.Add(_monitor_label);

            this.Controls.Add(_monitor_button);

            _select_listbox = new CustomDCSelectListBox();

            _select_listbox.Size = new Size(200, 360);

            _select_listbox.IsSelectInfo += Select_listbox_IsSelectInfo;

            this.Controls.Add(_select_listbox);


            _query_text_pannel = new DSkinPanel();

            _query_text_pannel.BackColor = Color.Transparent;

            _query_text_pannel.BackgroundImage = _query_text_box_bg;

            _query_text_pannel.BackgroundImageLayout = ImageLayout.Center;

            _query_text_pannel.Size = _query_text_box_bg.Size;


            _query_text = new DSkinTextBox();

            _query_text.BackColor = Color.Yellow;

            _query_text.TransparencyKey = Color.Yellow;

            _query_text.BorderStyle = BorderStyle.None;

            _query_text.BackgroundImageLayout = ImageLayout.Center;

            _query_text.BackgroundImage = _query_text_box_bg;

            _query_text.ForeColor = Color.Snow;

            _query_text.Modified = true;

            _query_text.WaterText = "请输入内容";

            _query_text.WaterTextOffset = new Point(5, 0);

            _query_text_pannel.Controls.Add(_query_text);

            this.Controls.Add(_query_text_pannel);
        }

        private void Select_listbox_IsSelectInfo(SelectListBoxInfo obj)
        {
            if (IsSelectInfo != null)
                IsSelectInfo(obj);
        }

    }
}
