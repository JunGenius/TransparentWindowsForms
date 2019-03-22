

using DSkin.Forms;
using System;
using System.Drawing;
using TransparentWindowsForms.Properties;
using TransparentWindowsForms.QhControl;
using TransparentWindowsForms.QhControl.menutree;


/*=================================================================
* Author: QUJUN
* CreatedTime: 2019/1/9 14:26:23
* Description: 透明窗体(菜单控制窗体)
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
    public partial class IconFrom : DSkinForm
    {
        private Image _menu_left_button = Resources.menu_right_button;

        private HCPreview _hc = null;

        private EyeBoxPictureList detail_view = null;  // 右侧菜单

        private MenuTreeList treeList = null; // 左侧菜单


        public IconFrom(HCPreview _hc)
        {
            InitializeComponent();

            this._hc = _hc;

            this.LocationChanged += IconFrom_LocationChanged;

            this.ShowInTaskbar = false;

            initControl();

        }

        private void IconFrom_LocationChanged(object sender, EventArgs e)
        {
            detail_view.Location = new Point(_hc.Width - 180 - 10, 135);

            treeList.Location = new Point(10, 135);
        }

        private void initControl()
        {

            detail_view = new EyeBoxPictureList(this);

            detail_view.Size = new Size(180, 550);

            this.Controls.Add(detail_view);


            treeList = new MenuTreeList();

            treeList.Size = new Size(350, 600);

            this.Controls.Add(treeList);

        }
    }
}
