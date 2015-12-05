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
            this.SuspendLayout();
            // 
            // lblShutdownTime
            // 
            this.lblShutdownTime.AutoSize = true;
            this.lblShutdownTime.Location = new System.Drawing.Point(12, 9);
            this.lblShutdownTime.Name = "lblShutdownTime";
            this.lblShutdownTime.Size = new System.Drawing.Size(80, 13);
            this.lblShutdownTime.TabIndex = 0;
            this.lblShutdownTime.Text = "Shutdown time:";
            // 
            // txtHour
            // 
            this.txtHour.Location = new System.Drawing.Point(75, 30);
            this.txtHour.MaxLength = 2;
            this.txtHour.Name = "txtHour";
            this.txtHour.Size = new System.Drawing.Size(24, 20);
            this.txtHour.TabIndex = 0;
            this.txtHour.Text = "12";
            // 
            // txtMin
            // 
            this.txtMin.Location = new System.Drawing.Point(121, 30);
            this.txtMin.MaxLength = 2;
            this.txtMin.Name = "txtMin";
            this.txtMin.Size = new System.Drawing.Size(28, 20);
            this.txtMin.TabIndex = 1;
            this.txtMin.Text = "00";
            // 
            // btnSetTime
            // 
            this.btnSetTime.Location = new System.Drawing.Point(203, 30);
            this.btnSetTime.Name = "btnSetTime";
            this.btnSetTime.Size = new System.Drawing.Size(60, 20);
            this.btnSetTime.TabIndex = 4;
            this.btnSetTime.Text = "Set Time";
            this.btnSetTime.UseVisualStyleBackColor = true;
            this.btnSetTime.MouseClick += new System.Windows.Forms.MouseEventHandler(this.btnSetTime_MouseClick);
            // 
            // chBoxRS
            // 
            this.chBoxRS.AutoSize = true;
            this.chBoxRS.Location = new System.Drawing.Point(175, 9);
            this.chBoxRS.Name = "chBoxRS";
            this.chBoxRS.Size = new System.Drawing.Size(98, 17);
            this.chBoxRS.TabIndex = 6;
            this.chBoxRS.Text = "Run at Start up";
            this.chBoxRS.UseVisualStyleBackColor = true;
            this.chBoxRS.CheckedChanged += new System.EventHandler(this.chBoxRS_CheckedChanged);
            // 
            // lblCurrentTime
            // 
            this.lblCurrentTime.AutoSize = true;
            this.lblCurrentTime.Location = new System.Drawing.Point(13, 77);
            this.lblCurrentTime.Name = "lblCurrentTime";
            this.lblCurrentTime.Size = new System.Drawing.Size(73, 13);
            this.lblCurrentTime.TabIndex = 7;
            this.lblCurrentTime.Text = "Current Time: ";
            // 
            // chBoxAD
            // 
            this.chBoxAD.AutoSize = true;
            this.chBoxAD.Location = new System.Drawing.Point(173, 52);
            this.chBoxAD.Name = "chBoxAD";
            this.chBoxAD.Size = new System.Drawing.Size(90, 17);
            this.chBoxAD.TabIndex = 5;
            this.chBoxAD.Text = "Activity Delay";
            this.chBoxAD.UseVisualStyleBackColor = true;
            this.chBoxAD.CheckedChanged += new System.EventHandler(this.chBoxAD_CheckedChanged);
            // 
            // lblStatus
            // 
            this.lblStatus.AutoSize = true;
            this.lblStatus.Location = new System.Drawing.Point(20, 36);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(49, 13);
            this.lblStatus.TabIndex = 9;
            this.lblStatus.Text = "Set Time";
            // 
            // comToD
            // 
            this.comToD.FormattingEnabled = true;
            this.comToD.Items.AddRange(new object[] {
            "AM",
            "PM"});
            this.comToD.Location = new System.Drawing.Point(155, 29);
            this.comToD.Name = "comToD";
            this.comToD.Size = new System.Drawing.Size(42, 21);
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
            this.lblCD.Location = new System.Drawing.Point(12, 56);
            this.lblCD.Name = "lblCD";
            this.lblCD.Size = new System.Drawing.Size(74, 13);
            this.lblCD.TabIndex = 11;
            this.lblCD.Text = "Current Delay:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(105, 33);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(10, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = ":";
            // 
            // notifyIcon
            // 
            this.notifyIcon.BalloonTipIcon = System.Windows.Forms.ToolTipIcon.Info;
            this.notifyIcon.BalloonTipText = "Click to change settings";
            this.notifyIcon.BalloonTipTitle = "Shutdown Timer";
            this.notifyIcon.Text = "Shutdown Timer";
            this.notifyIcon.MouseClick += new System.Windows.Forms.MouseEventHandler(this.notifyIcon_MouseClick);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(286, 99);
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
    }
}

