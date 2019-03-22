using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DSkin.Controls;
using TransparentWindowsForms.Properties;
using DSkin.DirectUI;
using System.Drawing.Text;

namespace TransparentWindowsForms.QhControl.scrollcombox
{
    public partial class CustomDCSelectListBox : DSkinUserControl
    {

        private DSkinPanel _layer_panel = null;

        private DSkinLabel _layer_label = null;

        private Image _layer_panel_bg = Resources.combox_list_bg;

        private Image _list_box_bg = Resources.combox_scroll_list_bg;

        private DSkinListBox _list_box = null;

        public int ItemsIndex = -1;

        private bool _isShowMenu = false;

        public event Action<SelectListBoxInfo> IsSelectInfo;

        public CustomDCSelectListBox()
        {
            InitializeComponent();

            this.Load += CustomDCSelectListBox_Load;

            init();
        }

        private void CustomDCSelectListBox_Load(object sender, EventArgs e)
        {
            _layer_panel.Location = new Point(0, 0);

            _layer_label.Location = new Point(10, 0);

            _layer_label.Size = _layer_panel.Size;

            _list_box.Location = new Point(0, _layer_panel_bg.Height);

           
        }

        private void init()
        {
            _layer_panel = new DSkinPanel();
            _layer_panel.Size = new Size(_layer_panel_bg.Width, _layer_panel_bg.Height);
            _layer_panel.BackgroundImage = _layer_panel_bg;
            _layer_panel.BackgroundImageLayout = ImageLayout.Center;
            _layer_panel.BackColor = Color.Transparent;
            _layer_panel.Click += _layer_panel_Click;

            _layer_label = new DSkinLabel();

            _layer_label.BackColor = Color.Transparent;

            _layer_label.ForeColor = Color.Snow;

            _layer_label.TextAlign = ContentAlignment.MiddleLeft;

            _layer_label.Text = "全景监控区域";

            _layer_label.Click += _layer_panel_Click;

            _layer_panel.Controls.Add(_layer_label);

            this.Controls.Add(_layer_panel);

            _list_box = new DSkinListBox();
            _list_box.Size = new Size(_list_box_bg.Width, 260);
            _list_box.BackgroundImage = _list_box_bg;
            _list_box.BackgroundImageLayout = ImageLayout.Center;
            _list_box.BackColor = Color.Transparent;
            _list_box.Visible = _isShowMenu;
            _list_box.ShowScrollBar = true;
            _list_box.ScrollBarWidth = 12;
            _list_box.InnerScrollBar.BackColor = Color.FromArgb(35, 61, 84);
            _list_box.InnerScrollBar.ArrowNormalColor = Color.FromArgb(50, 0, 0, 0);
            _list_box.InnerScrollBar.ArrowHoverColor = Color.FromArgb(100, 0, 0, 0);
            _list_box.InnerScrollBar.ArrowPressColor = Color.FromArgb(80, 0, 0, 0);
            _list_box.InnerScrollBar.ScrollBarNormalImage = Resources.scroll_thumb;
            _list_box.InnerScrollBar.Fillet = true;       

            AddFriendItems(false, "信息栏");

            AddFriendItems(false, "矩形框");

            AddFriendItems(true, "视频监控");

            AddFriendItems(false, "卡口监控");

            AddFriendItems(false, "道路施工");

            AddFriendItems(false, "诱导屏");

            AddFriendItems(false, "导向屏");

            AddFriendItems(false, "建筑物");

            AddFriendItems(false, "警用设备");

            AddFriendItems(false, "信号机");

            this.Controls.Add(_list_box);
        }



