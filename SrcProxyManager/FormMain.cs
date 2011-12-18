using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
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

            // start current work mode accordingly
            switch (m_appManagerRef.AppProfile.m_workMode) {
            case WorkMode.Auto:
                m_appManagerRef.AutoSwitchProxy();
                break;
            case WorkMode.Direct:
                m_appManagerRef.DisableProxy();
                break;
            case WorkMode.Proxy:
                m_appManagerRef.EnableProxy();
                break;
            }

            // set registry key according to profile
            m_appManagerRef.ApplyProfileItemAutoStart();
        }

        private void InitGuiNotifyIcon()
        {
            // Init NotifyIcon
            notifyIcon.ContextMenu = new ContextMenu();
            notifyIcon.Visible = true;

            // Init NotifyIconBalloon
            notifyIcon.BalloonTipIcon = ToolTipIcon.Info;
            notifyIcon.BalloonTipTitle = AssemblyProduct;
            notifyIcon.BalloonTipText = " ";

            // Init NotifyIconMenuContext
            MenuItem[] mis = new MenuItem[14];
            int idx = 0;

            mis[idx] = new MenuItem();
            mis[idx].Text = "Exit " + AssemblyProduct;
            mis[idx].Click += new System.EventHandler(UserRequest_ExitApplication);
            ++idx;
            mis[idx] = new MenuItem("-");
            ++idx;
            mis[idx] = new MenuItem();
            mis[idx].Text = "About " + AssemblyProduct;
            mis[idx].Click += new System.EventHandler(UserRequest_ShowDlgAbout);
            ++idx;
            mis[idx] = new MenuItem("-");
            ++idx;
            mis[idx] = new MenuItem();
            mis[idx].Text = "Options";
            mis[idx].Click += new System.EventHandler(UserRequest_ShowDlgOptions);
            ++idx;
            mis[idx] = new MenuItem("-");
            ++idx;
            mis[idx] = new MenuItem();          // network status
            mis[idx].Enabled = false;
            m_miNiCtxNetworkStatus = mis[idx];
            ++idx;
            mis[idx] = new MenuItem();          // ip address
            mis[idx].Enabled = false;
            m_miNiCtxIPAddress = mis[idx];
            ++idx;
            mis[idx] = new MenuItem("-");
            ++idx;
            mis[idx] = new MenuItem();
            mis[idx].Text = "Auto Mode";
            mis[idx].Click += new System.EventHandler(UserRequest_SwitchToAutoMode);
            mis[idx].RadioCheck = true;
            mis[idx].Checked = false;
            m_listModeMenuItems.Add(mis[idx]);  // [0] Auto Mode
            ++idx;
            mis[idx] = new MenuItem();
            mis[idx].Text = "Direct Mode";
            mis[idx].Click += new System.EventHandler(UserRequest_SwitchToDirectMode);
            mis[idx].RadioCheck = true;
            mis[idx].Checked = false;
            m_listModeMenuItems.Add(mis[idx]);  // [1] Direct Mode
            ++idx;
            mis[idx] = new MenuItem();
            mis[idx].Text = "Proxy Mode";
            mis[idx].Click += new System.EventHandler(UserRequest_SwitchToProxyMode);
            mis[idx].RadioCheck = true;
            mis[idx].Checked = false;
            m_listModeMenuItems.Add(mis[idx]);  // [2] Proxy Mode
            ++idx;
            mis[idx] = new MenuItem("-");
            ++idx;
            mis[idx] = new MenuItem();
            mis[idx].Text = "Open " + AssemblyProduct;
            mis[idx].Click += new System.EventHandler(UserRequest_ShowFormMain);
            mis[idx].DefaultItem = true;

            notifyIcon.ContextMenu = new ContextMenu(mis);

            UpdateGui_NotifyIconMenuNetwork();
            UpdateGui_NotifyIconTextIndication();
        }

        private void UpdateGui_TextBoxMainContent()
        {
            // change reference: network
            NetworkDetector nd = m_appManagerRef.Detector;
            string ui = "[" + Utils.GetDateTime() + "] ";
            if (nd.IsNetworkActive()) {
                ui += "Network Active" + "\r\n";
                ui += "\r\n";
                ui += "IP . . . . . . . : " + nd.ActiveNetworkIPAddress() + "\r\n";
                ui += "Mask . . . . . . : " + nd.ActiveNetworkSubMask() + "\r\n";
                ui += "Gateway. . . . . : " + nd.ActiveNetworkGateway() + "\r\n";
                ui += "DNS. . . . . . . : " + nd.ActiveNetworkDnsAddress() + "\r\n";
                ui += "DNS Suffix . . . : " + nd.ActiveNetworkDnsSuffix() + "\r\n";
                ui += "\r\n";
            } else {
                ui += "Network Inactive" + "\r\n";
                ui += "\r\n";
            }
            // TODO: remove proxy info from this text box
            ui += "Proxy Stat: ";
            ui += (IeProxyOptions.ProxyEnable ? "Enable" : "Disable");
            ui += "\r\n";
            ui += "Proxy Addr: " + IeProxyOptions.ProxyAddr;
            ui += "\r\n";
            ui += "Bypass: " + IeProxyOptions.Bypass;
            ui += "\r\n";
            tbStatus.Text = ui;
        }

        private void UpdateGui_GroupBoxTitle()
        {
            // change reference: proxy
            gbWorkMode.Text = "Current Work Mode: "
                + m_appManagerRef.AppProfile.m_workMode
                + " Mode";
        }

        private void UpdateGui_NotifyIconTextIndication()
        {
            // change reference: network, proxy
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
                 + (m_appManagerRef.Detector.IsNetworkActive() ? "Active" : "Inactive");
            tip += Environment.NewLine;
            if (m_appManagerRef.Detector.IsNetworkActive()) {
                tip += "IP Address: "
                    + m_appManagerRef.Detector.ActiveNetworkIPAddress();
                tip += Environment.NewLine;
            }
            tip += "Work Mode: "
                + m_appManagerRef.AppProfile.m_workMode.ToString();
            if (IeProxyOptions.ProxyEnable) {
                tip += Environment.NewLine;
                tip += "Proxy Address: " + IeProxyOptions.ProxyAddr;
            }

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

        #region AppManager Notifications

        public void AppMgrNotify_NetworkChanged(object sender, EventArgs e)
        {
            UpdateGui_TextBoxMainContent();
            UpdateGui_GroupBoxTitle();
            UpdateGui_NotifyIconTextIndication();
            UpdateGui_NotifyIconMenuNetwork();
            UpdateGui_NotifyIconMenuWorkMode();
            if (WindowState == FormWindowState.Minimized) {
                UpdateGui_NotifyIconBalloonTip();
            }
        }

        #endregion

        #region User Notifications

        private void UserRequest_SwitchToAutoMode(object sender, EventArgs e)
        {
            m_appManagerRef.ProfileChangedWorkMode(WorkMode.Auto);
            m_appManagerRef.AutoSwitchProxy();
        }

        private void UserRequest_SwitchToDirectMode(object sender, EventArgs e)
        {
            m_appManagerRef.ProfileChangedWorkMode(WorkMode.Direct);
            m_appManagerRef.DisableProxy();
        }

        private void UserRequest_SwitchToProxyMode(object sender, EventArgs e)
        {
            m_appManagerRef.ProfileChangedWorkMode(WorkMode.Proxy);
            m_appManagerRef.EnableProxy();
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

        private void UserRequest_ExitApplication(object sender, EventArgs e)
        {
            this.Close();
        }

        private void UserRequest_ShowDlgAbout(object sender, EventArgs e)
        {
            DlgAboutBox.Instance.StartPosition =
                (this.WindowState == FormWindowState.Minimized)
                ? FormStartPosition.CenterScreen
                : FormStartPosition.CenterParent;
            DlgAboutBox.Instance.ShowDialog(this);
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
            UpdateGui_TextBoxMainContent();
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
        private List<MenuItem> m_listModeMenuItems;
        private MenuItem m_miNiCtxNetworkStatus;
        private MenuItem m_miNiCtxIPAddress;
    }
}
