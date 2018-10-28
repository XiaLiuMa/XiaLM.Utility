namespace XiaLM.FormTest.CAN
{
    partial class CanForm
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
            this.butConnectCAN = new System.Windows.Forms.Button();
            this.butStartCAN = new System.Windows.Forms.Button();
            this.butCloseCAN = new System.Windows.Forms.Button();
            this.butResetCAN = new System.Windows.Forms.Button();
            this.butSetCAN = new System.Windows.Forms.Button();
            this.butCANSend = new System.Windows.Forms.Button();
            this.tstFrameID = new System.Windows.Forms.TextBox();
            this.txtTimeStamp = new System.Windows.Forms.TextBox();
            this.txtFrameFormat = new System.Windows.Forms.TextBox();
            this.txtFrameType = new System.Windows.Forms.TextBox();
            this.txtFrameData = new System.Windows.Forms.TextBox();
            this.txtFrameSendType = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // butConnectCAN
            // 
            this.butConnectCAN.Location = new System.Drawing.Point(109, 12);
            this.butConnectCAN.Name = "butConnectCAN";
            this.butConnectCAN.Size = new System.Drawing.Size(75, 23);
            this.butConnectCAN.TabIndex = 0;
            this.butConnectCAN.Text = "连接";
            this.butConnectCAN.UseVisualStyleBackColor = true;
            this.butConnectCAN.Click += new System.EventHandler(this.butConnectCAN_Click);
            // 
            // butStartCAN
            // 
            this.butStartCAN.Location = new System.Drawing.Point(207, 12);
            this.butStartCAN.Name = "butStartCAN";
            this.butStartCAN.Size = new System.Drawing.Size(75, 23);
            this.butStartCAN.TabIndex = 1;
            this.butStartCAN.Text = "启动";
            this.butStartCAN.UseVisualStyleBackColor = true;
            this.butStartCAN.Click += new System.EventHandler(this.butStartCAN_Click);
            // 
            // butCloseCAN
            // 
            this.butCloseCAN.Location = new System.Drawing.Point(306, 12);
            this.butCloseCAN.Name = "butCloseCAN";
            this.butCloseCAN.Size = new System.Drawing.Size(75, 23);
            this.butCloseCAN.TabIndex = 2;
            this.butCloseCAN.Text = "关闭";
            this.butCloseCAN.UseVisualStyleBackColor = true;
            this.butCloseCAN.Click += new System.EventHandler(this.butCloseCAN_Click);
            // 
            // butResetCAN
            // 
            this.butResetCAN.Location = new System.Drawing.Point(399, 12);
            this.butResetCAN.Name = "butResetCAN";
            this.butResetCAN.Size = new System.Drawing.Size(75, 23);
            this.butResetCAN.TabIndex = 3;
            this.butResetCAN.Text = "复位";
            this.butResetCAN.UseVisualStyleBackColor = true;
            this.butResetCAN.Click += new System.EventHandler(this.butResetCAN_Click);
            // 
            // butSetCAN
            // 
            this.butSetCAN.Location = new System.Drawing.Point(12, 12);
            this.butSetCAN.Name = "butSetCAN";
            this.butSetCAN.Size = new System.Drawing.Size(75, 23);
            this.butSetCAN.TabIndex = 4;
            this.butSetCAN.Text = "设置";
            this.butSetCAN.UseVisualStyleBackColor = true;
            this.butSetCAN.Click += new System.EventHandler(this.butSetCAN_Click);
            // 
            // butCANSend
            // 
            this.butCANSend.Location = new System.Drawing.Point(12, 73);
            this.butCANSend.Name = "butCANSend";
            this.butCANSend.Size = new System.Drawing.Size(462, 23);
            this.butCANSend.TabIndex = 5;
            this.butCANSend.Text = "发送";
            this.butCANSend.UseVisualStyleBackColor = true;
            this.butCANSend.Click += new System.EventHandler(this.butCANSend_Click);
            // 
            // tstFrameID
            // 
            this.tstFrameID.Location = new System.Drawing.Point(12, 49);
            this.tstFrameID.Name = "tstFrameID";
            this.tstFrameID.Size = new System.Drawing.Size(72, 21);
            this.tstFrameID.TabIndex = 6;
            // 
            // txtTimeStamp
            // 
            this.txtTimeStamp.Location = new System.Drawing.Point(90, 49);
            this.txtTimeStamp.Name = "txtTimeStamp";
            this.txtTimeStamp.Size = new System.Drawing.Size(72, 21);
            this.txtTimeStamp.TabIndex = 7;
            // 
            // txtFrameFormat
            // 
            this.txtFrameFormat.Location = new System.Drawing.Point(168, 49);
            this.txtFrameFormat.Name = "txtFrameFormat";
            this.txtFrameFormat.Size = new System.Drawing.Size(72, 21);
            this.txtFrameFormat.TabIndex = 8;
            // 
            // txtFrameType
            // 
            this.txtFrameType.Location = new System.Drawing.Point(246, 49);
            this.txtFrameType.Name = "txtFrameType";
            this.txtFrameType.Size = new System.Drawing.Size(72, 21);
            this.txtFrameType.TabIndex = 9;
            // 
            // txtFrameData
            // 
            this.txtFrameData.Location = new System.Drawing.Point(324, 49);
            this.txtFrameData.Name = "txtFrameData";
            this.txtFrameData.Size = new System.Drawing.Size(72, 21);
            this.txtFrameData.TabIndex = 10;
            // 
            // txtFrameSendType
            // 
            this.txtFrameSendType.Location = new System.Drawing.Point(402, 49);
            this.txtFrameSendType.Name = "txtFrameSendType";
            this.txtFrameSendType.Size = new System.Drawing.Size(72, 21);
            this.txtFrameSendType.TabIndex = 11;
            // 
            // CanForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(484, 361);
            this.Controls.Add(this.txtFrameSendType);
            this.Controls.Add(this.txtFrameData);
            this.Controls.Add(this.txtFrameType);
            this.Controls.Add(this.txtFrameFormat);
            this.Controls.Add(this.txtTimeStamp);
            this.Controls.Add(this.tstFrameID);
            this.Controls.Add(this.butCANSend);
            this.Controls.Add(this.butSetCAN);
            this.Controls.Add(this.butResetCAN);
            this.Controls.Add(this.butCloseCAN);
            this.Controls.Add(this.butStartCAN);
            this.Controls.Add(this.butConnectCAN);
            this.Name = "CanForm";
            this.Text = "CanForm";
            this.Load += new System.EventHandler(this.CanForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button butConnectCAN;
        private System.Windows.Forms.Button butStartCAN;
        private System.Windows.Forms.Button butCloseCAN;
        private System.Windows.Forms.Button butResetCAN;
        private System.Windows.Forms.Button butSetCAN;
        private System.Windows.Forms.Button butCANSend;
        private System.Windows.Forms.TextBox tstFrameID;
        private System.Windows.Forms.TextBox txtTimeStamp;
        private System.Windows.Forms.TextBox txtFrameFormat;
        private System.Windows.Forms.TextBox txtFrameType;
        private System.Windows.Forms.TextBox txtFrameData;
        private System.Windows.Forms.TextBox txtFrameSendType;
    }
}