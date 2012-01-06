namespace ProxyManager
{
    partial class DlgOptionsProxyGroup
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.dgvProxyItems = new System.Windows.Forms.DataGridView();
            this.ColEnable = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.ColProxyAddr = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColBypass = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.lblName = new System.Windows.Forms.Label();
            this.tbName = new System.Windows.Forms.TextBox();
            this.cbEnable = new System.Windows.Forms.CheckBox();
            this.gbApplyRule = new System.Windows.Forms.GroupBox();
            this.tlp = new System.Windows.Forms.TableLayoutPanel();
            this.cbFilterName = new System.Windows.Forms.CheckBox();
            this.cbFilterId = new System.Windows.Forms.CheckBox();
            this.cbFilterIpAddr = new System.Windows.Forms.CheckBox();
            this.cbFilterMask = new System.Windows.Forms.CheckBox();
            this.tbFilterGateway = new System.Windows.Forms.TextBox();
            this.cbFilterDns = new System.Windows.Forms.CheckBox();
            this.tbFilterId = new System.Windows.Forms.TextBox();
            this.tbFilterIpAddr = new System.Windows.Forms.TextBox();
            this.cbFilterGateway = new System.Windows.Forms.CheckBox();
            this.tbFilterName = new System.Windows.Forms.TextBox();
            this.tbFilterDns = new System.Windows.Forms.TextBox();
            this.cbFilterDnsSuffix = new System.Windows.Forms.CheckBox();
            this.tbFilterDnsSuffix = new System.Windows.Forms.TextBox();
            this.tbFilterMask = new System.Windows.Forms.TextBox();
            this.gbProxyItems = new System.Windows.Forms.GroupBox();
            this.btnDown = new System.Windows.Forms.Button();
            this.btnUp = new System.Windows.Forms.Button();
            this.btnDel = new System.Windows.Forms.Button();
            this.btnAdd = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dgvProxyItems)).BeginInit();
            this.gbApplyRule.SuspendLayout();
            this.tlp.SuspendLayout();
            this.gbProxyItems.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnOK
            // 
            this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOK.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnOK.Location = new System.Drawing.Point(414, 387);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 5;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCancel.Location = new System.Drawing.Point(495, 387);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 6;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // dgvProxyItems
            // 
            this.dgvProxyItems.AllowUserToResizeRows = false;
            this.dgvProxyItems.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvProxyItems.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvProxyItems.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dgvProxyItems.ColumnHeadersHeight = 20;
            this.dgvProxyItems.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dgvProxyItems.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ColEnable,
            this.ColProxyAddr,
            this.ColBypass});
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle4.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle4.Font = new System.Drawing.Font("Lucida Console", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle4.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle4.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle4.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvProxyItems.DefaultCellStyle = dataGridViewCellStyle4;
            this.dgvProxyItems.Location = new System.Drawing.Point(9, 19);
            this.dgvProxyItems.MultiSelect = false;
            this.dgvProxyItems.Name = "dgvProxyItems";
            this.dgvProxyItems.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle5.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle5.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle5.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle5.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle5.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle5.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvProxyItems.RowHeadersDefaultCellStyle = dataGridViewCellStyle5;
            this.dgvProxyItems.RowHeadersWidth = 24;
            this.dgvProxyItems.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.dgvProxyItems.RowTemplate.Height = 20;
            this.dgvProxyItems.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvProxyItems.Size = new System.Drawing.Size(483, 188);
            this.dgvProxyItems.TabIndex = 0;
            this.dgvProxyItems.RowsAdded += new System.Windows.Forms.DataGridViewRowsAddedEventHandler(this.dgvProxyItems_RowsAdded);
            // 
            // ColEnable
            // 
            this.ColEnable.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.ColumnHeader;
            this.ColEnable.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ColEnable.HeaderText = "Enable";
            this.ColEnable.Name = "ColEnable";
            this.ColEnable.Width = 46;
            // 
            // ColProxyAddr
            // 
            this.ColProxyAddr.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.Padding = new System.Windows.Forms.Padding(2, 0, 16, 0);
            this.ColProxyAddr.DefaultCellStyle = dataGridViewCellStyle2;
            this.ColProxyAddr.HeaderText = "Proxy Address";
            this.ColProxyAddr.Name = "ColProxyAddr";
            this.ColProxyAddr.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.ColProxyAddr.Width = 80;
            // 
            // ColBypass
            // 
            this.ColBypass.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.Padding = new System.Windows.Forms.Padding(2, 0, 16, 0);
            this.ColBypass.DefaultCellStyle = dataGridViewCellStyle3;
            this.ColBypass.HeaderText = "Bypass List";
            this.ColBypass.Name = "ColBypass";
            this.ColBypass.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // lblName
            // 
            this.lblName.AutoSize = true;
            this.lblName.Location = new System.Drawing.Point(12, 14);
            this.lblName.Name = "lblName";
            this.lblName.Size = new System.Drawing.Size(68, 13);
            this.lblName.TabIndex = 0;
            this.lblName.Text = "Proxy Group:";
            // 
            // tbName
            // 
            this.tbName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tbName.Location = new System.Drawing.Point(86, 12);
            this.tbName.Name = "tbName";
            this.tbName.Size = new System.Drawing.Size(250, 20);
            this.tbName.TabIndex = 1;
            // 
            // cbEnable
            // 
            this.cbEnable.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cbEnable.AutoSize = true;
            this.cbEnable.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cbEnable.Location = new System.Drawing.Point(418, 12);
            this.cbEnable.Name = "cbEnable";
            this.cbEnable.Size = new System.Drawing.Size(154, 17);
            this.cbEnable.TabIndex = 2;
            this.cbEnable.Text = "Enable Current Proxy Group";
            this.cbEnable.UseVisualStyleBackColor = true;
            // 
            // gbApplyRule
            // 
            this.gbApplyRule.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gbApplyRule.Controls.Add(this.tlp);
            this.gbApplyRule.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.gbApplyRule.Location = new System.Drawing.Point(15, 257);
            this.gbApplyRule.Name = "gbApplyRule";
            this.gbApplyRule.Size = new System.Drawing.Size(557, 124);
            this.gbApplyRule.TabIndex = 4;
            this.gbApplyRule.TabStop = false;
            this.gbApplyRule.Text = "Apply Rule for Current Proxy Group (Wildcard characters * and ? allowed)";
            // 
            // tlp
            // 
            this.tlp.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.tlp.ColumnCount = 5;
            this.tlp.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 135F));
            this.tlp.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlp.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 35F));
            this.tlp.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 135F));
            this.tlp.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlp.Controls.Add(this.cbFilterName, 0, 1);
            this.tlp.Controls.Add(this.cbFilterId, 0, 0);
            this.tlp.Controls.Add(this.cbFilterIpAddr, 0, 2);
            this.tlp.Controls.Add(this.cbFilterMask, 0, 3);
            this.tlp.Controls.Add(this.tbFilterGateway, 4, 0);
            this.tlp.Controls.Add(this.cbFilterDns, 3, 1);
            this.tlp.Controls.Add(this.tbFilterId, 1, 0);
            this.tlp.Controls.Add(this.tbFilterIpAddr, 1, 2);
            this.tlp.Controls.Add(this.cbFilterGateway, 3, 0);
            this.tlp.Controls.Add(this.tbFilterName, 1, 1);
            this.tlp.Controls.Add(this.tbFilterDns, 4, 1);
            this.tlp.Controls.Add(this.cbFilterDnsSuffix, 3, 2);
            this.tlp.Controls.Add(this.tbFilterDnsSuffix, 4, 2);
            this.tlp.Controls.Add(this.tbFilterMask, 1, 3);
            this.tlp.Location = new System.Drawing.Point(6, 19);
            this.tlp.Name = "tlp";
            this.tlp.RowCount = 4;
            this.tlp.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tlp.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tlp.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tlp.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tlp.Size = new System.Drawing.Size(545, 96);
            this.tlp.TabIndex = 0;
            // 
            // cbFilterName
            // 
            this.cbFilterName.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.cbFilterName.AutoSize = true;
            this.cbFilterName.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cbFilterName.Location = new System.Drawing.Point(3, 27);
            this.cbFilterName.Name = "cbFilterName";
            this.cbFilterName.Size = new System.Drawing.Size(116, 17);
            this.cbFilterName.TabIndex = 2;
            this.cbFilterName.Text = "Filter Adapter Name";
            this.cbFilterName.UseVisualStyleBackColor = true;
            this.cbFilterName.CheckedChanged += new System.EventHandler(this.cbFilterName_CheckedChanged);
            // 
            // cbFilterId
            // 
            this.cbFilterId.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.cbFilterId.AutoSize = true;
            this.cbFilterId.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cbFilterId.Location = new System.Drawing.Point(3, 3);
            this.cbFilterId.Name = "cbFilterId";
            this.cbFilterId.Size = new System.Drawing.Size(99, 17);
            this.cbFilterId.TabIndex = 0;
            this.cbFilterId.Text = "Filter Adapter ID";
            this.cbFilterId.UseVisualStyleBackColor = true;
            this.cbFilterId.CheckedChanged += new System.EventHandler(this.cbFilterId_CheckedChanged);
            // 
            // cbFilterIpAddr
            // 
            this.cbFilterIpAddr.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.cbFilterIpAddr.AutoSize = true;
            this.cbFilterIpAddr.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cbFilterIpAddr.Location = new System.Drawing.Point(3, 51);
            this.cbFilterIpAddr.Name = "cbFilterIpAddr";
            this.cbFilterIpAddr.Size = new System.Drawing.Size(99, 17);
            this.cbFilterIpAddr.TabIndex = 4;
            this.cbFilterIpAddr.Text = "Filter IP Address";
            this.cbFilterIpAddr.UseVisualStyleBackColor = true;
            this.cbFilterIpAddr.CheckedChanged += new System.EventHandler(this.cbFilterIpAddr_CheckedChanged);
            // 
            // cbFilterMask
            // 
            this.cbFilterMask.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.cbFilterMask.AutoSize = true;
            this.cbFilterMask.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cbFilterMask.Location = new System.Drawing.Point(3, 75);
            this.cbFilterMask.Name = "cbFilterMask";
            this.cbFilterMask.Size = new System.Drawing.Size(111, 17);
            this.cbFilterMask.TabIndex = 6;
            this.cbFilterMask.Text = "Filter Subnet Mask";
            this.cbFilterMask.UseVisualStyleBackColor = true;
            this.cbFilterMask.CheckedChanged += new System.EventHandler(this.cbFilterMask_CheckedChanged);
            // 
            // tbFilterGateway
            // 
            this.tbFilterGateway.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.tbFilterGateway.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tbFilterGateway.Font = new System.Drawing.Font("Lucida Console", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbFilterGateway.Location = new System.Drawing.Point(428, 3);
            this.tbFilterGateway.Name = "tbFilterGateway";
            this.tbFilterGateway.Size = new System.Drawing.Size(114, 19);
            this.tbFilterGateway.TabIndex = 9;
            // 
            // cbFilterDns
            // 
            this.cbFilterDns.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.cbFilterDns.AutoSize = true;
            this.cbFilterDns.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cbFilterDns.Location = new System.Drawing.Point(293, 27);
            this.cbFilterDns.Name = "cbFilterDns";
            this.cbFilterDns.Size = new System.Drawing.Size(108, 17);
            this.cbFilterDns.TabIndex = 10;
            this.cbFilterDns.Text = "Filter Default DNS";
            this.cbFilterDns.UseVisualStyleBackColor = true;
            this.cbFilterDns.CheckedChanged += new System.EventHandler(this.cbFilterDns_CheckedChanged);
            // 
            // tbFilterId
            // 
            this.tbFilterId.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.tbFilterId.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tbFilterId.Font = new System.Drawing.Font("Lucida Console", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbFilterId.Location = new System.Drawing.Point(138, 3);
            this.tbFilterId.Name = "tbFilterId";
            this.tbFilterId.Size = new System.Drawing.Size(114, 19);
            this.tbFilterId.TabIndex = 1;
            // 
            // tbFilterIpAddr
            // 
            this.tbFilterIpAddr.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.tbFilterIpAddr.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tbFilterIpAddr.Font = new System.Drawing.Font("Lucida Console", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbFilterIpAddr.Location = new System.Drawing.Point(138, 51);
            this.tbFilterIpAddr.Name = "tbFilterIpAddr";
            this.tbFilterIpAddr.Size = new System.Drawing.Size(114, 19);
            this.tbFilterIpAddr.TabIndex = 5;
            // 
            // cbFilterGateway
            // 
            this.cbFilterGateway.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.cbFilterGateway.AutoSize = true;
            this.cbFilterGateway.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cbFilterGateway.Location = new System.Drawing.Point(293, 3);
            this.cbFilterGateway.Name = "cbFilterGateway";
            this.cbFilterGateway.Size = new System.Drawing.Size(127, 17);
            this.cbFilterGateway.TabIndex = 8;
            this.cbFilterGateway.Text = "Filter Default Gateway";
            this.cbFilterGateway.UseVisualStyleBackColor = true;
            this.cbFilterGateway.CheckedChanged += new System.EventHandler(this.cbFilterGateway_CheckedChanged);
            // 
            // tbFilterName
            // 
            this.tbFilterName.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.tbFilterName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tbFilterName.Font = new System.Drawing.Font("Lucida Console", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbFilterName.Location = new System.Drawing.Point(138, 27);
            this.tbFilterName.Name = "tbFilterName";
            this.tbFilterName.Size = new System.Drawing.Size(114, 19);
            this.tbFilterName.TabIndex = 3;
            // 
            // tbFilterDns
            // 
            this.tbFilterDns.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.tbFilterDns.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tbFilterDns.Font = new System.Drawing.Font("Lucida Console", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbFilterDns.Location = new System.Drawing.Point(428, 27);
            this.tbFilterDns.Name = "tbFilterDns";
            this.tbFilterDns.Size = new System.Drawing.Size(114, 19);
            this.tbFilterDns.TabIndex = 11;
            // 
            // cbFilterDnsSuffix
            // 
            this.cbFilterDnsSuffix.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.cbFilterDnsSuffix.AutoSize = true;
            this.cbFilterDnsSuffix.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cbFilterDnsSuffix.Location = new System.Drawing.Point(293, 51);
            this.cbFilterDnsSuffix.Name = "cbFilterDnsSuffix";
            this.cbFilterDnsSuffix.Size = new System.Drawing.Size(100, 17);
            this.cbFilterDnsSuffix.TabIndex = 12;
            this.cbFilterDnsSuffix.Text = "Filter DNS Suffix";
            this.cbFilterDnsSuffix.UseVisualStyleBackColor = true;
            this.cbFilterDnsSuffix.CheckedChanged += new System.EventHandler(this.cbFilterDnsSuffix_CheckedChanged);
            // 
            // tbFilterDnsSuffix
            // 
            this.tbFilterDnsSuffix.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.tbFilterDnsSuffix.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tbFilterDnsSuffix.Font = new System.Drawing.Font("Lucida Console", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbFilterDnsSuffix.Location = new System.Drawing.Point(428, 51);
            this.tbFilterDnsSuffix.Name = "tbFilterDnsSuffix";
            this.tbFilterDnsSuffix.Size = new System.Drawing.Size(114, 19);
            this.tbFilterDnsSuffix.TabIndex = 13;
            // 
            // tbFilterMask
            // 
            this.tbFilterMask.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.tbFilterMask.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tbFilterMask.Font = new System.Drawing.Font("Lucida Console", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbFilterMask.Location = new System.Drawing.Point(138, 75);
            this.tbFilterMask.Name = "tbFilterMask";
            this.tbFilterMask.Size = new System.Drawing.Size(114, 19);
            this.tbFilterMask.TabIndex = 7;
            // 
            // gbProxyItems
            // 
            this.gbProxyItems.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gbProxyItems.Controls.Add(this.btnDown);
            this.gbProxyItems.Controls.Add(this.btnUp);
            this.gbProxyItems.Controls.Add(this.btnDel);
            this.gbProxyItems.Controls.Add(this.btnAdd);
            this.gbProxyItems.Controls.Add(this.dgvProxyItems);
            this.gbProxyItems.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.gbProxyItems.Location = new System.Drawing.Point(15, 38);
            this.gbProxyItems.Name = "gbProxyItems";
            this.gbProxyItems.Size = new System.Drawing.Size(557, 213);
            this.gbProxyItems.TabIndex = 3;
            this.gbProxyItems.TabStop = false;
            this.gbProxyItems.Text = "Items for Current Proxy Group";
            // 
            // btnDown
            // 
            this.btnDown.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnDown.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDown.Location = new System.Drawing.Point(498, 107);
            this.btnDown.Name = "btnDown";
            this.btnDown.Size = new System.Drawing.Size(50, 23);
            this.btnDown.TabIndex = 4;
            this.btnDown.Text = "Down";
            this.btnDown.UseVisualStyleBackColor = true;
            this.btnDown.Click += new System.EventHandler(this.btnDown_Click);
            // 
            // btnUp
            // 
            this.btnUp.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnUp.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnUp.Location = new System.Drawing.Point(498, 78);
            this.btnUp.Name = "btnUp";
            this.btnUp.Size = new System.Drawing.Size(50, 23);
            this.btnUp.TabIndex = 3;
            this.btnUp.Text = "Up";
            this.btnUp.UseVisualStyleBackColor = true;
            this.btnUp.Click += new System.EventHandler(this.btnUp_Click);
            // 
            // btnDel
            // 
            this.btnDel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnDel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDel.Location = new System.Drawing.Point(498, 49);
            this.btnDel.Name = "btnDel";
            this.btnDel.Size = new System.Drawing.Size(50, 23);
            this.btnDel.TabIndex = 2;
            this.btnDel.Text = "Del";
            this.btnDel.UseVisualStyleBackColor = true;
            this.btnDel.Click += new System.EventHandler(this.btnDel_Click);
            // 
            // btnAdd
            // 
            this.btnAdd.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAdd.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAdd.Location = new System.Drawing.Point(498, 20);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(50, 23);
            this.btnAdd.TabIndex = 1;
            this.btnAdd.Text = "Add";
            this.btnAdd.UseVisualStyleBackColor = true;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // DlgOptionsProxyGroup
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoValidate = System.Windows.Forms.AutoValidate.EnablePreventFocusChange;
            this.ClientSize = new System.Drawing.Size(584, 422);
            this.Controls.Add(this.cbEnable);
            this.Controls.Add(this.tbName);
            this.Controls.Add(this.lblName);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.gbApplyRule);
            this.Controls.Add(this.gbProxyItems);
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(500, 350);
            this.Name = "DlgOptionsProxyGroup";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Options - Proxy Group";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.DlgOptionsProxyGroup_FormClosing);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.DlgOptionsProxyGroup_FormClosed);
            this.Shown += new System.EventHandler(this.DlgOptionsProxyGroup_Shown);
            ((System.ComponentModel.ISupportInitialize)(this.dgvProxyItems)).EndInit();
            this.gbApplyRule.ResumeLayout(false);
            this.tlp.ResumeLayout(false);
            this.tlp.PerformLayout();
            this.gbProxyItems.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.DataGridView dgvProxyItems;
        private System.Windows.Forms.Label lblName;
        private System.Windows.Forms.TextBox tbName;
        private System.Windows.Forms.CheckBox cbEnable;
        private System.Windows.Forms.GroupBox gbApplyRule;
        private System.Windows.Forms.TableLayoutPanel tlp;
        private System.Windows.Forms.CheckBox cbFilterDns;
        private System.Windows.Forms.TextBox tbFilterId;
        private System.Windows.Forms.TextBox tbFilterName;
        private System.Windows.Forms.TextBox tbFilterIpAddr;
        private System.Windows.Forms.CheckBox cbFilterGateway;
        private System.Windows.Forms.CheckBox cbFilterMask;
        private System.Windows.Forms.TextBox tbFilterMask;
        private System.Windows.Forms.CheckBox cbFilterIpAddr;
        private System.Windows.Forms.TextBox tbFilterGateway;
        private System.Windows.Forms.CheckBox cbFilterName;
        private System.Windows.Forms.TextBox tbFilterDns;
        private System.Windows.Forms.CheckBox cbFilterId;
        private System.Windows.Forms.CheckBox cbFilterDnsSuffix;
        private System.Windows.Forms.TextBox tbFilterDnsSuffix;
        private System.Windows.Forms.GroupBox gbProxyItems;
        private System.Windows.Forms.Button btnDown;
        private System.Windows.Forms.Button btnUp;
        private System.Windows.Forms.Button btnDel;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.DataGridViewCheckBoxColumn ColEnable;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColProxyAddr;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColBypass;
    }
}