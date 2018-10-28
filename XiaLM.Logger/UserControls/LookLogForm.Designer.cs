namespace XiaLM.Logger.UserControls
{
    partial class LookLogForm
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
            this.panel2 = new System.Windows.Forms.Panel();
            this.labelDefault = new System.Windows.Forms.Label();
            this.radioButtonDebug = new System.Windows.Forms.RadioButton();
            this.radioButtonFatal = new System.Windows.Forms.RadioButton();
            this.radioButtonError = new System.Windows.Forms.RadioButton();
            this.radioButtonWarn = new System.Windows.Forms.RadioButton();
            this.radioButtonInfo = new System.Windows.Forms.RadioButton();
            this.radioButtonDefault = new System.Windows.Forms.RadioButton();
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
            this.labelInfo = new System.Windows.Forms.Label();
            this.labelWarn = new System.Windows.Forms.Label();
            this.labelError = new System.Windows.Forms.Label();
            this.labelFatal = new System.Windows.Forms.Label();
            this.labelDubeg = new System.Windows.Forms.Label();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel2
            // 
            this.panel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel2.Controls.Add(this.labelDubeg);
            this.panel2.Controls.Add(this.labelFatal);
            this.panel2.Controls.Add(this.labelError);
            this.panel2.Controls.Add(this.labelWarn);
            this.panel2.Controls.Add(this.labelInfo);
            this.panel2.Controls.Add(this.labelDefault);
            this.panel2.Controls.Add(this.radioButtonDebug);
            this.panel2.Controls.Add(this.radioButtonFatal);
            this.panel2.Controls.Add(this.radioButtonError);
            this.panel2.Controls.Add(this.radioButtonWarn);
            this.panel2.Controls.Add(this.radioButtonInfo);
            this.panel2.Controls.Add(this.radioButtonDefault);
            this.panel2.Controls.Add(this.richTextBox1);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(800, 450);
            this.panel2.TabIndex = 1;
            // 
            // labelDefault
            // 
            this.labelDefault.AutoSize = true;
            this.labelDefault.Location = new System.Drawing.Point(80, 11);
            this.labelDefault.Name = "labelDefault";
            this.labelDefault.Size = new System.Drawing.Size(23, 12);
            this.labelDefault.TabIndex = 7;
            this.labelDefault.Text = "0个";
            // 
            // radioButtonDebug
            // 
            this.radioButtonDebug.AutoSize = true;
            this.radioButtonDebug.Location = new System.Drawing.Point(683, 9);
            this.radioButtonDebug.Name = "radioButtonDebug";
            this.radioButtonDebug.Size = new System.Drawing.Size(53, 16);
            this.radioButtonDebug.TabIndex = 6;
            this.radioButtonDebug.Text = "Debug";
            this.radioButtonDebug.UseVisualStyleBackColor = true;
            this.radioButtonDebug.CheckedChanged += new System.EventHandler(this.radioButtonSelectModel_CheckedChanged);
            // 
            // radioButtonFatal
            // 
            this.radioButtonFatal.AutoSize = true;
            this.radioButtonFatal.Location = new System.Drawing.Point(552, 9);
            this.radioButtonFatal.Name = "radioButtonFatal";
            this.radioButtonFatal.Size = new System.Drawing.Size(53, 16);
            this.radioButtonFatal.TabIndex = 5;
            this.radioButtonFatal.Text = "Fatal";
            this.radioButtonFatal.UseVisualStyleBackColor = true;
            this.radioButtonFatal.CheckedChanged += new System.EventHandler(this.radioButtonSelectModel_CheckedChanged);
            // 
            // radioButtonError
            // 
            this.radioButtonError.AutoSize = true;
            this.radioButtonError.Location = new System.Drawing.Point(417, 9);
            this.radioButtonError.Name = "radioButtonError";
            this.radioButtonError.Size = new System.Drawing.Size(53, 16);
            this.radioButtonError.TabIndex = 4;
            this.radioButtonError.Text = "Error";
            this.radioButtonError.UseVisualStyleBackColor = true;
            this.radioButtonError.CheckedChanged += new System.EventHandler(this.radioButtonSelectModel_CheckedChanged);
            // 
            // radioButtonWarn
            // 
            this.radioButtonWarn.AutoSize = true;
            this.radioButtonWarn.Location = new System.Drawing.Point(289, 9);
            this.radioButtonWarn.Name = "radioButtonWarn";
            this.radioButtonWarn.Size = new System.Drawing.Size(47, 16);
            this.radioButtonWarn.TabIndex = 3;
            this.radioButtonWarn.Text = "Warn";
            this.radioButtonWarn.UseVisualStyleBackColor = true;
            this.radioButtonWarn.CheckedChanged += new System.EventHandler(this.radioButtonSelectModel_CheckedChanged);
            // 
            // radioButtonInfo
            // 
            this.radioButtonInfo.AutoSize = true;
            this.radioButtonInfo.Location = new System.Drawing.Point(157, 9);
            this.radioButtonInfo.Name = "radioButtonInfo";
            this.radioButtonInfo.Size = new System.Drawing.Size(47, 16);
            this.radioButtonInfo.TabIndex = 2;
            this.radioButtonInfo.Text = "Info";
            this.radioButtonInfo.UseVisualStyleBackColor = true;
            this.radioButtonInfo.CheckedChanged += new System.EventHandler(this.radioButtonSelectModel_CheckedChanged);
            // 
            // radioButtonDefault
            // 
            this.radioButtonDefault.AutoSize = true;
            this.radioButtonDefault.Checked = true;
            this.radioButtonDefault.Location = new System.Drawing.Point(10, 9);
            this.radioButtonDefault.Name = "radioButtonDefault";
            this.radioButtonDefault.Size = new System.Drawing.Size(65, 16);
            this.radioButtonDefault.TabIndex = 1;
            this.radioButtonDefault.TabStop = true;
            this.radioButtonDefault.Text = "Default";
            this.radioButtonDefault.UseVisualStyleBackColor = true;
            this.radioButtonDefault.CheckedChanged += new System.EventHandler(this.radioButtonSelectModel_CheckedChanged);
            // 
            // richTextBox1
            // 
            this.richTextBox1.Location = new System.Drawing.Point(1, 34);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.Size = new System.Drawing.Size(792, 411);
            this.richTextBox1.TabIndex = 0;
            this.richTextBox1.Text = "";
            // 
            // labelInfo
            // 
            this.labelInfo.AutoSize = true;
            this.labelInfo.Location = new System.Drawing.Point(210, 11);
            this.labelInfo.Name = "labelInfo";
            this.labelInfo.Size = new System.Drawing.Size(23, 12);
            this.labelInfo.TabIndex = 8;
            this.labelInfo.Text = "0个";
            // 
            // labelWarn
            // 
            this.labelWarn.AutoSize = true;
            this.labelWarn.Location = new System.Drawing.Point(342, 11);
            this.labelWarn.Name = "labelWarn";
            this.labelWarn.Size = new System.Drawing.Size(23, 12);
            this.labelWarn.TabIndex = 9;
            this.labelWarn.Text = "0个";
            // 
            // labelError
            // 
            this.labelError.AutoSize = true;
            this.labelError.Location = new System.Drawing.Point(476, 11);
            this.labelError.Name = "labelError";
            this.labelError.Size = new System.Drawing.Size(23, 12);
            this.labelError.TabIndex = 10;
            this.labelError.Text = "0个";
            // 
            // labelFatal
            // 
            this.labelFatal.AutoSize = true;
            this.labelFatal.Location = new System.Drawing.Point(611, 11);
            this.labelFatal.Name = "labelFatal";
            this.labelFatal.Size = new System.Drawing.Size(23, 12);
            this.labelFatal.TabIndex = 11;
            this.labelFatal.Text = "0个";
            // 
            // labelDubeg
            // 
            this.labelDubeg.AutoSize = true;
            this.labelDubeg.Location = new System.Drawing.Point(742, 11);
            this.labelDubeg.Name = "labelDubeg";
            this.labelDubeg.Size = new System.Drawing.Size(23, 12);
            this.labelDubeg.TabIndex = 12;
            this.labelDubeg.Text = "0个";
            // 
            // LookLogForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.panel2);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.Name = "LookLogForm";
            this.Text = "日志查看器";
            this.Load += new System.EventHandler(this.LookLogForm_Load);
            this.SizeChanged += new System.EventHandler(this.LookLogForm_SizeChanged);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.RichTextBox richTextBox1;
        private System.Windows.Forms.RadioButton radioButtonInfo;
        private System.Windows.Forms.RadioButton radioButtonDefault;
        private System.Windows.Forms.RadioButton radioButtonFatal;
        private System.Windows.Forms.RadioButton radioButtonError;
        private System.Windows.Forms.RadioButton radioButtonWarn;
        private System.Windows.Forms.RadioButton radioButtonDebug;
        private System.Windows.Forms.Label labelDefault;
        private System.Windows.Forms.Label labelDubeg;
        private System.Windows.Forms.Label labelFatal;
        private System.Windows.Forms.Label labelError;
        private System.Windows.Forms.Label labelWarn;
        private System.Windows.Forms.Label labelInfo;
    }
}