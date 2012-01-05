namespace ProxyManager
{
    partial class DlgOptions
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
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.gbCommon = new System.Windows.Forms.GroupBox();
            this.lblDefWorkMode = new System.Windows.Forms.Label();
            this.rbProxy = new System.Windows.Forms.RadioButton();
            this.rbDirect = new System.Windows.Forms.RadioButton();
            this.rbAuto = new System.Windows.Forms.RadioButton();
            this.cbLogToFile = new System.Windows.Forms.CheckBox();
            this.cbStartMinimized = new System.Windows.Forms.CheckBox();
            this.cbStartAuto = new System.Windows.Forms.CheckBox();
            this.gbProxyGroups = new System.Windows.Forms.GroupBox();
            this.btnDown = new System.Windows.Forms.Button();
            this.btnUp = new System.Windows.Forms.Button();
            this.btnDelete = new System.Windows.Forms.Button();
            this.btnEdit = new System.Windows.Forms.Button();
            this.btnNew = new System.Windows.Forms.Button();
            this.lvProxyGroups = new System.Windows.Forms.ListView();
            this.gbCommon.SuspendLayout();
            this.gbProxyGroups.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnOK
            // 
            this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOK.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnOK.Location = new System.Drawing.Point(186, 305);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 2;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCancel.Location = new System.Drawing.Point(267, 305);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 3;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // gbCommon
            // 
            this.gbCommon.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gbCommon.Controls.Add(this.lblDefWorkMode);
            this.gbCommon.Controls.Add(this.rbProxy);
            this.gbCommon.Controls.Add(this.rbDirect);
            this.gbCommon.Controls.Add(this.rbAuto);
            this.gbCommon.Controls.Add(this.cbLogToFile);
            this.gbCommon.Controls.Add(this.cbStartMinimized);
            this.gbCommon.Controls.Add(this.cbStartAuto);
            this.gbCommon.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.gbCommon.Location = new System.Drawing.Point(13, 13);
            this.gbCommon.Name = "gbCommon";
            this.gbCommon.Size = new System.Drawing.Size(329, 114);
            this.gbCommon.TabIndex = 0;
            this.gbCommon.TabStop = false;
            this.gbCommon.Text = "Common Settings";
            // 
            // lblDefWorkMode
            // 
            this.lblDefWorkMode.AutoSize = true;
            this.lblDefWorkMode.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.lblDefWorkMode.Location = new System.Drawing.Point(13, 21);
            this.lblDefWorkMode.Name = "lblDefWorkMode";
            this.lblDefWorkMode.Size = new System.Drawing.Size(103, 13);
            this.lblDefWorkMode.TabIndex = 0;
            this.lblDefWorkMode.Text = "Default Work Mode:";
            // 
            // rbProxy
            // 
            this.rbProxy.AutoSize = true;
            this.rbProxy.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.rbProxy.Location = new System.Drawing.Point(232, 19);
            this.rbProxy.Name = "rbProxy";
            this.rbProxy.Size = new System.Drawing.Size(50, 17);
            this.rbProxy.TabIndex = 3;
            this.rbProxy.TabStop = true;
            this.rbProxy.Text = "Proxy";
            this.rbProxy.UseVisualStyleBackColor = true;
            // 
            // rbDirect
            // 
            this.rbDirect.AutoSize = true;
            this.rbDirect.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.rbDirect.Location = new System.Drawing.Point(174, 19);
            this.rbDirect.Name = "rbDirect";
            this.rbDirect.Size = new System.Drawing.Size(52, 17);
            this.rbDirect.TabIndex = 2;
            this.rbDirect.TabStop = true;
            this.rbDirect.Text = "Direct";
            this.rbDirect.UseVisualStyleBackColor = true;
            // 
            // rbAuto
            // 
            this.rbAuto.AutoSize = true;
            this.rbAuto.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.rbAuto.Location = new System.Drawing.Point(122, 19);
            this.rbAuto.Name = "rbAuto";
            this.rbAuto.Size = new System.Drawing.Size(46, 17);
            this.rbAuto.TabIndex = 1;
            this.rbAuto.TabStop = true;
            this.rbAuto.Text = "Auto";
            this.rbAuto.UseVisualStyleBackColor = true;
            // 
            // cbLogToFile
            // 
            this.cbLogToFile.AutoSize = true;
            this.cbLogToFile.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cbLogToFile.Location = new System.Drawing.Point(16, 90);
            this.cbLogToFile.Name = "cbLogToFile";
            this.cbLogToFile.Size = new System.Drawing.Size(83, 17);
            this.cbLogToFile.TabIndex = 6;
            this.cbLogToFile.Text = "Log to logfile";
            this.cbLogToFile.UseVisualStyleBackColor = true;
            // 
            // cbStartMinimized
            // 
            this.cbStartMinimized.AutoSize = true;
            this.cbStartMinimized.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cbStartMinimized.Location = new System.Drawing.Point(16, 67);
            this.cbStartMinimized.Name = "cbStartMinimized";
            this.cbStartMinimized.Size = new System.Drawing.Size(199, 17);
            this.cbStartMinimized.TabIndex = 5;
            this.cbStartMinimized.Text = "Minimized to tray when program starts";
            this.cbStartMinimized.UseVisualStyleBackColor = true;
            // 
            // cbStartAuto
            // 
            this.cbStartAuto.AutoSize = true;
            this.cbStartAuto.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cbStartAuto.Location = new System.Drawing.Point(16, 44);
            this.cbStartAuto.Name = "cbStartAuto";
            this.cbStartAuto.Size = new System.Drawing.Size(172, 17);
            this.cbStartAuto.TabIndex = 4;
            this.cbStartAuto.Text = "Auto start with Windows startup";
            this.cbStartAuto.UseVisualStyleBackColor = true;
            // 
            // gbProxyGroups
            // 
            this.gbProxyGroups.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gbProxyGroups.Controls.Add(this.btnDown);
            this.gbProxyGroups.Controls.Add(this.btnUp);
            this.gbProxyGroups.Controls.Add(this.btnDelete);
            this.gbProxyGroups.Controls.Add(this.btnEdit);
            this.gbProxyGroups.Controls.Add(this.btnNew);
            this.gbProxyGroups.Controls.Add(this.lvProxyGroups);
            this.gbProxyGroups.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.gbProxyGroups.Location = new System.Drawing.Point(13, 134);
            this.gbProxyGroups.Name = "gbProxyGroups";
            this.gbProxyGroups.Size = new System.Drawing.Size(329, 165);
            this.gbProxyGroups.TabIndex = 1;
            this.gbProxyGroups.TabStop = false;
            this.gbProxyGroups.Text = "Proxy Groups";
            // 
            // btnDown
            // 
            this.btnDown.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnDown.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDown.Location = new System.Drawing.Point(273, 136);
            this.btnDown.Name = "btnDown";
            this.btnDown.Size = new System.Drawing.Size(50, 23);
            this.btnDown.TabIndex = 5;
            this.btnDown.Text = "Down";
            this.btnDown.UseVisualStyleBackColor = true;
            this.btnDown.Click += new System.EventHandler(this.btnDown_Click);
            // 
            // btnUp
            // 
            this.btnUp.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnUp.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnUp.Location = new System.Drawing.Point(273, 107);
            this.btnUp.Name = "btnUp";
            this.btnUp.Size = new System.Drawing.Size(50, 23);
            this.btnUp.TabIndex = 4;
            this.btnUp.Text = "Up";
            this.btnUp.UseVisualStyleBackColor = true;
            this.btnUp.Click += new System.EventHandler(this.btnUp_Click);
            // 
            // btnDelete
            // 
            this.btnDelete.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnDelete.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDelete.Location = new System.Drawing.Point(273, 78);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(50, 23);
            this.btnDelete.TabIndex = 3;
            this.btnDelete.Text = "Delete";
            this.btnDelete.UseVisualStyleBackColor = true;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // btnEdit
            // 
            this.btnEdit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnEdit.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnEdit.Location = new System.Drawing.Point(273, 49);
            this.btnEdit.Name = "btnEdit";
            this.btnEdit.Size = new System.Drawing.Size(50, 23);
            this.btnEdit.TabIndex = 2;
            this.btnEdit.Text = "Edit";
            this.btnEdit.UseVisualStyleBackColor = true;
            this.btnEdit.Click += new System.EventHandler(this.btnEdit_Click);
            // 
            // btnNew
            // 
            this.btnNew.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnNew.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnNew.Location = new System.Drawing.Point(273, 20);
            this.btnNew.Name = "btnNew";
            this.btnNew.Size = new System.Drawing.Size(50, 23);
            this.btnNew.TabIndex = 1;
            this.btnNew.Text = "New";
            this.btnNew.UseVisualStyleBackColor = true;
            this.btnNew.Click += new System.EventHandler(this.btnNew_Click);
            // 
            // lvProxyGroups
            // 
            this.lvProxyGroups.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lvProxyGroups.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lvProxyGroups.FullRowSelect = true;
            this.lvProxyGroups.GridLines = true;
            this.lvProxyGroups.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.lvProxyGroups.Location = new System.Drawing.Point(7, 20);
            this.lvProxyGroups.MultiSelect = false;
            this.lvProxyGroups.Name = "lvProxyGroups";
            this.lvProxyGroups.Size = new System.Drawing.Size(260, 139);
            this.lvProxyGroups.TabIndex = 0;
            this.lvProxyGroups.UseCompatibleStateImageBehavior = false;
            this.lvProxyGroups.View = System.Windows.Forms.View.Details;
            this.lvProxyGroups.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.lvProxyGroups_MouseDoubleClick);
            // 
            // DlgOptions
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(354, 340);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.gbCommon);
            this.Controls.Add(this.gbProxyGroups);
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(370, 378);
            this.Name = "DlgOptions";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Options";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.DlgOptions_FormClosed);
            this.Shown += new System.EventHandler(this.DlgOptions_Shown);
            this.gbCommon.ResumeLayout(false);
            this.gbCommon.PerformLayout();
            this.gbProxyGroups.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.GroupBox gbCommon;
        private System.Windows.Forms.CheckBox cbStartAuto;
        private System.Windows.Forms.CheckBox cbStartMinimized;
        private System.Windows.Forms.CheckBox cbLogToFile;
        private System.Windows.Forms.RadioButton rbAuto;
        private System.Windows.Forms.RadioButton rbProxy;
        private System.Windows.Forms.RadioButton rbDirect;
        private System.Windows.Forms.Label lblDefWorkMode;
        private System.Windows.Forms.GroupBox gbProxyGroups;
        private System.Windows.Forms.ListView lvProxyGroups;
        private System.Windows.Forms.Button btnDown;
        private System.Windows.Forms.Button btnUp;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.Button btnEdit;
        private System.Windows.Forms.Button btnNew;
    }
}