        public DuiLabel AddMonitorGroup(string groupName, string openOrclose)
        {
            DuiLabel group = AddDuiLabel(groupName, new Font("微软雅黑", 9.5f), new Size(this.Width - 85, 25), new Point(0, 0),
                false);
            group.TextRenderMode = TextRenderingHint.AntiAliasGridFit;
            group.TextAlign = ContentAlignment.TopLeft;
            group.Font = new Font("微软雅黑", 9.5f);
            group.TextPadding = 3;
            //dui.TextRenderMode=System.Drawing.Text.TextRenderingHint.AntiAlias;
            group.BackgroundImageLayout = ImageLayout.None;
            group.BackColor = Color.WhiteSmoke;
            group.BackgroundImage = _layer_panel_bg;
            group.Size = _layer_panel_bg.Size;
            group.Name = _list_box.Items.Count.ToString();
            TagGroupInfo tag = new TagGroupInfo();
            tag.OpenCloseStyle = openOrclose;
            //tag.ItemsCount = itemsCount;
            group.Tag = tag;
            group.MouseUp += FriendGroupMouseUp;
            //dui.MouseClick += dui_MouseClick;
            group.MouseEnter += GroupMouseEnter;
            group.MouseLeave += GroupMouseLeave;

            _list_box.Items.Add(group);

            return group;
        }

        public DuiLabel AddDuiLabel(string text, Font font, Size size, Point location, bool isEven)
        {
            DuiLabel duiLabel = new DuiLabel();
            duiLabel.Size = size;
            duiLabel.Text = text;
            duiLabel.Font = font;
            duiLabel.TextRenderMode = TextRenderingHint.AntiAliasGridFit;
            duiLabel.TextAlign = ContentAlignment.MiddleLeft;
            duiLabel.Location = location;
            if (isEven)
            {
                duiLabel.MouseEnter += HightMouseEnter;
                duiLabel.MouseLeave += HightMouseLeave;
            }
            return duiLabel;
        }


        #region 好友列表
        public void AddFriendItems(bool is_check, string info)
        {
            //添加一个DSkinBaseControl控件用来存储好友信息DUI控件
            DuiBaseControl duiBaseControl = new DuiBaseControl();
            duiBaseControl.Name = _list_box.Items.Count.ToString();
            duiBaseControl.BackColor = Color.Transparent;
            duiBaseControl.Size = new Size(_list_box.Width - 10, 30);
            //给控件添加鼠标enter和leaver事件
            //duiBaseControl.MouseUp += ItemsMouseUp;
            //duiBaseControl.MouseEnter += DSkinBaseControlMouseEnter;
            //duiBaseControl.MouseLeave += DSkinBaseControlMouseLeaver;

            //向DSkinBaseControl控件添加一个DuiCheckBox控件
            DuiCheckBox duiPictureBox = new DuiCheckBox();
            duiPictureBox.Location = new Point(10, 5);
            duiPictureBox.Size = new Size(20, 20);
            duiPictureBox.Tag = info;
            duiPictureBox.CheckedChanged += DuiPictureBox_CheckedChanged;

            duiPictureBox.Checked = is_check;
            //duiPictureBox.BackgroundImageLayout = ImageLayout.Stretch;
            //duiPictureBox.BackgroundImage = is_check ? Resources.combox_scroll_check : Resources.combox_scroll_un_check;
            duiBaseControl.Controls.Add(duiPictureBox);

            //向DSkinBaseControl控件添加一个DuiLable控件用来存储个性签名
            DuiLabel duiLabelqm = new DuiLabel();
            duiLabelqm.ForeColor = Color.Snow;
            duiLabelqm.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            duiLabelqm.TextRenderMode = TextRenderingHint.AntiAliasGridFit;
            duiLabelqm.Text = info;
            duiLabelqm.Location = new System.Drawing.Point(40, 5);
            duiLabelqm.Size = new Size(_list_box.Width - 40, 20);
            duiBaseControl.Controls.Add(duiLabelqm);
            _list_box.Name = _list_box.Items.Count.ToString();
            //将好友项存入列表中
            this._list_box.Items.Add(duiBaseControl);
            this._list_box.LayoutContent();
        }

        private void DuiPictureBox_CheckedChanged(object sender, EventArgs e)
        {
            DuiCheckBox cb = (DuiCheckBox)sender;

            if (cb != null)
            {

                string info = (string)cb.Tag;

                if (IsSelectInfo != null)
                    IsSelectInfo(new SelectListBoxInfo(info, cb.Checked));
            }
        }

