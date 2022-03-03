namespace LoadBingPicture
{
    partial class frmMain
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmMain));
            this.label1 = new System.Windows.Forms.Label();
            this.btnSetDesktop = new System.Windows.Forms.Button();
            this.notifyIcon1 = new System.Windows.Forms.NotifyIcon(this.components);
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.selResolution02 = new System.Windows.Forms.RadioButton();
            this.selResolution01 = new System.Windows.Forms.RadioButton();
            this.selResolution00 = new System.Windows.Forms.RadioButton();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.btnRefresh = new System.Windows.Forms.Button();
            this.chkInfo = new System.Windows.Forms.CheckBox();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(0, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(100, 23);
            this.label1.TabIndex = 7;
            // 
            // btnSetDesktop
            // 
            this.btnSetDesktop.Location = new System.Drawing.Point(0, 0);
            this.btnSetDesktop.Name = "btnSetDesktop";
            this.btnSetDesktop.Size = new System.Drawing.Size(75, 23);
            this.btnSetDesktop.TabIndex = 8;
            // 
            // notifyIcon1
            // 
            this.notifyIcon1.Icon = ((System.Drawing.Icon)(resources.GetObject("notifyIcon1.Icon")));
            this.notifyIcon1.Text = "Load Bing Picture";
            this.notifyIcon1.Visible = true;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Location = new System.Drawing.Point(143, 27);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(320, 180);
            this.pictureBox1.TabIndex = 9;
            this.pictureBox1.TabStop = false;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.selResolution02);
            this.groupBox1.Controls.Add(this.selResolution01);
            this.groupBox1.Controls.Add(this.selResolution00);
            this.groupBox1.Location = new System.Drawing.Point(487, 27);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(113, 122);
            this.groupBox1.TabIndex = 10;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Resolution";
            // 
            // selResolution02
            // 
            this.selResolution02.AutoSize = true;
            this.selResolution02.Location = new System.Drawing.Point(22, 79);
            this.selResolution02.Name = "selResolution02";
            this.selResolution02.Size = new System.Drawing.Size(49, 17);
            this.selResolution02.TabIndex = 2;
            this.selResolution02.Text = "UHD";
            this.selResolution02.UseVisualStyleBackColor = true;
            this.selResolution02.CheckedChanged += new System.EventHandler(this.selResolution_CheckedChanged);
            // 
            // selResolution01
            // 
            this.selResolution01.AutoSize = true;
            this.selResolution01.Location = new System.Drawing.Point(22, 56);
            this.selResolution01.Name = "selResolution01";
            this.selResolution01.Size = new System.Drawing.Size(78, 17);
            this.selResolution01.TabIndex = 1;
            this.selResolution01.Text = "1920x1080";
            this.selResolution01.UseVisualStyleBackColor = true;
            this.selResolution01.CheckedChanged += new System.EventHandler(this.selResolution_CheckedChanged);
            // 
            // selResolution00
            // 
            this.selResolution00.AutoSize = true;
            this.selResolution00.Location = new System.Drawing.Point(22, 33);
            this.selResolution00.Name = "selResolution00";
            this.selResolution00.Size = new System.Drawing.Size(72, 17);
            this.selResolution00.TabIndex = 0;
            this.selResolution00.Text = "1366x768";
            this.selResolution00.UseVisualStyleBackColor = true;
            this.selResolution00.CheckedChanged += new System.EventHandler(this.selResolution_CheckedChanged);
            // 
            // timer1
            // 
            this.timer1.Interval = 60000;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // listBox1
            // 
            this.listBox1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.listBox1.FormattingEnabled = true;
            this.listBox1.Location = new System.Drawing.Point(0, 259);
            this.listBox1.Name = "listBox1";
            this.listBox1.Size = new System.Drawing.Size(632, 95);
            this.listBox1.TabIndex = 11;
            // 
            // btnRefresh
            // 
            this.btnRefresh.Location = new System.Drawing.Point(487, 184);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(113, 23);
            this.btnRefresh.TabIndex = 12;
            this.btnRefresh.Text = "Refresh";
            this.btnRefresh.UseVisualStyleBackColor = true;
            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
            // 
            // chkInfo
            // 
            this.chkInfo.AutoSize = true;
            this.chkInfo.Checked = true;
            this.chkInfo.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkInfo.Location = new System.Drawing.Point(495, 161);
            this.chkInfo.Name = "chkInfo";
            this.chkInfo.Size = new System.Drawing.Size(73, 17);
            this.chkInfo.TabIndex = 13;
            this.chkInfo.Text = "Show info";
            this.chkInfo.UseVisualStyleBackColor = true;
            this.chkInfo.CheckedChanged += new System.EventHandler(this.chkInfo_CheckedChanged);
            // 
            // comboBox1
            // 
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Location = new System.Drawing.Point(12, 29);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(121, 21);
            this.comboBox1.TabIndex = 14;
            // 
            // frmMain
            // 
            this.AcceptButton = this.btnSetDesktop;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(632, 354);
            this.Controls.Add(this.comboBox1);
            this.Controls.Add(this.chkInfo);
            this.Controls.Add(this.btnRefresh);
            this.Controls.Add(this.listBox1);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnSetDesktop);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "frmMain";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Load Bing Picture";
            this.WindowState = System.Windows.Forms.FormWindowState.Minimized;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.Resize += new System.EventHandler(this.Form1_Resize);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnSetDesktop;
        private System.Windows.Forms.NotifyIcon notifyIcon1;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton selResolution02;
        private System.Windows.Forms.RadioButton selResolution01;
        private System.Windows.Forms.RadioButton selResolution00;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.ListBox listBox1;
        private System.Windows.Forms.Button btnRefresh;
        private System.Windows.Forms.CheckBox chkInfo;
        private System.Windows.Forms.ComboBox comboBox1;
    }
}

