using System;
using System.Windows.Forms;

namespace ProxyManager
{
    public partial class DlgOptions : Form
    {
        private static DlgOptions m_dlgInstance = null;
        private static Profile m_dlgProfile = null;

        public static DlgOptions Instance
        {
            get
            {
                if (m_dlgInstance == null) {
                    m_dlgInstance = new DlgOptions();
                }
                return m_dlgInstance;
            }
        }

        public DialogResult ShowDialog(Profile profile)
        {
            Logger.V(">> DlgOptions.ShowDialog");
            SetDialogLayout(profile);
            DialogResult dr = ShowDialog();
            Logger.V("<< DlgOptions.ShowDialog : " + dr.ToString());
            return dr;
        }

        public DialogResult ShowDialog(IWin32Window owner, Profile profile)
        {
            Logger.V(">> DlgOptions.ShowDialog");
            SetDialogLayout(profile);
            DialogResult dr = ShowDialog(owner);
            Logger.V("<< DlgOptions.ShowDialog : " + dr.ToString());
            return dr;
        }

        public static Profile DlgProfile
        {
            get { return m_dlgProfile; }
        }

        private DlgOptions()
        {
            InitializeComponent();

            ColumnHeader cb;
            // proxy group name
            cb = new ColumnHeader();
            cb.Text = "Proxy Group Name";
            cb.Width = 120;
            lvProxyGroups.Columns.Add(cb);
            // status: enable/disable
            cb = new ColumnHeader();
            cb.Text = "Status";
            cb.Width = 60;
            cb.TextAlign = HorizontalAlignment.Center;
            lvProxyGroups.Columns.Add(cb);
            // item count
            cb = new ColumnHeader();
            cb.Text = "Item #";
            cb.Width = 50;
            cb.TextAlign = HorizontalAlignment.Right;
            lvProxyGroups.Columns.Add(cb);
        }

        #region Sync dialog layout and profile data

        private void SetDialogLayout(Profile profile)
        {
            m_dlgProfile = new Profile(profile);

            // default work mode
            m_dlgInstance.rbAuto.Checked = false;
            m_dlgInstance.rbProxy.Checked = false;
            m_dlgInstance.rbDirect.Checked = false;

            switch (m_dlgProfile.m_defWorkMode) {
            case WorkMode.Auto:
                m_dlgInstance.rbAuto.Checked = true;
                break;
            case WorkMode.Direct:
                m_dlgInstance.rbDirect.Checked = true;
                break;
            case WorkMode.Proxy:
                m_dlgInstance.rbProxy.Checked = true;
                break;
            }

            // start auto
            m_dlgInstance.cbStartAuto.Checked = m_dlgProfile.m_isStartAuto;

            // start minimized
            m_dlgInstance.cbStartMinimized.Checked = m_dlgProfile.m_isStartMinimized;

            // log to file
            m_dlgInstance.cbLogToFile.Checked = m_dlgProfile.m_isLogToFile;

            // log level
            m_dlgInstance.cbLogLevel.Enabled = m_dlgInstance.cbLogToFile.Checked;
            m_dlgInstance.cbLogLevel.Items.Add(Logger.Category.Error);
            m_dlgInstance.cbLogLevel.Items.Add(Logger.Category.Warning);
            m_dlgInstance.cbLogLevel.Items.Add(Logger.Category.Information);
            m_dlgInstance.cbLogLevel.Items.Add(Logger.Category.Verbose);
            if (profile.m_logLevel != Logger.Category.NONE) {
                m_dlgInstance.cbLogLevel.Text = profile.m_logLevel.ToString();
            } else {
                // default, for forbidding NONE category
                m_dlgInstance.cbLogLevel.Text = Logger.Category.Information.ToString();
            }

            // proxy group
            lvProxyGroups.Items.Clear();
            foreach (ProxyGroup pg in m_dlgProfile.m_listProxyGroups) {
                ListViewItem item = CreateListViewItem(pg);
                lvProxyGroups.Items.Add(item);
            }
        }

        private void DlgOptions_Shown(object sender, EventArgs e)
        {
            btnOK.Focus();
        }

        private void DlgOptions_FormClosed(object sender, FormClosedEventArgs e)
        {
            // default work mode
            if (m_dlgInstance.rbAuto.Checked) {
                m_dlgProfile.m_defWorkMode = WorkMode.Auto;
            } else if (m_dlgInstance.rbDirect.Checked) {
                m_dlgProfile.m_defWorkMode = WorkMode.Direct;
            } else {
                m_dlgProfile.m_defWorkMode = WorkMode.Proxy;
            }
            // start auto
            m_dlgProfile.m_isStartAuto = m_dlgInstance.cbStartAuto.Checked;
            // start minimized
            m_dlgProfile.m_isStartMinimized = m_dlgInstance.cbStartMinimized.Checked;
            // log to file
            m_dlgProfile.m_isLogToFile = m_dlgInstance.cbLogToFile.Checked;
            // log level
            if (m_dlgInstance.cbLogLevel.Text.Equals(Logger.Category.Error.ToString())) {
                m_dlgProfile.m_logLevel = Logger.Category.Error;
            } else if (m_dlgInstance.cbLogLevel.Text.Equals(Logger.Category.Warning.ToString())) {
                m_dlgProfile.m_logLevel = Logger.Category.Warning;
            } else if (m_dlgInstance.cbLogLevel.Text.Equals(Logger.Category.Verbose.ToString())) {
                m_dlgProfile.m_logLevel = Logger.Category.Verbose;
            } else {
                // default, for forbidding NONE category
                m_dlgProfile.m_logLevel = Logger.Category.Information;
            }
        }

