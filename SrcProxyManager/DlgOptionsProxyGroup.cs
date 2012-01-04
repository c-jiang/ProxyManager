using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ProxyManager
{
    public partial class DlgOptionsProxyGroup : Form
    {
        private static DlgOptionsProxyGroup m_dlgInstance = null;
        private static ProxyGroup m_dlgProxyGroup = null;

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
            SetDialogLayout(pg);
            return ShowDialog(owner);
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
                    dgvProxyItems.Rows[idx].Cells[0].Value = pi.m_isEnabled;
                    dgvProxyItems.Rows[idx].Cells[1].Value = pi.m_szProxyAddr;
                    dgvProxyItems.Rows[idx].Cells[2].Value = pi.m_szBypass;
                }
            }

            // proxy group apply rule
            if (m_dlgProxyGroup.m_applyRule == null) {
                m_dlgProxyGroup.m_applyRule = new ApplyRule();
            }
            // filter network adapter id
            m_dlgInstance.cbFilterId.Checked = m_dlgProxyGroup.m_applyRule.m_bIdFilter;
            m_dlgInstance.tbFilterId.Text = m_dlgProxyGroup.m_applyRule.m_szIdFilter;
            m_dlgInstance.tbFilterId.Enabled = m_dlgInstance.cbFilterId.Checked;
            // filter network adapter name
            m_dlgInstance.cbFilterName.Checked = m_dlgProxyGroup.m_applyRule.m_bNameFilter;
            m_dlgInstance.tbFilterName.Text = m_dlgProxyGroup.m_applyRule.m_szNameFilter;
            m_dlgInstance.tbFilterName.Enabled = m_dlgInstance.cbFilterName.Checked;
            // filter ip addr
            m_dlgInstance.cbFilterIpAddr.Checked = m_dlgProxyGroup.m_applyRule.m_bIpAddrFilter;
            m_dlgInstance.tbFilterIpAddr.Text = m_dlgProxyGroup.m_applyRule.m_szIpAddrFilter;
            m_dlgInstance.tbFilterIpAddr.Enabled = m_dlgInstance.cbFilterIpAddr.Checked;
            // filter subnet mask
            m_dlgInstance.cbFilterMask.Checked = m_dlgProxyGroup.m_applyRule.m_bSubMaskFilter;
            m_dlgInstance.tbFilterMask.Text = m_dlgProxyGroup.m_applyRule.m_szSubMaskFilter;
            m_dlgInstance.tbFilterMask.Enabled = m_dlgInstance.cbFilterMask.Checked;
            // filter gateway
            m_dlgInstance.cbFilterGateway.Checked = m_dlgProxyGroup.m_applyRule.m_bGatewayFilter;
            m_dlgInstance.tbFilterGateway.Text = m_dlgProxyGroup.m_applyRule.m_szGatewayFilter;
            m_dlgInstance.tbFilterGateway.Enabled = m_dlgInstance.cbFilterGateway.Checked;
            // filter dns addr
            m_dlgInstance.cbFilterDns.Checked = m_dlgProxyGroup.m_applyRule.m_bDnsAddrFilter;
            m_dlgInstance.tbFilterDns.Text = m_dlgProxyGroup.m_applyRule.m_szDnsAddrFilter;
            m_dlgInstance.tbFilterDns.Enabled = m_dlgInstance.cbFilterDns.Checked;
            // filter dns suffix
            m_dlgInstance.cbFilterDnsSuffix.Checked = m_dlgProxyGroup.m_applyRule.m_bDnsSuffixFilter;
            m_dlgInstance.tbFilterDnsSuffix.Text = m_dlgProxyGroup.m_applyRule.m_szDnsSuffixFilter;
            m_dlgInstance.tbFilterDnsSuffix.Enabled = m_dlgInstance.cbFilterDnsSuffix.Checked;
        }

        private void DlgOptionsProxyGroup_FormClosing(object sender, FormClosingEventArgs e)
        {
            // proxy group name
            m_dlgProxyGroup.m_szName = tbName.Text.Trim();
            // proxy group enable/disable
            m_dlgProxyGroup.m_isEnabled = cbEnable.Checked;

            // proxy group items
            if (dgvProxyItems.RowCount <= 0) {
                m_dlgProxyGroup.m_listProxyItems = null;
            } else {
                m_dlgProxyGroup.m_listProxyItems.Clear();
                foreach (DataGridViewRow row in dgvProxyItems.Rows) {
                    if (row.IsNewRow) {
                        continue;
                    }
                    m_dlgProxyGroup.m_listProxyItems.Add(new ProxyItem(
                        Boolean.Parse(row.Cells[0].Value.ToString()),
                        row.Cells[1].Value.ToString().Trim(),
                        row.Cells[2].Value.ToString().Trim()));
                }
            }

            // proxy group apply rule
            if (m_dlgProxyGroup.m_applyRule == null) {
                m_dlgProxyGroup.m_applyRule = new ApplyRule();
            }
            // filter network adapter id
            m_dlgProxyGroup.m_applyRule.m_bIdFilter = m_dlgInstance.cbFilterId.Checked;
            m_dlgProxyGroup.m_applyRule.m_szIdFilter = m_dlgInstance.tbFilterId.Text.Trim();
            // filter network adapter name
            m_dlgProxyGroup.m_applyRule.m_bNameFilter = m_dlgInstance.cbFilterName.Checked;
            m_dlgProxyGroup.m_applyRule.m_szNameFilter = m_dlgInstance.tbFilterName.Text.Trim();
            // filter ip addr
            m_dlgProxyGroup.m_applyRule.m_bIpAddrFilter = m_dlgInstance.cbFilterIpAddr.Checked;
            m_dlgProxyGroup.m_applyRule.m_szIpAddrFilter = m_dlgInstance.tbFilterIpAddr.Text.Trim();
            // filter subnet mask
            m_dlgProxyGroup.m_applyRule.m_bSubMaskFilter = m_dlgInstance.cbFilterMask.Checked;
            m_dlgProxyGroup.m_applyRule.m_szSubMaskFilter = m_dlgInstance.tbFilterMask.Text.Trim();
            // filter gateway
            m_dlgProxyGroup.m_applyRule.m_bGatewayFilter = m_dlgInstance.cbFilterGateway.Checked;
            m_dlgProxyGroup.m_applyRule.m_szGatewayFilter = m_dlgInstance.tbFilterGateway.Text.Trim();
            // filter dns addr
            m_dlgProxyGroup.m_applyRule.m_bDnsAddrFilter = m_dlgInstance.cbFilterDns.Checked;
            m_dlgProxyGroup.m_applyRule.m_szDnsAddrFilter = m_dlgInstance.tbFilterDns.Text.Trim();
            // filter dns suffix
            m_dlgProxyGroup.m_applyRule.m_bDnsSuffixFilter = m_dlgInstance.cbFilterDnsSuffix.Checked;
            m_dlgProxyGroup.m_applyRule.m_szDnsSuffixFilter = m_dlgInstance.tbFilterDnsSuffix.Text.Trim();
        }

        #endregion

        #region Event Handlers to GUI Components

        private void dgvProxyItems_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            // init the default value for each data grid view row
            dgvProxyItems.Rows[e.RowIndex].Cells[0].Value = false;
            dgvProxyItems.Rows[e.RowIndex].Cells[1].Value = String.Empty;
            dgvProxyItems.Rows[e.RowIndex].Cells[2].Value = String.Empty;
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

        #endregion
    }
}