        #endregion


        private void _layer_panel_Click(object sender, EventArgs e)
        {
            _isShowMenu = !_isShowMenu;
            _list_box.Visible = _isShowMenu;
        }

        /// <summary>
        /// 好友项点击选中时触发的事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ItemsMouseUp(object sender, MouseEventArgs e)
        {
            if (ItemsIndex > 0)
            {
                _list_box.Items[ItemsIndex].BackColor = ((DuiBaseControl)sender).BackColor = Color.White;
            }
            ItemsIndex = int.Parse(((DuiBaseControl)sender).Name);
            _list_box.Items[ItemsIndex].BackColor = Color.FromArgb(253, 240, 173);
            _list_box.LayoutContent();
        }
        /// <summary>
        /// 好友项鼠标进入控件时显示的背景色
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DSkinBaseControlMouseEnter(object sender, EventArgs e)
        {
            if (ItemsIndex != int.Parse(((DuiBaseControl)sender).Name))
            {
                ((DuiBaseControl)sender).BackColor = Color.FromArgb(255, 251, 205);
            }

        }
        /// <summary>
        /// 好友项鼠标离开时显示的背景色
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DSkinBaseControlMouseLeaver(object sender, EventArgs e)
        {
            if (ItemsIndex != int.Parse(((DuiBaseControl)sender).Name))
            {
                ((DuiBaseControl)sender).BackColor = Color.White;
            }
        }


        #region 分组项鼠标Enter和Leave事件
        /// <summary>
        /// 分组项鼠标离开控件时显示的背景色
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GroupMouseLeave(object sender, EventArgs e)
        {
            ((DuiBaseControl)sender).BackColor = Color.White;
        }

        /// <summary>
        /// 分组项鼠标进入控件时显示的背景色
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GroupMouseEnter(object sender, EventArgs e)
        {
            ((DuiBaseControl)sender).BackColor = Color.FromArgb(255, 251, 205);
        }

        #endregion

        /// <summary>
        /// 鼠标离开控件时不显示边框
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void HightMouseLeave(object sender, EventArgs e)
        {
            ((DuiBaseControl)sender).Borders.AllColor = Color.Transparent;
        }

        /// <summary>
        ///  鼠标进入控件时高亮显示边框
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void HightMouseEnter(object sender, EventArgs e)
        {

            ((DuiBaseControl)sender).Borders.AllColor = Color.FromArgb(40, Color.Black);
        }


        #region 分组项事件

        /// <summary>
        /// 分组项鼠标弹起时触发的时间（鼠标单击事件）
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FriendGroupMouseUp(object sender, MouseEventArgs e)
        {
            TagGroupInfo obj = (TagGroupInfo)((DuiBaseControl)sender).Tag;
            int id = Convert.ToInt32(((DuiBaseControl)sender).Name);

            if (obj.OpenCloseStyle == "open")
            {
                obj.OpenCloseStyle = "close";
                ((DuiBaseControl)sender).Tag = obj;
                for (int i = id + 1; i <= obj.ItemsCount + id; i++)
                {
                    try
                    {
                        _list_box.Items[i].Visible = false;
                    }
                    catch
                    {
                    }
                }
                ((DuiBaseControl)sender).BackgroundImage = _list_box_bg;
                _list_box.LayoutContent();
            }
            else
            {
                obj.OpenCloseStyle = "open";
                ((DuiBaseControl)sender).Tag = obj;

                for (int i = id + 1; i <= obj.ItemsCount + id; i++)
                {
                    try
                    {
                        _list_box.Items[i].Visible = true;
                    }
                    catch
                    {
                    }
                }
                ((DuiBaseControl)sender).BackgroundImage = _list_box_bg;
                _list_box.LayoutContent();
            }

        }

        #endregion
    }
}

public class TagGroupInfo
{
    public string OpenCloseStyle = "";
    public int ItemsCount = 0;
}