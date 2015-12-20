namespace ShutdownTimer
{
    partial class Form1
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.lblShutdownTime = new System.Windows.Forms.Label();
            this.txtHour = new System.Windows.Forms.TextBox();
            this.txtMin = new System.Windows.Forms.TextBox();
            this.btnSetTime = new System.Windows.Forms.Button();
            this.chBoxRS = new System.Windows.Forms.CheckBox();
            this.lblCurrentTime = new System.Windows.Forms.Label();
            this.chBoxAD = new System.Windows.Forms.CheckBox();
            this.lblStatus = new System.Windows.Forms.Label();
            this.comToD = new System.Windows.Forms.ComboBox();
            this.timer = new System.Windows.Forms.Timer(this.components);
            this.lblCD = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.notifyIcon = new System.Windows.Forms.NotifyIcon(this.components);
            this.comDelay = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // lblShutdownTime
            // 
            this.lblShutdownTime.AutoSize = true;
            this.lblShutdownTime.Location = new System.Drawing.Point(13, 51);
            this.lblShutdownTime.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblShutdownTime.Name = "lblShutdownTime";
            this.lblShutdownTime.Size = new System.Drawing.Size(111, 19);
            this.lblShutdownTime.TabIndex = 0;
            this.lblShutdownTime.Text = "Shutdown time:";
            // 
            // txtHour
            // 
            this.txtHour.Location = new System.Drawing.Point(90, 9);
            this.txtHour.Margin = new System.Windows.Forms.Padding(4);
            this.txtHour.MaxLength = 2;
            this.txtHour.Name = "txtHour";
            this.txtHour.Size = new System.Drawing.Size(34, 26);
            this.txtHour.TabIndex = 0;
            this.txtHour.Text = "12";
            // 
            // txtMin
            // 
            this.txtMin.Location = new System.Drawing.Point(151, 10);
            this.txtMin.Margin = new System.Windows.Forms.Padding(4);
            this.txtMin.MaxLength = 2;
            this.txtMin.Name = "txtMin";
            this.txtMin.Size = new System.Drawing.Size(40, 26);
            this.txtMin.TabIndex = 1;
            this.txtMin.Text = "00";
            // 
            // btnSetTime
            // 
            this.btnSetTime.Location = new System.Drawing.Point(268, 10);
            this.btnSetTime.Margin = new System.Windows.Forms.Padding(4);
            this.btnSetTime.Name = "btnSetTime";
            this.btnSetTime.Size = new System.Drawing.Size(90, 29);
            this.btnSetTime.TabIndex = 4;
            this.btnSetTime.Text = "Set Time";
            this.btnSetTime.UseVisualStyleBackColor = true;
            this.btnSetTime.MouseClick += new System.Windows.Forms.MouseEventHandler(this.btnSetTime_MouseClick);
            // 
            // chBoxRS
            // 
            this.chBoxRS.AutoSize = true;
            this.chBoxRS.Location = new System.Drawing.Point(228, 51);
            this.chBoxRS.Margin = new System.Windows.Forms.Padding(4);
            this.chBoxRS.Name = "chBoxRS";
            this.chBoxRS.Size = new System.Drawing.Size(130, 23);
            this.chBoxRS.TabIndex = 6;
            this.chBoxRS.Text = "Run at Start up";
            this.chBoxRS.UseVisualStyleBackColor = true;
            this.chBoxRS.CheckedChanged += new System.EventHandler(this.chBoxRS_CheckedChanged);
            // 
            // lblCurrentTime
            // 
            this.lblCurrentTime.AutoSize = true;
            this.lblCurrentTime.Location = new System.Drawing.Point(16, 115);
            this.lblCurrentTime.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblCurrentTime.Name = "lblCurrentTime";
            this.lblCurrentTime.Size = new System.Drawing.Size(108, 19);
            this.lblCurrentTime.TabIndex = 7;
            this.lblCurrentTime.Text = "Current Time: ";
            // 
            // chBoxAD
            // 
            this.chBoxAD.AutoSize = true;
            this.chBoxAD.Location = new System.Drawing.Point(228, 78);
            this.chBoxAD.Margin = new System.Windows.Forms.Padding(4);
            this.chBoxAD.Name = "chBoxAD";
            this.chBoxAD.Size = new System.Drawing.Size(124, 23);
            this.chBoxAD.TabIndex = 5;
            this.chBoxAD.Text = "Activity Delay";
            this.chBoxAD.UseVisualStyleBackColor = true;
            this.chBoxAD.CheckedChanged += new System.EventHandler(this.chBoxAD_CheckedChanged);
            // 
            // lblStatus
            // 
            this.lblStatus.AutoSize = true;
            this.lblStatus.Location = new System.Drawing.Point(13, 18);
            this.lblStatus.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(69, 19);
            this.lblStatus.TabIndex = 9;
            this.lblStatus.Text = "Set Time";
            // 
            // comToD
            // 
            this.comToD.FormattingEnabled = true;
            this.comToD.Items.AddRange(new object[] {
            "AM",
            "PM"});
            this.comToD.Location = new System.Drawing.Point(199, 10);
            this.comToD.Margin = new System.Windows.Forms.Padding(4);
            this.comToD.Name = "comToD";
            this.comToD.Size = new System.Drawing.Size(61, 27);
            this.comToD.TabIndex = 2;
            // 
            // timer
            // 
            this.timer.Enabled = true;
            this.timer.Interval = 1000;
            this.timer.Tick += new System.EventHandler(this.timer_Tick);
            // 
            // lblCD
            // 
            this.lblCD.AutoSize = true;
            this.lblCD.Location = new System.Drawing.Point(14, 82);
            this.lblCD.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblCD.Name = "lblCD";
            this.lblCD.Size = new System.Drawing.Size(110, 19);
            this.lblCD.TabIndex = 11;
            this.lblCD.Text = "Current Delay:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(129, 12);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(14, 19);
            this.label2.TabIndex = 4;
            this.label2.Text = ":";
            // 
            // notifyIcon
            // 
            this.notifyIcon.BalloonTipIcon = System.Windows.Forms.ToolTipIcon.Info;
            this.notifyIcon.BalloonTipText = "Minimized to System Tray";
            this.notifyIcon.BalloonTipTitle = "Shutdown Timer";
            this.notifyIcon.Icon = ((System.Drawing.Icon)(resources.GetObject("notifyIcon.Icon")));
            this.notifyIcon.Text = "Shutdown Timer";
            // 
            // comDelay
            // 
            this.comDelay.FormattingEnabled = true;
            this.comDelay.Items.AddRange(new object[] {
            "5",
            "10",
            "15",
            "30",
            "60"});
            this.comDelay.Location = new System.Drawing.Point(199, 107);
            this.comDelay.Margin = new System.Windows.Forms.Padding(4);
            this.comDelay.Name = "comDelay";
            this.comDelay.Size = new System.Drawing.Size(61, 27);
            this.comDelay.TabIndex = 12;
            this.comDelay.SelectedIndexChanged += new System.EventHandler(this.comDelay_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(271, 110);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(81, 19);
            this.label1.TabIndex = 13;
            this.label1.Text = "Min Delay";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 19F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(362, 143);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.comDelay);
            this.Controls.Add(this.lblCD);
            this.Controls.Add(this.comToD);
            this.Controls.Add(this.lblStatus);
            this.Controls.Add(this.chBoxAD);
            this.Controls.Add(this.lblCurrentTime);
            this.Controls.Add(this.chBoxRS);
            this.Controls.Add(this.btnSetTime);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtMin);
            this.Controls.Add(this.txtHour);
            this.Controls.Add(this.lblShutdownTime);
            this.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "Form1";
            this.Text = "Shutdown Timer";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.Form1_MouseMove);
            this.Resize += new System.EventHandler(this.Form1_Resize);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblShutdownTime;
        private System.Windows.Forms.TextBox txtHour;
        private System.Windows.Forms.TextBox txtMin;
        private System.Windows.Forms.Button btnSetTime;
        private System.Windows.Forms.CheckBox chBoxRS;
        private System.Windows.Forms.Label lblCurrentTime;
        private System.Windows.Forms.CheckBox chBoxAD;
        private System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.ComboBox comToD;
        private System.Windows.Forms.Timer timer;
        private System.Windows.Forms.Label lblCD;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.NotifyIcon notifyIcon;
        private System.Windows.Forms.ComboBox comDelay;
        private System.Windows.Forms.Label label1;
    }
}

