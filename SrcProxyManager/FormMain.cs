using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Reflection;
using System.Text;
using System.Windows.Forms;


namespace ProxyManager
{
    public partial class FormMain : Form
    {
        public FormMain(AppManager appManager)
        {
            // init member variables
            m_appManagerRef = appManager;
            m_prevState = FormWindowState.Normal;
            m_listModeMenuItems = new List<MenuItem>();
            m_miNiCtxNetworkStatus = null;
            m_miNiCtxIPAddress = null;
            m_idxNiCtxProxySelection = 0;

            m_appManagerRef.NotifyGuiNetworkChanged +=
                new AppManager.NotifyNetworkChanged(
                    this.AppMgrNotify_NetworkChanged);

            // init GUI components
            InitializeComponent();
            this.Text = AssemblyProduct;
            aboutToolStripMenuItem.Text = "&About " + AssemblyProduct;
            InitGuiNotifyIcon();

            // set GUI properties according to profile
            if (m_appManagerRef.AppProfile.m_isStartMinimized) {
                this.ShowInTaskbar = false;
                this.WindowState = FormWindowState.Minimized;
            }

            // start current work mode
            m_appManagerRef.ApplyProfileItemWorkMode();

            // set registry key according to profile
            m_appManagerRef.ApplyProfileItemAutoStart();
        }

        private void InitGuiNotifyIcon()
        {
            // init NotifyIcon
            notifyIcon.ContextMenu = new ContextMenu();
            notifyIcon.Visible = true;

            // init NotifyIconBalloon
            notifyIcon.BalloonTipIcon = ToolTipIcon.Info;
            notifyIcon.BalloonTipTitle = AssemblyProduct;
            notifyIcon.BalloonTipText = " ";

            // init NotifyIconMenuContext
            m_arrayMenuItems = new MenuItem[15];
            int idx = 0;

            m_arrayMenuItems[idx] = new MenuItem();
            m_arrayMenuItems[idx].Text = "Exit " + AssemblyProduct;
            m_arrayMenuItems[idx].Click += new System.EventHandler(UserRequest_ExitApplication);
            ++idx;
            m_arrayMenuItems[idx] = new MenuItem("-");
            ++idx;
            m_arrayMenuItems[idx] = new MenuItem();
            m_arrayMenuItems[idx].Text = "About " + AssemblyProduct;
            m_arrayMenuItems[idx].Click += new System.EventHandler(UserRequest_ShowDlgAbout);
            ++idx;
            m_arrayMenuItems[idx] = new MenuItem("-");
            ++idx;
            m_arrayMenuItems[idx] = new MenuItem();
            m_arrayMenuItems[idx].Text = "Options";
            m_arrayMenuItems[idx].Click += new System.EventHandler(UserRequest_ShowDlgOptions);
            ++idx;
            m_arrayMenuItems[idx] = new MenuItem("-");
            ++idx;
            m_arrayMenuItems[idx] = new MenuItem();         // network status
            m_arrayMenuItems[idx].Enabled = false;
            m_miNiCtxNetworkStatus = m_arrayMenuItems[idx];
            ++idx;
            m_arrayMenuItems[idx] = new MenuItem();         // ip address
            m_arrayMenuItems[idx].Enabled = false;
            m_miNiCtxIPAddress = m_arrayMenuItems[idx];
            ++idx;
            m_arrayMenuItems[idx] = new MenuItem("");       // proxy selection
            m_idxNiCtxProxySelection = idx;
            ++idx;
            m_arrayMenuItems[idx] = new MenuItem("-");
            ++idx;
            m_arrayMenuItems[idx] = new MenuItem();
            m_arrayMenuItems[idx].Text = "Auto Mode";
            m_arrayMenuItems[idx].Click += new System.EventHandler(UserRequest_SwitchToAutoMode);
            m_arrayMenuItems[idx].RadioCheck = true;
            m_arrayMenuItems[idx].Checked = false;
            m_listModeMenuItems.Add(m_arrayMenuItems[idx]); // [0] Auto Mode
            ++idx;
            m_arrayMenuItems[idx] = new MenuItem();
            m_arrayMenuItems[idx].Text = "Direct Mode";
            m_arrayMenuItems[idx].Click += new System.EventHandler(UserRequest_SwitchToDirectMode);
            m_arrayMenuItems[idx].RadioCheck = true;
            m_arrayMenuItems[idx].Checked = false;
            m_listModeMenuItems.Add(m_arrayMenuItems[idx]); // [1] Direct Mode
            ++idx;
            m_arrayMenuItems[idx] = new MenuItem();
            m_arrayMenuItems[idx].Text = "Proxy Mode";
            m_arrayMenuItems[idx].Click += new System.EventHandler(UserRequest_SwitchToProxyMode);
            m_arrayMenuItems[idx].RadioCheck = true;
            m_arrayMenuItems[idx].Checked = false;
            m_listModeMenuItems.Add(m_arrayMenuItems[idx]); // [2] Proxy Mode
            ++idx;
            m_arrayMenuItems[idx] = new MenuItem("-");
            ++idx;
            m_arrayMenuItems[idx] = new MenuItem();
            m_arrayMenuItems[idx].Text = "Open " + AssemblyProduct;
            m_arrayMenuItems[idx].Click += new System.EventHandler(UserRequest_ShowFormMain);
            m_arrayMenuItems[idx].DefaultItem = true;

            notifyIcon.ContextMenu = new ContextMenu(m_arrayMenuItems);
        }

