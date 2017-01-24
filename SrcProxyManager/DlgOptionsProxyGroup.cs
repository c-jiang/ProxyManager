using System;
using System.Windows.Forms;

namespace ProxyManager
{
    public partial class DlgOptionsProxyGroup : Form
    {
        private static DlgOptionsProxyGroup m_dlgInstance = null;
        private static ProxyGroup m_dlgProxyGroup = null;
        private static bool m_bExitByOK = false;

        private const int IDX_IS_ENABLED = 0;
        private const int IDX_PROXY_ADDR = 1;
        private const int IDX_BYPASS = 2;

        public static DlgOptionsProxyGroup Instance
        {
            get
            {
                if (m_dlgInstance == null) {
                    m_dlgInstance = new DlgOptionsProxyGroup();
                }
                return m_dlgInstance;
            }
        }

        public DialogResult ShowDialog(IWin32Window owner, ProxyGroup pg)
        {
            Logger.V(">> DlgOptionsProxyGroup.ShowDialog");
            m_bExitByOK = false;
            SetDialogLayout(pg);
            DialogResult dr = ShowDialog(owner);
            Logger.V("<< DlgOptionsProxyGroup.ShowDialog : " + dr.ToString());
            return dr;
        }

        public ProxyGroup DlgProxyGroup
        {
            get { return m_dlgProxyGroup; }
        }

        private DlgOptionsProxyGroup()
        {
            InitializeComponent();
        }

        #region Sync dialog layout and profile data

        private void SetDialogLayout(ProxyGroup pg)
        {
            if (pg != null) {
                m_dlgProxyGroup = new ProxyGroup(pg);
            } else {
                m_dlgProxyGroup = new ProxyGroup();
            }

            // proxy group name
            tbName.Text = m_dlgProxyGroup.m_szName;
            // proxy group enable/disable
            cbEnable.Checked = m_dlgProxyGroup.m_isEnabled;

            // proxy group items
            dgvProxyItems.Rows.Clear();
            if (m_dlgProxyGroup.m_listProxyItems != null) {
                foreach (ProxyItem pi in m_dlgProxyGroup.m_listProxyItems) {
                    int idx = dgvProxyItems.Rows.Add();
                    dgvProxyItems.Rows[idx].Cells[IDX_IS_ENABLED].Value = pi.m_isEnabled;
                    dgvProxyItems.Rows[idx].Cells[IDX_PROXY_ADDR].Value = pi.m_szProxyAddr;
                    dgvProxyItems.Rows[idx].Cells[IDX_BYPASS].Value = pi.m_szBypass;
                }
            }

            // proxy group apply rule
            if (m_dlgProxyGroup.m_applyRule == null) {
                m_dlgProxyGroup.m_applyRule = new ApplyRule();
            }
            // filter network adapter id
            cbFilterId.Checked = m_dlgProxyGroup.m_applyRule.m_bIdFilter;
            tbFilterId.Text = m_dlgProxyGroup.m_applyRule.m_szIdFilter;
            tbFilterId.Enabled = cbFilterId.Checked;
            // filter network adapter name
            cbFilterName.Checked = m_dlgProxyGroup.m_applyRule.m_bNameFilter;
            tbFilterName.Text = m_dlgProxyGroup.m_applyRule.m_szNameFilter;
            tbFilterName.Enabled = cbFilterName.Checked;
            // filter ip addr
            cbFilterIpAddr.Checked = m_dlgProxyGroup.m_applyRule.m_bIpAddrFilter;
            tbFilterIpAddr.Text = m_dlgProxyGroup.m_applyRule.m_szIpAddrFilter;
            tbFilterIpAddr.Enabled = cbFilterIpAddr.Checked;
            // filter subnet mask
            cbFilterMask.Checked = m_dlgProxyGroup.m_applyRule.m_bSubMaskFilter;
            tbFilterMask.Text = m_dlgProxyGroup.m_applyRule.m_szSubMaskFilter;
            tbFilterMask.Enabled = cbFilterMask.Checked;
            // filter gateway
            cbFilterGateway.Checked = m_dlgProxyGroup.m_applyRule.m_bGatewayFilter;
            tbFilterGateway.Text = m_dlgProxyGroup.m_applyRule.m_szGatewayFilter;
            tbFilterGateway.Enabled = cbFilterGateway.Checked;
            // filter dns addr
            cbFilterDns.Checked = m_dlgProxyGroup.m_applyRule.m_bDnsAddrFilter;
            tbFilterDns.Text = m_dlgProxyGroup.m_applyRule.m_szDnsAddrFilter;
            tbFilterDns.Enabled = cbFilterDns.Checked;
            // filter dns suffix
            cbFilterDnsSuffix.Checked = m_dlgProxyGroup.m_applyRule.m_bDnsSuffixListFilter;
            tbFilterDnsSuffix.Text = m_dlgProxyGroup.m_applyRule.m_szDnsSuffixListFilter;
            tbFilterDnsSuffix.Enabled = cbFilterDnsSuffix.Checked;
        }

