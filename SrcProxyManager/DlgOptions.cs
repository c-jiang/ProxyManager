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
            SetDialogLayout(profile);
            return ShowDialog();
        }

        public DialogResult ShowDialog(IWin32Window owner, Profile profile)
        {
            SetDialogLayout(profile);
            return ShowDialog(owner);
        }

        public static Profile DlgProfile
        {
            get { return m_dlgProfile; }
        }

        private DlgOptions()
        {
            InitializeComponent();

            ColumnHeader cb;
            cb = new ColumnHeader();
            cb.Text = "Proxy Group Name";
            cb.Width = 120;
            lvProxyGroups.Columns.Add(cb);
            cb = new ColumnHeader();
            cb.Text = "Status";
            cb.Width = 60;
            lvProxyGroups.Columns.Add(cb);
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

            // proxy group
            lvProxyGroups.Items.Clear();
            if (m_dlgProfile.m_listProxyGroups != null) {
                foreach (ProxyGroup pg in m_dlgProfile.m_listProxyGroups) {
                    ListViewItem lvi = new ListViewItem();
                    lvi.Text = pg.m_szName;
                    lvi.SubItems.Add(new ListViewItem.ListViewSubItem(lvi,
                        (pg.m_isEnabled ? "Enable" : "Disable")));
                    lvi.SubItems.Add(new ListViewItem.ListViewSubItem(lvi,
                        (pg.m_listProxyItems == null
                            ? "0" : pg.m_listProxyItems.Count.ToString())));
                    lvProxyGroups.Items.Add(lvi);
                }
            }
        }

        private void DlgOptions_FormClosing(object sender, FormClosingEventArgs e)
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

            // proxy group
            // TODO:
        }

        #endregion

        #region Event Handlers to GUI Components

        private void lvProxyGroups_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            // TODO: entry the dialog for editing Proxy Group
        }

        private void btnNew_Click(object sender, EventArgs e)
        {
            // TODO: create a new Proxy Group
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            // TODO: edit current Proxy Group
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            // TODO: delete current Proxy Group
        }

        private void btnUp_Click(object sender, EventArgs e)
        {
            // TODO: move current Proxy Group up
        }

        private void btnDown_Click(object sender, EventArgs e)
        {
            // TODO: move current Proxy Group down
        }

        #endregion
    }
}
