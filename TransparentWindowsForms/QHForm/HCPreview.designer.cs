namespace TransparentWindowsForms
{
    partial class HCPreview
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (m_lRealHandle >= 0)
            {
                CHCNetSDK.NET_DVR_StopRealPlay(m_lRealHandle);
            }
            if (m_lUserID >= 0)
            {
                CHCNetSDK.NET_DVR_Logout(m_lUserID);
            }
            if (m_bInitSDK == true)
            {
                CHCNetSDK.NET_DVR_Cleanup();
            }

            if (disposing && (components != null))
            {
                components.Dispose();
            }


            stop();

            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.RealPlayWnd = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.RealPlayWnd)).BeginInit();
            this.SuspendLayout();
            // 
            // RealPlayWnd
            // 
            this.RealPlayWnd.BackColor = System.Drawing.Color.White;
            this.RealPlayWnd.Dock = System.Windows.Forms.DockStyle.Fill;
            this.RealPlayWnd.Location = new System.Drawing.Point(0, 0);
            this.RealPlayWnd.Margin = new System.Windows.Forms.Padding(4);
            this.RealPlayWnd.Name = "RealPlayWnd";
            this.RealPlayWnd.Size = new System.Drawing.Size(1924, 1055);
            this.RealPlayWnd.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.RealPlayWnd.TabIndex = 30;
            this.RealPlayWnd.TabStop = false;
            this.RealPlayWnd.LocationChanged += new System.EventHandler(this.HCPreview_LocationChanged);
            this.RealPlayWnd.Click += new System.EventHandler(this.RealPlayWnd_Click);
            this.RealPlayWnd.Resize += new System.EventHandler(this.HCPreview_Resize);
            // 
            // HCPreview
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1924, 1055);
            this.Controls.Add(this.RealPlayWnd);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "HCPreview";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "图像识别";
            this.LocationChanged += new System.EventHandler(this.HCPreview_LocationChanged);
            this.Resize += new System.EventHandler(this.HCPreview_Resize);
            ((System.ComponentModel.ISupportInitialize)(this.RealPlayWnd)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.PictureBox RealPlayWnd;
    }
}