        private void UpdateGui_FormMainLayout()
        {
            // update text box
            NetworkDetector nd = m_appManagerRef.Detector;
            string ui = "[" + Utils.GetDateTime() + "]"
                + Environment.NewLine + Environment.NewLine;
            ui += "Network Status: ";
            if (nd.IsNetworkActive()) {
                ui += "Active" + Environment.NewLine + Environment.NewLine;
                ui += "Network Adaptor ID . . : " + nd.ActiveNetworkId()
                    + Environment.NewLine;
                ui += "Network Adaptor Name . : " + nd.ActiveNetworkName()
                    + Environment.NewLine;
                ui += "Network Adaptor Desc . : " + nd.ActiveNetworkDescription()
                    + Environment.NewLine;
                ui += "IP Address . . . . . . : " + nd.ActiveNetworkIPAddress()
                    + Environment.NewLine;
                ui += "Subnet Mask. . . . . . : " + nd.ActiveNetworkSubMask()
                    + Environment.NewLine;
                ui += "Default Gateway. . . . : " + nd.ActiveNetworkGateway()
                    + Environment.NewLine;
                ui += "DNS Server . . . . . . : " + nd.ActiveNetworkDnsAddress()
                    + Environment.NewLine;
                ui += "DNS Suffix . . . . . . : " + nd.ActiveNetworkDnsSuffix()
                    + Environment.NewLine;
            } else {
                ui += "Inactive" + Environment.NewLine;
            }
            tbStatus.Text = ui;

            // update label text
            labelProxyAddr.Text = "Proxy Server: "
                + (IeProxyOptions.ProxyEnable ? IeProxyOptions.ProxyAddr : "Disabled");

            // update group box title
            gbWorkMode.Text = "Work Mode: "
                + m_appManagerRef.AppProfile.m_workMode
                + " Mode";

            // set focus
            tbStatus.Select(0, -1);
            btnRefresh.Focus();
        }

        private void UpdateGui_NotifyIconTextIndication()
        {
            string str = AssemblyProduct + " ("
                + m_appManagerRef.AppProfile.m_workMode + " Mode)"
                + Environment.NewLine;
            str += "Network Status: "
                + (m_appManagerRef.Detector.IsNetworkActive() ? "Active" : "Inactive");
            notifyIcon.Text = str;
        }

        private void UpdateGui_NotifyIconBalloonTip()
        {
            string tip = "Network Status: "
                + (m_appManagerRef.Detector.IsNetworkActive() ? "Active" : "Inactive")
                + Environment.NewLine;
            tip += "IP Address: " + (m_appManagerRef.Detector.IsNetworkActive()
                ? m_appManagerRef.Detector.ActiveNetworkIPAddress() : "N/A")
                + Environment.NewLine;
            tip += "Work Mode: "
                + m_appManagerRef.AppProfile.m_workMode.ToString() + " Mode"
                + Environment.NewLine;
            tip += "Proxy: " + (IeProxyOptions.ProxyEnable
                ? IeProxyOptions.ProxyAddr : "Disabled");

            notifyIcon.BalloonTipText = tip;
            notifyIcon.ShowBalloonTip(3000);
        }

        private void UpdateGui_NotifyIconMenuNetwork()
        {
            // change reference: network
            m_miNiCtxNetworkStatus.Text = "Network Status: "
                + (m_appManagerRef.Detector.IsNetworkActive()
                    ? "Active" : "Inactive");

            m_miNiCtxIPAddress.Text = "IP Address: "
                + (m_appManagerRef.Detector.IsNetworkActive()
                    ? m_appManagerRef.Detector.ActiveNetworkIPAddress()
                    : "N/A");
        }

        private void UpdateGui_NotifyIconMenuWorkMode()
        {
            // change reference: proxy
            foreach (MenuItem iter in m_listModeMenuItems) {
                iter.Checked = false;
            }
            switch (m_appManagerRef.AppProfile.m_workMode) {
            case WorkMode.Auto:
                m_listModeMenuItems[0].Checked = true;
                break;
            case WorkMode.Direct:
                m_listModeMenuItems[1].Checked = true;
                break;
            case WorkMode.Proxy:
                m_listModeMenuItems[2].Checked = true;
                break;
            }
        }

