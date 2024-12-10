namespace PCRobotApp
{
    partial class Form1
    {
        private System.ComponentModel.IContainer components = null;

        private System.Windows.Forms.TextBox txtToken;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnTest;
        private System.Windows.Forms.Button btnStartBot;
        private System.Windows.Forms.Button btnStopBot;
        private System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.LinkLabel linkCredit;
        private System.Windows.Forms.NotifyIcon notifyIcon;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem menuRestore;
        private System.Windows.Forms.ToolStripMenuItem menuExit;
        private System.Windows.Forms.CheckBox chkStartWithWindows;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.txtToken = new System.Windows.Forms.TextBox();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnTest = new System.Windows.Forms.Button();
            this.btnStartBot = new System.Windows.Forms.Button();
            this.btnStopBot = new System.Windows.Forms.Button();
            this.lblStatus = new System.Windows.Forms.Label();
            this.linkCredit = new System.Windows.Forms.LinkLabel();
            this.notifyIcon = new System.Windows.Forms.NotifyIcon(this.components);
            this.contextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.menuRestore = new System.Windows.Forms.ToolStripMenuItem();
            this.menuExit = new System.Windows.Forms.ToolStripMenuItem();
            this.chkStartWithWindows = new System.Windows.Forms.CheckBox();
            this.contextMenuStrip.SuspendLayout();
            this.SuspendLayout();

            // txtToken
            this.txtToken.Location = new System.Drawing.Point(12, 25);
            this.txtToken.Name = "txtToken";
            this.txtToken.Size = new System.Drawing.Size(500, 23);
            this.txtToken.TabIndex = 0;

            // btnSave
            this.btnSave.Location = new System.Drawing.Point(12, 60);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(100, 30);
            this.btnSave.TabIndex = 1;
            this.btnSave.Text = "Save Token";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);

            // btnTest
            this.btnTest.Location = new System.Drawing.Point(130, 60);
            this.btnTest.Name = "btnTest";
            this.btnTest.Size = new System.Drawing.Size(100, 30);
            this.btnTest.TabIndex = 2;
            this.btnTest.Text = "Test Token";
            this.btnTest.UseVisualStyleBackColor = true;
            this.btnTest.Click += new System.EventHandler(this.btnTest_Click);

            // btnStartBot
            this.btnStartBot.Location = new System.Drawing.Point(250, 60);
            this.btnStartBot.Name = "btnStartBot";
            this.btnStartBot.Size = new System.Drawing.Size(100, 30);
            this.btnStartBot.TabIndex = 3;
            this.btnStartBot.Text = "Start Bot";
            this.btnStartBot.UseVisualStyleBackColor = true;
            this.btnStartBot.Click += new System.EventHandler(this.btnStartBot_Click);

            // btnStopBot
            this.btnStopBot.Location = new System.Drawing.Point(370, 60);
            this.btnStopBot.Name = "btnStopBot";
            this.btnStopBot.Size = new System.Drawing.Size(100, 30);
            this.btnStopBot.TabIndex = 4;
            this.btnStopBot.Text = "Stop Bot";
            this.btnStopBot.UseVisualStyleBackColor = true;
            this.btnStopBot.Enabled = false;
            this.btnStopBot.Click += new System.EventHandler(this.btnStopBot_Click);

            // lblStatus
            this.lblStatus.AutoSize = true;
            this.lblStatus.Location = new System.Drawing.Point(12, 100);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(39, 15);
            this.lblStatus.TabIndex = 5;
            this.lblStatus.Text = "Ready";

            // linkCredit
            this.linkCredit.AutoSize = true;
            this.linkCredit.Location = new System.Drawing.Point(12, 150);
            this.linkCredit.Name = "linkCredit";
            this.linkCredit.Size = new System.Drawing.Size(250, 15);
            this.linkCredit.TabIndex = 7;
            this.linkCredit.TabStop = true;
            this.linkCredit.Text = "Open-source at: monokaijs/pc-robot-telegram-bot-csharp";
            this.linkCredit.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.LinkCredit_LinkClicked);

            // notifyIcon
            this.notifyIcon.Text = "PC Robot Telegram Bot";
            this.notifyIcon.Visible = false;
            this.notifyIcon.Icon = System.Drawing.Icon.ExtractAssociatedIcon(Application.ExecutablePath);
            this.notifyIcon.ContextMenuStrip = this.contextMenuStrip;
            this.notifyIcon.DoubleClick += new System.EventHandler(this.NotifyIcon_DoubleClick);

            // contextMenuStrip
            this.contextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
                this.menuRestore,
                this.menuExit});
            this.contextMenuStrip.Name = "contextMenuStrip";
            this.contextMenuStrip.Size = new System.Drawing.Size(117, 48);

            // menuRestore
            this.menuRestore.Name = "menuRestore";
            this.menuRestore.Size = new System.Drawing.Size(116, 22);
            this.menuRestore.Text = "Restore";
            this.menuRestore.Click += new System.EventHandler(this.MenuRestore_Click);

            // menuExit
            this.menuExit.Name = "menuExit";
            this.menuExit.Size = new System.Drawing.Size(116, 22);
            this.menuExit.Text = "Exit";
            this.menuExit.Click += new System.EventHandler(this.MenuExit_Click);

            // chkStartWithWindows
            this.chkStartWithWindows.AutoSize = true;
            this.chkStartWithWindows.Location = new System.Drawing.Point(12, 120);
            this.chkStartWithWindows.Name = "chkStartWithWindows";
            this.chkStartWithWindows.Size = new System.Drawing.Size(126, 19);
            this.chkStartWithWindows.TabIndex = 6;
            this.chkStartWithWindows.Text = "Start with Windows";
            this.chkStartWithWindows.UseVisualStyleBackColor = true;
            this.chkStartWithWindows.CheckedChanged += new System.EventHandler(this.ChkStartWithWindows_CheckedChanged);

            // Form1
            this.ClientSize = new System.Drawing.Size(530, 180);
            this.Controls.Add(this.chkStartWithWindows);
            this.Controls.Add(this.linkCredit);
            this.Controls.Add(this.lblStatus);
            this.Controls.Add(this.btnStopBot);
            this.Controls.Add(this.btnStartBot);
            this.Controls.Add(this.btnTest);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.txtToken);
            this.Name = "Form1";
            this.Text = "PC Robot Telegram Bot Configurator";
            this.Resize += new System.EventHandler(this.Form1_Resize);

            this.contextMenuStrip.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();
        }
    }
}
