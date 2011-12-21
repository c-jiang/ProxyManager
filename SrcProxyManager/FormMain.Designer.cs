namespace ProxyManager
{
    partial class FormMain
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
            if (disposing && (components != null)) {
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormMain));
            this.tbStatus = new System.Windows.Forms.TextBox();
            this.btnRefresh = new System.Windows.Forms.Button();
            this.btnProxyMode = new System.Windows.Forms.Button();
            this.btnDirectMode = new System.Windows.Forms.Button();
            this.btnAutoMode = new System.Windows.Forms.Button();
            this.gbWorkMode = new System.Windows.Forms.GroupBox();
            this.msMain = new System.Windows.Forms.MenuStrip();
            this.mainToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.minTotrayToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.notifyIcon = new System.Windows.Forms.NotifyIcon(this.components);
            this.gbNetworkOverview = new System.Windows.Forms.GroupBox();
            this.labelProxyAddr = new System.Windows.Forms.Label();
            this.gbWorkMode.SuspendLayout();
            this.msMain.SuspendLayout();
            this.gbNetworkOverview.SuspendLayout();
            this.SuspendLayout();
            // 
            // tbStatus
            // 
            this.tbStatus.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tbStatus.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tbStatus.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.tbStatus.Font = new System.Drawing.Font("Lucida Console", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbStatus.Location = new System.Drawing.Point(6, 19);
            this.tbStatus.Multiline = true;
            this.tbStatus.Name = "tbStatus";
            this.tbStatus.ReadOnly = true;
            this.tbStatus.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.tbStatus.Size = new System.Drawing.Size(355, 186);
            this.tbStatus.TabIndex = 0;
            this.tbStatus.TabStop = false;
            this.tbStatus.WordWrap = false;
            // 
            // btnRefresh
            // 
            this.btnRefresh.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnRefresh.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRefresh.Location = new System.Drawing.Point(286, 211);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(75, 23);
            this.btnRefresh.TabIndex = 2;
            this.btnRefresh.Text = "Refresh";
            this.btnRefresh.UseVisualStyleBackColor = true;
            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
            // 
            // btnProxyMode
            // 
            this.btnProxyMode.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)));
            this.btnProxyMode.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnProxyMode.Location = new System.Drawing.Point(233, 18);
            this.btnProxyMode.Name = "btnProxyMode";
            this.btnProxyMode.Size = new System.Drawing.Size(88, 23);
            this.btnProxyMode.TabIndex = 2;
            this.btnProxyMode.Text = "Proxy Mode";
            this.btnProxyMode.UseVisualStyleBackColor = true;
            this.btnProxyMode.Click += new System.EventHandler(this.btnProxyMode_Click);
            // 
            // btnDirectMode
            // 
            this.btnDirectMode.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)));
            this.btnDirectMode.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDirectMode.Location = new System.Drawing.Point(139, 18);
            this.btnDirectMode.Name = "btnDirectMode";
            this.btnDirectMode.Size = new System.Drawing.Size(88, 23);
            this.btnDirectMode.TabIndex = 1;
            this.btnDirectMode.Text = "Direct Mode";
            this.btnDirectMode.UseVisualStyleBackColor = true;
            this.btnDirectMode.Click += new System.EventHandler(this.btnDirectMode_Click);
            // 
            // btnAutoMode
            // 
            this.btnAutoMode.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)));
            this.btnAutoMode.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAutoMode.Location = new System.Drawing.Point(45, 18);
            this.btnAutoMode.Name = "btnAutoMode";
            this.btnAutoMode.Size = new System.Drawing.Size(88, 23);
            this.btnAutoMode.TabIndex = 0;
            this.btnAutoMode.Text = "Auto Mode";
            this.btnAutoMode.UseVisualStyleBackColor = true;
            this.btnAutoMode.Click += new System.EventHandler(this.btnAutoMode_Click);
            // 
            // gbWorkMode
            // 
            this.gbWorkMode.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.gbWorkMode.Controls.Add(this.btnProxyMode);
            this.gbWorkMode.Controls.Add(this.btnDirectMode);
            this.gbWorkMode.Controls.Add(this.btnAutoMode);
            this.gbWorkMode.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.gbWorkMode.Location = new System.Drawing.Point(13, 280);
            this.gbWorkMode.Name = "gbWorkMode";
            this.gbWorkMode.Size = new System.Drawing.Size(367, 49);
            this.gbWorkMode.TabIndex = 2;
            this.gbWorkMode.TabStop = false;
            this.gbWorkMode.Text = "Work Mode:";
            // 
            // msMain
            // 
            this.msMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mainToolStripMenuItem,
            this.helpToolStripMenuItem});
            this.msMain.Location = new System.Drawing.Point(0, 0);
            this.msMain.Name = "msMain";
            this.msMain.Size = new System.Drawing.Size(392, 24);
            this.msMain.TabIndex = 0;
            this.msMain.Text = "Main Menu";
            // 
            // mainToolStripMenuItem
            // 
            this.mainToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.minTotrayToolStripMenuItem,
            this.toolStripSeparator1,
            this.exitToolStripMenuItem});
            this.mainToolStripMenuItem.Name = "mainToolStripMenuItem";
            this.mainToolStripMenuItem.Size = new System.Drawing.Size(41, 20);
            this.mainToolStripMenuItem.Text = "&Main";
            // 
            // minTotrayToolStripMenuItem
            // 
            this.minTotrayToolStripMenuItem.Name = "minTotrayToolStripMenuItem";
            this.minTotrayToolStripMenuItem.Size = new System.Drawing.Size(151, 22);
            this.minTotrayToolStripMenuItem.Text = "Mi&nimize to Tray";
            this.minTotrayToolStripMenuItem.Click += new System.EventHandler(this.minimizeToTrayToolStripMenuItem_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(148, 6);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(151, 22);
            this.exitToolStripMenuItem.Text = "&Exit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.aboutToolStripMenuItem});
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            this.helpToolStripMenuItem.Size = new System.Drawing.Size(40, 20);
            this.helpToolStripMenuItem.Text = "&Help";
            // 
            // aboutToolStripMenuItem
            // 
            this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            this.aboutToolStripMenuItem.Size = new System.Drawing.Size(103, 22);
            this.aboutToolStripMenuItem.Text = "&About";
            this.aboutToolStripMenuItem.Click += new System.EventHandler(this.aboutToolStripMenuItem_Click);
            // 
            // notifyIcon
            // 
            this.notifyIcon.Icon = ((System.Drawing.Icon)(resources.GetObject("notifyIcon.Icon")));
            this.notifyIcon.Text = "notifyIcon";
            this.notifyIcon.Visible = true;
            this.notifyIcon.Click += new System.EventHandler(this.notifyIcon_Click);
            // 
            // gbNetworkOverview
            // 
            this.gbNetworkOverview.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.gbNetworkOverview.Controls.Add(this.btnRefresh);
            this.gbNetworkOverview.Controls.Add(this.tbStatus);
            this.gbNetworkOverview.Controls.Add(this.labelProxyAddr);
            this.gbNetworkOverview.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.gbNetworkOverview.Location = new System.Drawing.Point(13, 28);
            this.gbNetworkOverview.Name = "gbNetworkOverview";
            this.gbNetworkOverview.Size = new System.Drawing.Size(367, 240);
            this.gbNetworkOverview.TabIndex = 1;
            this.gbNetworkOverview.TabStop = false;
            this.gbNetworkOverview.Text = "Network Overview";
            // 
            // labelProxyAddr
            // 
            this.labelProxyAddr.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.labelProxyAddr.AutoSize = true;
            this.labelProxyAddr.Location = new System.Drawing.Point(6, 216);
            this.labelProxyAddr.Name = "labelProxyAddr";
            this.labelProxyAddr.Size = new System.Drawing.Size(70, 13);
            this.labelProxyAddr.TabIndex = 1;
            this.labelProxyAddr.Text = "Proxy Server:";
            // 
            // FormMain
            // 
            this.AcceptButton = this.btnRefresh;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(392, 341);
            this.Controls.Add(this.gbWorkMode);
            this.Controls.Add(this.msMain);
            this.Controls.Add(this.gbNetworkOverview);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.msMain;
            this.MinimumSize = new System.Drawing.Size(340, 300);
            this.Name = "FormMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Resize += new System.EventHandler(this.FormMain_Resize);
            this.gbWorkMode.ResumeLayout(false);
            this.msMain.ResumeLayout(false);
            this.msMain.PerformLayout();
            this.gbNetworkOverview.ResumeLayout(false);
            this.gbNetworkOverview.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox tbStatus;
        private System.Windows.Forms.Button btnRefresh;
        private System.Windows.Forms.Button btnProxyMode;
        private System.Windows.Forms.Button btnDirectMode;
        private System.Windows.Forms.Button btnAutoMode;
        private System.Windows.Forms.GroupBox gbWorkMode;
        private System.Windows.Forms.MenuStrip msMain;
        private System.Windows.Forms.ToolStripMenuItem mainToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem minTotrayToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.NotifyIcon notifyIcon;
        private System.Windows.Forms.GroupBox gbNetworkOverview;
        private System.Windows.Forms.Label labelProxyAddr;
    }
}

