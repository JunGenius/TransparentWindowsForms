namespace TransparentWindowsForms.QhControl.menutree
{
    partial class MenuTreeList
    {
        /// <summary> 
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region 组件设计器生成的代码

        /// <summary> 
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.dSkinTreeView1 = new DSkin.Controls.DSkinTreeView();
            ((System.ComponentModel.ISupportInitialize)(this.dSkinTreeView1)).BeginInit();
            this.SuspendLayout();
            // 
            // dSkinTreeView1
            // 
            this.dSkinTreeView1.IconSize = new System.Drawing.Size(10, 10);
            this.dSkinTreeView1.Location = new System.Drawing.Point(44, 43);
            this.dSkinTreeView1.Name = "dSkinTreeView1";
            this.dSkinTreeView1.Size = new System.Drawing.Size(100, 100);
            this.dSkinTreeView1.TabIndex = 0;
            this.dSkinTreeView1.Text = "dSkinTreeView1";
            // 
            // MenuTreeList
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.dSkinTreeView1);
            this.Name = "MenuTreeList";
            ((System.ComponentModel.ISupportInitialize)(this.dSkinTreeView1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DSkin.Controls.DSkinTreeView dSkinTreeView1;
    }
}