        private void DlgOptionsProxyGroup_Shown(object sender, EventArgs e)
        {
            btnOK.Focus();
        }

        private void DlgOptionsProxyGroup_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!m_bExitByOK) {
                return;
            }

            tbName.Text = tbName.Text.Trim();
            tbFilterId.Text = tbFilterId.Text.Trim();
            tbFilterName.Text = tbFilterName.Text.Trim();
            tbFilterIpAddr.Text = tbFilterIpAddr.Text.Trim();
            tbFilterMask.Text = tbFilterMask.Text.Trim();
            tbFilterGateway.Text = tbFilterGateway.Text.Trim();
            tbFilterDns.Text = tbFilterDns.Text.Trim();
            tbFilterDnsSuffix.Text = tbFilterDnsSuffix.Text.Trim();

            string msg = String.Empty;

            if (tbName.Text.Length == 0) {
                msg = "Proxy Group name can not be blank.";
                tbName.Focus();
                goto END;
            }

            if (cbFilterId.Checked && tbFilterId.Text.Length == 0) {
                msg = "Expression for Adapter ID is blank while this filter is enabled.";
                tbFilterId.Focus();
                goto END;
            }
            if (cbFilterName.Checked && tbFilterName.Text.Length == 0) {
                msg = "Expression for Adapter Name is blank while this filter is enabled.";
                tbFilterName.Focus();
                goto END;
            }
            if (cbFilterIpAddr.Checked && tbFilterIpAddr.Text.Length == 0) {
                msg = "Expression for IP Address is blank while this filter is enabled.";
                tbFilterIpAddr.Focus();
                goto END;
            }
            if (cbFilterMask.Checked && tbFilterMask.Text.Length == 0) {
                msg = "Expression for Subnet Mask is blank while this filter is enabled.";
                tbFilterMask.Focus();
                goto END;
            }
            if (cbFilterGateway.Checked && tbFilterGateway.Text.Length == 0) {
                msg = "Expression for Default Gateway is blank while this filter is enabled.";
                tbFilterGateway.Focus();
                goto END;
            }
            if (cbFilterDns.Checked && tbFilterDns.Text.Length == 0) {
                msg = "Expression for Default DNS is blank while this filter is enabled.";
                tbFilterDns.Focus();
                goto END;
            }
            if (cbFilterDnsSuffix.Checked && tbFilterDnsSuffix.Text.Length == 0) {
                msg = "Expression for DNS Suffix is blank while this filter is enabled.";
                tbFilterDnsSuffix.Focus();
                goto END;
            }

            bool flag = cbFilterId.Checked
                      | cbFilterName.Checked
                      | cbFilterIpAddr.Checked
                      | cbFilterMask.Checked
                      | cbFilterGateway.Checked
                      | cbFilterDns.Checked
                      | cbFilterDnsSuffix.Checked;
            if (flag == false) {
                DialogResult dr = MessageBox.Show(
                    @"All filters have been turned off." + Environment.NewLine
                    + @"Are you sure to turn all of them off?",
                    AppManager.ASSEMBLY_PRODUCT,
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);
                if (dr == DialogResult.No) {
                    e.Cancel = true;
                    m_bExitByOK = false;
                    cbFilterId.Focus();
                }
            }

