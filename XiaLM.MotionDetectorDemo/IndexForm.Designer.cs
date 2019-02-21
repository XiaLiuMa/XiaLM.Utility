namespace XiaLM.MotionDetectorDemo
{
    partial class IndexForm
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

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.OpenItem = new System.Windows.Forms.ToolStripMenuItem();
            this.OpenGifItem = new System.Windows.Forms.ToolStripMenuItem();
            this.OpenVideoItem = new System.Windows.Forms.ToolStripMenuItem();
            this.DetectorItem = new System.Windows.Forms.ToolStripMenuItem();
            this.DetectorItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.DetectorItem2 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem5 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem9 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem10 = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.OpenItem,
            this.DetectorItem,
            this.toolStripMenuItem5});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(800, 25);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // OpenItem
            // 
            this.OpenItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.OpenGifItem,
            this.OpenVideoItem});
            this.OpenItem.Name = "OpenItem";
            this.OpenItem.Size = new System.Drawing.Size(52, 21);
            this.OpenItem.Text = "Open";
            // 
            // OpenGifItem
            // 
            this.OpenGifItem.Name = "OpenGifItem";
            this.OpenGifItem.Size = new System.Drawing.Size(180, 22);
            this.OpenGifItem.Text = "OpenGif";
            // 
            // OpenVideoItem
            // 
            this.OpenVideoItem.Name = "OpenVideoItem";
            this.OpenVideoItem.Size = new System.Drawing.Size(180, 22);
            this.OpenVideoItem.Text = "OpenVideo";
            // 
            // DetectorItem
            // 
            this.DetectorItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.DetectorItem1,
            this.DetectorItem2});
            this.DetectorItem.Name = "DetectorItem";
            this.DetectorItem.Size = new System.Drawing.Size(70, 21);
            this.DetectorItem.Text = "Detector";
            // 
            // DetectorItem1
            // 
            this.DetectorItem1.Checked = true;
            this.DetectorItem1.CheckState = System.Windows.Forms.CheckState.Checked;
            this.DetectorItem1.Name = "DetectorItem1";
            this.DetectorItem1.Size = new System.Drawing.Size(180, 22);
            this.DetectorItem1.Text = "Detector1";
            // 
            // DetectorItem2
            // 
            this.DetectorItem2.Name = "DetectorItem2";
            this.DetectorItem2.Size = new System.Drawing.Size(180, 22);
            this.DetectorItem2.Text = "Detector2";
            // 
            // toolStripMenuItem5
            // 
            this.toolStripMenuItem5.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem9,
            this.toolStripMenuItem10});
            this.toolStripMenuItem5.Name = "toolStripMenuItem5";
            this.toolStripMenuItem5.Size = new System.Drawing.Size(41, 21);
            this.toolStripMenuItem5.Text = "456";
            // 
            // toolStripMenuItem9
            // 
            this.toolStripMenuItem9.Name = "toolStripMenuItem9";
            this.toolStripMenuItem9.Size = new System.Drawing.Size(97, 22);
            this.toolStripMenuItem9.Text = "123";
            // 
            // toolStripMenuItem10
            // 
            this.toolStripMenuItem10.Name = "toolStripMenuItem10";
            this.toolStripMenuItem10.Size = new System.Drawing.Size(97, 22);
            this.toolStripMenuItem10.Text = "456";
            // 
            // IndexForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "IndexForm";
            this.Text = "IndexForm";
            this.Load += new System.EventHandler(this.IndexForm_Load);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem OpenItem;
        private System.Windows.Forms.ToolStripMenuItem OpenGifItem;
        private System.Windows.Forms.ToolStripMenuItem OpenVideoItem;
        private System.Windows.Forms.ToolStripMenuItem DetectorItem;
        private System.Windows.Forms.ToolStripMenuItem DetectorItem1;
        private System.Windows.Forms.ToolStripMenuItem DetectorItem2;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem5;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem9;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem10;
    }
}