        private void UpdateGui_NotifyIconMenuProxySelection()
        {
            switch (m_appManagerRef.AppProfile.m_workMode) {
            case WorkMode.Auto: {
                    if (!IeProxyOptions.ProxyEnable) {
                        // fall through
                        goto case WorkMode.Direct;
                    }
                    ProxyGroup pg = m_appManagerRef.AppProfile.m_listProxyGroups[
                        m_appManagerRef.AutoModeProxyGroupIndex];
                    MenuItem[] mis = new MenuItem[pg.m_listProxyItems.Count];
                    for (int i = 0; i < pg.m_listProxyItems.Count; ++i) {
                        mis[i] = new MenuItem(pg.m_listProxyItems[i].m_szProxyAddr);
                        if (!pg.m_listProxyItems[i].m_isEnabled) {
                            mis[i].Enabled = false;
                        } else {
                            mis[i].Click += new System.EventHandler(
                                UserRequest_SelectProxyFromCurrentGroup);
                        }
                        mis[i].Checked = false;
                        mis[i].RadioCheck = true;
                    }
                    mis[pg.m_iSelectedIndex - 1].Checked = true;
                    m_arrayMenuItems[m_idxNiCtxProxySelection] = new MenuItem(
                        "Select Proxy from Group", mis);
                    notifyIcon.ContextMenu = new ContextMenu(m_arrayMenuItems);
                    break;
                }

            case WorkMode.Direct: {
                    m_arrayMenuItems[m_idxNiCtxProxySelection] = new MenuItem(
                        "Select Proxy: N/A");
                    m_arrayMenuItems[m_idxNiCtxProxySelection].Enabled = false;
                    notifyIcon.ContextMenu = new ContextMenu(m_arrayMenuItems);
                    break;
                }

            case WorkMode.Proxy: {
                    string proxyAddr = IeProxyOptions.ProxyAddr;
                    string bypass = IeProxyOptions.Bypass;
                    bool autoConfDisabled = IeProxyOptions.AutoConfDisabled;
                    bool found = false;

                    List<ProxyGroup> listPg = m_appManagerRef.AppProfile.m_listProxyGroups;
                    MenuItem[] misPg = new MenuItem[listPg.Count];

                    for (int i = 0; i < listPg.Count; ++i) {
                        List<ProxyItem> listPi = listPg[i].m_listProxyItems;
                        MenuItem[] mis = new MenuItem[listPi.Count];
                        for (int j = 0; j < listPi.Count; ++j) {
                            mis[j] = new MenuItem(listPi[j].m_szProxyAddr,
                                UserRequest_SelectProxyFromPool);
                            mis[j].Checked = false;
                            mis[j].RadioCheck = true;

                            if (!found &&
                                listPi[j].m_szProxyAddr.Equals(proxyAddr) &&
                                listPi[j].m_szBypass.Equals(bypass) &&
                                listPi[j].m_isAutoConfDisabled.Equals(autoConfDisabled)) {
                                mis[j].Checked = true;
                                found = true;
                            }
                        }
                        misPg[i] = new MenuItem(listPg[i].m_szName, mis);
                    }
                    m_arrayMenuItems[m_idxNiCtxProxySelection] = new MenuItem(
                        "Select Proxy from Pool", misPg);
                    notifyIcon.ContextMenu = new ContextMenu(m_arrayMenuItems);
                    break;
                }
            }
        }

        #region AppManager Notifications

        public void AppMgrNotify_NetworkChanged(object sender, EventArgs e)
        {
            UpdateGui_FormMainLayout();
            UpdateGui_NotifyIconTextIndication();
            UpdateGui_NotifyIconMenuNetwork();
            UpdateGui_NotifyIconMenuWorkMode();
            UpdateGui_NotifyIconMenuProxySelection();
            if (WindowState == FormWindowState.Minimized) {
                UpdateGui_NotifyIconBalloonTip();
            }
        }

        #endregion

        #region User Notifications

        private void UserRequest_SwitchToAutoMode(object sender, EventArgs e)
        {
            m_appManagerRef.ChangeProfileWorkMode(WorkMode.Auto);
        }

        private void UserRequest_SwitchToDirectMode(object sender, EventArgs e)
        {
            m_appManagerRef.ChangeProfileWorkMode(WorkMode.Direct);
        }

        private void UserRequest_SwitchToProxyMode(object sender, EventArgs e)
        {
            m_appManagerRef.ChangeProfileWorkMode(WorkMode.Proxy);
        }

        private void UserRequest_ShowFormMain(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Minimized) {
                this.Show();                    // step 1 - show
                this.ShowInTaskbar = true;      // step 2 - show
                this.WindowState = m_prevState; // step 3 - show
            } else {
                this.Activate();
            }
        }