        #endregion

        #region Event Handlers to GUI Components

        private void lvProxyGroups_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left) {
                return;
            }
            EditProxyGroup(lvProxyGroups.FocusedItem);
        }

        private void cbLogToFile_CheckedChanged(object sender, EventArgs e)
        {
            cbLogLevel.Enabled = cbLogToFile.Checked;
        }

        private void btnNew_Click(object sender, EventArgs e)
        {
            DialogResult dr = DlgOptionsProxyGroup.Instance.ShowDialog(
                this, new ProxyGroup());
            if (dr == DialogResult.OK) {
                ProxyGroup pg = new ProxyGroup(DlgOptionsProxyGroup.Instance.DlgProxyGroup);
                ListViewItem item = CreateListViewItem(pg);
                if (lvProxyGroups.SelectedItems.Count <= 0) {
                    // add to the tail
                    m_dlgProfile.m_listProxyGroups.Add(pg);
                    lvProxyGroups.Items.Add(item);
                } else {
                    // insert at current selected item
                    int idx = lvProxyGroups.SelectedItems[0].Index;
                    m_dlgProfile.m_listProxyGroups.Insert(idx, pg);
                    lvProxyGroups.Items.Insert(idx, item);
                }
                item.Selected = true;
            }
            lvProxyGroups.Focus();
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (HasListViewItemSelected()) {
                EditProxyGroup(lvProxyGroups.SelectedItems[0]);
            }
            lvProxyGroups.Focus();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (HasListViewItemSelected()) {
                DialogResult dr = MessageBox.Show(
                    "Are you sure to delete the selected Proxy Group?",
                    AppManager.ASSEMBLY_PRODUCT,
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);
                if (dr == DialogResult.Yes) {
                    int idx = lvProxyGroups.SelectedItems[0].Index;
                    DeleteProxyGroup(lvProxyGroups.SelectedItems[0]);
                    if (idx >= lvProxyGroups.Items.Count) {
                        idx = lvProxyGroups.Items.Count - 1;
                    }
                    if (idx >= 0) {
                        lvProxyGroups.Items[idx].Selected = true;
                    }
                }
            }
            lvProxyGroups.Focus();
        }

        private void btnUp_Click(object sender, EventArgs e)
        {
            if (HasListViewItemSelected()) {
                int idx = lvProxyGroups.SelectedItems[0].Index;
                if (idx > 0) {
                    ProxyGroup pg = m_dlgProfile.m_listProxyGroups[idx];
                    m_dlgProfile.m_listProxyGroups.RemoveAt(idx);
                    m_dlgProfile.m_listProxyGroups.Insert(idx - 1, pg);

                    ListViewItem item = lvProxyGroups.Items[idx];
                    lvProxyGroups.Items.RemoveAt(idx);
                    lvProxyGroups.Items.Insert(idx - 1, item);
                }
            }
            lvProxyGroups.Focus();
        }

        private void btnDown_Click(object sender, EventArgs e)
        {
            if (HasListViewItemSelected()) {
                int idx = lvProxyGroups.SelectedItems[0].Index;
                if (idx < lvProxyGroups.Items.Count - 1) {
                    ProxyGroup pg = m_dlgProfile.m_listProxyGroups[idx];
                    m_dlgProfile.m_listProxyGroups.RemoveAt(idx);
                    m_dlgProfile.m_listProxyGroups.Insert(idx + 1, pg);

                    ListViewItem item = lvProxyGroups.Items[idx];
                    lvProxyGroups.Items.RemoveAt(idx);
                    lvProxyGroups.Items.Insert(idx + 1, item);
                }
            }
            lvProxyGroups.Focus();
        }

        private void EditProxyGroup(ListViewItem item)
        {
            ProxyGroup pg = m_dlgProfile.m_listProxyGroups[item.Index];
            DialogResult dr = DlgOptionsProxyGroup.Instance.ShowDialog(this, pg);
            if ((dr == DialogResult.OK) && (!pg.Equals(DlgOptionsProxyGroup.Instance.DlgProxyGroup))) {
                m_dlgProfile.m_listProxyGroups[item.Index] = new ProxyGroup(
                    DlgOptionsProxyGroup.Instance.DlgProxyGroup);
                pg = m_dlgProfile.m_listProxyGroups[item.Index];
                item.SubItems[0].Text = pg.m_szName;
                item.SubItems[1].Text = (pg.m_isEnabled ? "Enable" : "Disable");
                item.SubItems[2].Text = (pg.m_listProxyItems == null
                    ? "0" : pg.m_listProxyItems.Count.ToString());
            }
        }

        private void DeleteProxyGroup(ListViewItem item)
        {
            m_dlgProfile.m_listProxyGroups.RemoveAt(item.Index);
            lvProxyGroups.Items.RemoveAt(item.Index);
        }

        private ListViewItem CreateListViewItem(ProxyGroup pg)
        {
            ListViewItem item = new ListViewItem();
            item.Text = pg.m_szName;
            item.SubItems.Add(new ListViewItem.ListViewSubItem(item,
                (pg.m_isEnabled ? "Enable" : "Disable")));
            item.SubItems.Add(new ListViewItem.ListViewSubItem(item,
                    (pg.m_listProxyItems == null
                        ? "0" : pg.m_listProxyItems.Count.ToString())));
            return item;
        }

        private bool HasListViewItemSelected()
        {
            if (lvProxyGroups.SelectedItems.Count <= 0) {
                MessageBox.Show("No Proxy Group is selected.",
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
