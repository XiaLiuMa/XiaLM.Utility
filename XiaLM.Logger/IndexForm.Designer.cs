namespace XiaLM.Logger
{
    partial class IndexForm
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
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.panel1 = new System.Windows.Forms.Panel();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.butStartMonitor = new System.Windows.Forms.Button();
            this.comBoxClient = new System.Windows.Forms.ComboBox();
            this.butPause = new System.Windows.Forms.Button();
            this.butOneClear = new System.Windows.Forms.Button();
            this.butAllClear = new System.Windows.Forms.Button();
            this.butLookLogFile = new System.Windows.Forms.Button();
            this.txtBoxLogFile = new System.Windows.Forms.TextBox();
            this.butSelctionLogFile = new System.Windows.Forms.Button();
            this.butLookAtLogFile = new System.Windows.Forms.Button();
            this.panel1.SuspendLayout();
            this.flowLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.tabControl1);
            this.panel1.Location = new System.Drawing.Point(12, 12);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(777, 557);
            this.panel1.TabIndex = 0;
            // 
            // tabControl1
            // 
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(775, 555);
            this.tabControl1.TabIndex = 0;
            this.tabControl1.SelectedIndexChanged += new System.EventHandler(this.tabControl1_SelectedIndexChanged);
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.flowLayoutPanel1.Controls.Add(this.butStartMonitor);
            this.flowLayoutPanel1.Controls.Add(this.comBoxClient);
            this.flowLayoutPanel1.Controls.Add(this.butPause);
            this.flowLayoutPanel1.Controls.Add(this.butOneClear);
            this.flowLayoutPanel1.Controls.Add(this.butAllClear);
            this.flowLayoutPanel1.Controls.Add(this.butLookLogFile);
            this.flowLayoutPanel1.Controls.Add(this.txtBoxLogFile);
            this.flowLayoutPanel1.Controls.Add(this.butSelctionLogFile);
            this.flowLayoutPanel1.Controls.Add(this.butLookAtLogFile);
            this.flowLayoutPanel1.Location = new System.Drawing.Point(795, 12);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(200, 557);
            this.flowLayoutPanel1.TabIndex = 1;
            // 
            // butStartMonitor
            // 
            this.butStartMonitor.Location = new System.Drawing.Point(3, 3);
            this.butStartMonitor.Name = "butStartMonitor";
            this.butStartMonitor.Size = new System.Drawing.Size(187, 23);
            this.butStartMonitor.TabIndex = 1;
            this.butStartMonitor.Text = "启动监控";
            this.butStartMonitor.UseVisualStyleBackColor = true;
            this.butStartMonitor.Click += new System.EventHandler(this.butStartMonitor_Click);
            // 
            // comBoxClient
            // 
            this.comBoxClient.FormattingEnabled = true;
            this.comBoxClient.Location = new System.Drawing.Point(3, 32);
            this.comBoxClient.Name = "comBoxClient";
            this.comBoxClient.Size = new System.Drawing.Size(187, 20);
            this.comBoxClient.TabIndex = 2;
            this.comBoxClient.SelectedIndexChanged += new System.EventHandler(this.comBoxClient_SelectedIndexChanged);
            // 
            // butPause
            // 
            this.butPause.Location = new System.Drawing.Point(3, 58);
            this.butPause.Name = "butPause";
            this.butPause.Size = new System.Drawing.Size(93, 23);
            this.butPause.TabIndex = 4;
            this.butPause.Text = "暂停";
            this.butPause.UseVisualStyleBackColor = true;
            this.butPause.Click += new System.EventHandler(this.butPause_Click);
            // 
            // butOneClear
            // 
            this.butOneClear.Location = new System.Drawing.Point(102, 58);
            this.butOneClear.Name = "butOneClear";
            this.butOneClear.Size = new System.Drawing.Size(88, 23);
            this.butOneClear.TabIndex = 5;
            this.butOneClear.Text = "清除";
            this.butOneClear.UseVisualStyleBackColor = true;
            this.butOneClear.Click += new System.EventHandler(this.butOneClear_Click);
            // 
            // butAllClear
            // 
            this.butAllClear.Location = new System.Drawing.Point(3, 87);
            this.butAllClear.Name = "butAllClear";
            this.butAllClear.Size = new System.Drawing.Size(187, 23);
            this.butAllClear.TabIndex = 6;
            this.butAllClear.Text = "全部清除";
            this.butAllClear.UseVisualStyleBackColor = true;
            this.butAllClear.Click += new System.EventHandler(this.butAllClear_Click);
            // 
            // butLookLogFile
            // 
            this.butLookLogFile.Location = new System.Drawing.Point(3, 116);
            this.butLookLogFile.Name = "butLookLogFile";
            this.butLookLogFile.Size = new System.Drawing.Size(187, 23);
            this.butLookLogFile.TabIndex = 7;
            this.butLookLogFile.Text = "查看当前日志文件";
            this.butLookLogFile.UseVisualStyleBackColor = true;
            this.butLookLogFile.Click += new System.EventHandler(this.butLookLogFile_Click);
            // 
            // txtBoxLogFile
            // 
            this.txtBoxLogFile.Location = new System.Drawing.Point(3, 145);
            this.txtBoxLogFile.Name = "txtBoxLogFile";
            this.txtBoxLogFile.Size = new System.Drawing.Size(141, 21);
            this.txtBoxLogFile.TabIndex = 8;
            // 
            // butSelctionLogFile
            // 
            this.butSelctionLogFile.Location = new System.Drawing.Point(150, 145);
            this.butSelctionLogFile.Name = "butSelctionLogFile";
            this.butSelctionLogFile.Size = new System.Drawing.Size(38, 23);
            this.butSelctionLogFile.TabIndex = 9;
            this.butSelctionLogFile.Text = "选择";
            this.butSelctionLogFile.UseVisualStyleBackColor = true;
            this.butSelctionLogFile.Click += new System.EventHandler(this.butSelctionLogFile_Click);
            // 
            // butLookAtLogFile
            // 
            this.butLookAtLogFile.Location = new System.Drawing.Point(3, 174);
            this.butLookAtLogFile.Name = "butLookAtLogFile";
            this.butLookAtLogFile.Size = new System.Drawing.Size(187, 23);
            this.butLookAtLogFile.TabIndex = 10;
            this.butLookAtLogFile.Text = "查看指定日志文件";
            this.butLookAtLogFile.UseVisualStyleBackColor = true;
            this.butLookAtLogFile.Click += new System.EventHandler(this.butLookAtLogFile_Click);
            // 
            // IndexForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1007, 581);
            this.Controls.Add(this.flowLayoutPanel1);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.Name = "IndexForm";
            this.Text = "IndexForm";
            this.SizeChanged += new System.EventHandler(this.IndexForm_SizeChanged);
            this.panel1.ResumeLayout(false);
            this.flowLayoutPanel1.ResumeLayout(false);
            this.flowLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.Button butStartMonitor;
        private System.Windows.Forms.ComboBox comBoxClient;
        private System.Windows.Forms.Button butPause;
        private System.Windows.Forms.Button butOneClear;
        private System.Windows.Forms.Button butAllClear;
        private System.Windows.Forms.Button butLookLogFile;
        private System.Windows.Forms.TextBox txtBoxLogFile;
        private System.Windows.Forms.Button butSelctionLogFile;
        private System.Windows.Forms.Button butLookAtLogFile;
    }
}