            return;
        END:
            e.Cancel = true;
            m_bExitByOK = false;
            MessageBox.Show(msg,
                AppManager.ASSEMBLY_PRODUCT,
                MessageBoxButtons.OK,
                MessageBoxIcon.Error);
        }

        private void DlgOptionsProxyGroup_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (!m_bExitByOK) {
                return;
            }

            // proxy group name
            m_dlgProxyGroup.m_szName = tbName.Text;
            // proxy group enable/disable
            m_dlgProxyGroup.m_isEnabled = cbEnable.Checked;

            // proxy group items
            m_dlgProxyGroup.m_listProxyItems.Clear();
            foreach (DataGridViewRow row in dgvProxyItems.Rows) {
                if (row.IsNewRow) {
                    continue;
                }
                m_dlgProxyGroup.m_listProxyItems.Add(new ProxyItem(
                    Boolean.Parse(row.Cells[IDX_IS_ENABLED].Value.ToString()),
                    row.Cells[IDX_PROXY_ADDR].Value.ToString().Trim(),
                    row.Cells[IDX_BYPASS].Value.ToString().Trim()));
            }

            // filter network adapter id
            m_dlgProxyGroup.m_applyRule.m_bIdFilter = cbFilterId.Checked;
            m_dlgProxyGroup.m_applyRule.m_szIdFilter = tbFilterId.Text;
            // filter network adapter name
            m_dlgProxyGroup.m_applyRule.m_bNameFilter = cbFilterName.Checked;
            m_dlgProxyGroup.m_applyRule.m_szNameFilter = tbFilterName.Text;
            // filter ip addr
            m_dlgProxyGroup.m_applyRule.m_bIpAddrFilter = cbFilterIpAddr.Checked;
            m_dlgProxyGroup.m_applyRule.m_szIpAddrFilter = tbFilterIpAddr.Text;
            // filter subnet mask
            m_dlgProxyGroup.m_applyRule.m_bSubMaskFilter = cbFilterMask.Checked;
            m_dlgProxyGroup.m_applyRule.m_szSubMaskFilter = tbFilterMask.Text;
            // filter gateway
            m_dlgProxyGroup.m_applyRule.m_bGatewayFilter = cbFilterGateway.Checked;
            m_dlgProxyGroup.m_applyRule.m_szGatewayFilter = tbFilterGateway.Text;
            // filter dns addr
            m_dlgProxyGroup.m_applyRule.m_bDnsAddrFilter = cbFilterDns.Checked;
            m_dlgProxyGroup.m_applyRule.m_szDnsAddrFilter = tbFilterDns.Text;
            // filter dns suffix
            m_dlgProxyGroup.m_applyRule.m_bDnsSuffixListFilter = cbFilterDnsSuffix.Checked;
            m_dlgProxyGroup.m_applyRule.m_szDnsSuffixListFilter = tbFilterDnsSuffix.Text;
        }

        #endregion

        #region Event Handlers to GUI Components

        private void dgvProxyItems_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            // init the default value for each data grid view row
            if (dgvProxyItems.Rows[e.RowIndex].Cells[IDX_IS_ENABLED].Value == null) {
                dgvProxyItems.Rows[e.RowIndex].Cells[IDX_IS_ENABLED].Value = false;
            }
            if (dgvProxyItems.Rows[e.RowIndex].Cells[IDX_PROXY_ADDR].Value == null) {
                dgvProxyItems.Rows[e.RowIndex].Cells[IDX_PROXY_ADDR].Value = String.Empty;
            }
            if (dgvProxyItems.Rows[e.RowIndex].Cells[IDX_BYPASS].Value == null) {
                dgvProxyItems.Rows[e.RowIndex].Cells[IDX_BYPASS].Value = String.Empty;
            }
        }

        private void dgvProxyItems_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (!dgvProxyItems.IsCurrentRowDirty) {
                return;
            }
            if (e.ColumnIndex == IDX_IS_ENABLED) {
                return;
            }

            DataGridViewCell cell = dgvProxyItems.Rows[e.RowIndex].Cells[e.ColumnIndex];
            var str = cell.Value as string;
            str = str.Trim().ToLower();

            switch (e.ColumnIndex) {
            case IDX_PROXY_ADDR:
                if (str.StartsWith("http://")) {
                    str = str.Remove(0, 7);
                }
                str = str.TrimEnd('/');
                break;
            case IDX_BYPASS:
                str = str.TrimEnd(';');
                break;
            }
            cell.Value = str;
        }

        private void cbFilterId_CheckedChanged(object sender, EventArgs e)
        {
            m_dlgInstance.tbFilterId.Enabled = m_dlgInstance.cbFilterId.Checked;
        }

        private void cbFilterName_CheckedChanged(object sender, EventArgs e)
        {
            m_dlgInstance.tbFilterName.Enabled = m_dlgInstance.cbFilterName.Checked;
        }

        private void cbFilterIpAddr_CheckedChanged(object sender, EventArgs e)
        {
            m_dlgInstance.tbFilterIpAddr.Enabled = m_dlgInstance.cbFilterIpAddr.Checked;
        }

        private void cbFilterMask_CheckedChanged(object sender, EventArgs e)
        {
            m_dlgInstance.tbFilterMask.Enabled = m_dlgInstance.cbFilterMask.Checked;
        }

        private void cbFilterGateway_CheckedChanged(object sender, EventArgs e)
        {
            m_dlgInstance.tbFilterGateway.Enabled = m_dlgInstance.cbFilterGateway.Checked;
        }

        private void cbFilterDns_CheckedChanged(object sender, EventArgs e)
        {
            m_dlgInstance.tbFilterDns.Enabled = m_dlgInstance.cbFilterDns.Checked;
        }

        private void cbFilterDnsSuffix_CheckedChanged(object sender, EventArgs e)
        {
            m_dlgInstance.tbFilterDnsSuffix.Enabled = m_dlgInstance.cbFilterDnsSuffix.Checked;
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (HasDataGridViewRowSelected()) {
                int idx = dgvProxyItems.SelectedRows[0].Index;
                dgvProxyItems.Rows.Insert(idx, new DataGridViewRow());
                dgvProxyItems.Rows[idx].Selected = true;
                dgvProxyItems.CurrentCell = dgvProxyItems.Rows[idx].Cells[IDX_PROXY_ADDR];
                dgvProxyItems.Focus();
                dgvProxyItems.BeginEdit(false);
            } else {
                dgvProxyItems.Focus();
            }
        }

        private void btnDel_Click(object sender, EventArgs e)
        {
            if (HasDataGridViewRowSelected()) {
                if (dgvProxyItems.SelectedRows[0].IsNewRow) {
                    MessageBox.Show("The last row cannot be deleted.",
                        AppManager.ASSEMBLY_PRODUCT,
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);
                } else {
                    dgvProxyItems.Rows.Remove(dgvProxyItems.SelectedRows[0]);
                }
            }
            dgvProxyItems.Focus();
        }

        private void btnUp_Click(object sender, EventArgs e)
        {
            if (HasDataGridViewRowSelected()) {
                int idx = dgvProxyItems.SelectedRows[0].Index;
                if (dgvProxyItems.SelectedRows[0].IsNewRow) {
                    MessageBox.Show("The last row cannot be moved up.",
                        AppManager.ASSEMBLY_PRODUCT,
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);
                } else if (idx > 0) {
                    DataGridViewRow item = dgvProxyItems.SelectedRows[0];
                    dgvProxyItems.Rows.RemoveAt(idx);
                    dgvProxyItems.Rows.Insert(idx - 1, item);
                    item.Selected = true;
                }
                dgvProxyItems.CurrentCell = dgvProxyItems.SelectedRows[0].Cells[IDX_PROXY_ADDR];
            }
            dgvProxyItems.Focus();
        }

        private void btnDown_Click(object sender, EventArgs e)
        {
            if (HasDataGridViewRowSelected()) {
                int idx = dgvProxyItems.SelectedRows[0].Index;
                if (dgvProxyItems.SelectedRows[0].IsNewRow) {
                    MessageBox.Show("The last row cannot be moved down.",
                        AppManager.ASSEMBLY_PRODUCT,
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);
                } else if (idx < dgvProxyItems.Rows.Count - 2) {
                    DataGridViewRow item = dgvProxyItems.SelectedRows[0];
                    dgvProxyItems.Rows.RemoveAt(idx);
                    dgvProxyItems.Rows.Insert(idx + 1, item);
                    item.Selected = true;
                }
                dgvProxyItems.CurrentCell = dgvProxyItems.SelectedRows[0].Cells[IDX_PROXY_ADDR];
            }
            dgvProxyItems.Focus();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            m_bExitByOK = true;
        }

        private bool HasDataGridViewRowSelected()
        {
            if (dgvProxyItems.SelectedRows.Count <= 0) {
                MessageBox.Show("No row is selected.",
                    AppManager.ASSEMBLY_PRODUCT,
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                return false;
            } else {
                return true;
            }
        }

        #endregion
    }
}