        private void UserRequest_SelectProxyFromCurrentGroup(object sender, EventArgs e)
        {
            var mi = sender as MenuItem;
            ProxyGroup pg = m_appManagerRef.AppProfile.m_listProxyGroups[
                m_appManagerRef.AutoModeProxyGroupIndex];
            pg.m_iSelectedIndex = mi.Index + 1;
            m_appManagerRef.EnableProxy(
                pg.m_listProxyItems[pg.m_iSelectedIndex - 1]);
        }

        private void UserRequest_SelectProxyFromPool(object sender, EventArgs e)
        {
            var mi = sender as MenuItem;
            int idx = mi.Index;

            foreach (MenuItem iter in m_arrayMenuItems[m_idxNiCtxProxySelection].MenuItems) {
                foreach (MenuItem i in iter.MenuItems) {
                    i.Checked = false;
                }
            }

            foreach (MenuItem iter in m_arrayMenuItems[m_idxNiCtxProxySelection].MenuItems) {
                if ((idx < iter.MenuItems.Count) &&
                    (iter.MenuItems[idx].GetHashCode() == mi.GetHashCode())) {
                    // menu item entry found
                    m_appManagerRef.EnableProxy(m_appManagerRef.AppProfile
                        .m_listProxyGroups[iter.Index].m_listProxyItems[idx]);
                    mi.Checked = true;
                    break;
                }
            }
        }

        private void UserRequest_ExitApplication(object sender, EventArgs e)
        {
            this.Close();
        }

        private void UserRequest_ShowDlgAbout(object sender, EventArgs e)
        {
            if (!DlgAboutBox.Instance.Visible) {
                DlgAboutBox.Instance.StartPosition =
                    (this.WindowState == FormWindowState.Minimized)
                    ? FormStartPosition.CenterScreen
                    : FormStartPosition.CenterParent;
                DlgAboutBox.Instance.ShowDialog(this);
            } else {
                DlgAboutBox.Instance.Activate();
            }
        }

        private void UserRequest_ShowDlgOptions(object sender, EventArgs e)
        {
            // TODO: Add the entry to Options dialog.
            MessageBox.Show(
                @"The entry to Options is not implemented.",
                AssemblyProduct,
                MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        #endregion

        #region Event Handlers to GUI Components

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            UpdateGui_FormMainLayout();
        }

        private void btnAutoMode_Click(object sender, EventArgs e)
        {
            UserRequest_SwitchToAutoMode(sender, e);
        }

        private void btnDirectMode_Click(object sender, EventArgs e)
        {
            UserRequest_SwitchToDirectMode(sender, e);
        }

        private void btnProxyMode_Click(object sender, EventArgs e)
        {
            UserRequest_SwitchToProxyMode(sender, e);
        }

        private void minimizeToTrayToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.ShowInTaskbar = false;                     // step 1 - hide
            this.WindowState = FormWindowState.Minimized;   // step 2 - hide
            this.Hide();                                    // step 3 - hide
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            UserRequest_ExitApplication(sender, e);
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            UserRequest_ShowDlgAbout(sender, e);
        }

        private void notifyIcon_Click(object sender, EventArgs e)
        {
            var evt = e as MouseEventArgs;
            if (evt.Button == MouseButtons.Left) {
                // trigger the show/hide
                switch (this.WindowState) {
                case FormWindowState.Minimized:
                    this.Show();                    // step 1 - show
                    this.ShowInTaskbar = true;      // step 2 - show
                    this.WindowState = m_prevState; // step 3 - show
                    break;
                case FormWindowState.Maximized:
                case FormWindowState.Normal:
                    this.ShowInTaskbar = false;     // step 1 - hide
                    this.WindowState = FormWindowState.Minimized;   // step 2 - hide
                    break;
                }
            }
        }

        private void FormMain_Resize(object sender, EventArgs e)
        {
            switch (this.WindowState) {
            case FormWindowState.Minimized:
                this.Hide();                        // step 3 - hide
                break;
            case FormWindowState.Maximized:
            case FormWindowState.Normal:
                m_prevState = this.WindowState;
                break;
            }
        }

        #endregion

        #region Assembly Attribute Accessors

        public string AssemblyProduct
        {
            get
            {
                object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyProductAttribute), false);
                if (attributes.Length == 0) {
                    return "Proxy Manager";
                }
                return ((AssemblyProductAttribute)attributes[0]).Product;
            }
        }

        #endregion

        
        private AppManager m_appManagerRef;
        private FormWindowState m_prevState;

        private MenuItem[] m_arrayMenuItems;
        private List<MenuItem> m_listModeMenuItems;
        private MenuItem m_miNiCtxNetworkStatus;
        private MenuItem m_miNiCtxIPAddress;
        private int m_idxNiCtxProxySelection;
    }
}
