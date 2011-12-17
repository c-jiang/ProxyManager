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

            // TODO: move to a seperate method for callback registration
            m_appManagerRef.NetworkChanged +=
                new AppManager.NotifyGuiNetworkChanged(
                    this.AppMgrNotify_NetworkChanged);
            m_appManagerRef.ProxyChanged +=
                new AppManager.NotifyGuiProxyChanged(
                    this.AppMgrNotify_ProxyChanged);
            m_appManagerRef.NetworkAndProxyChanged +=
                new AppManager.NotifyGuiNetworkAndProxyChanged(
                    this.AppMgrNotify_NetworkAndProxyChanged);

            // init GUI components
            InitializeComponent();
            if (m_appManagerRef.AppProfile.m_isStartMinimized) {
                // TODO: whether move to ApplyAppProfile()
                this.ShowInTaskbar = false;
                this.WindowState = FormWindowState.Minimized;
            }
            this.Text = AssemblyProduct;
            aboutToolStripMenuItem.Text = "&About " + AssemblyProduct;
            InitGuiNotifyIcon();

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
        }

        private void InitGuiNotifyIcon()
        {
            // Init NotifyIcon
            notifyIcon.ContextMenu = new ContextMenu();
            notifyIcon.Visible = true;

            // Init NotifyIconMenuContext
            MenuItem[] mis = new MenuItem[11];
            int idx = 0;

            mis[idx] = new MenuItem();
            mis[idx].Text = "Exit " + AssemblyProduct;
            mis[idx].Click += new System.EventHandler(exitToolStripMenuItem_Click);
            ++idx;
            mis[idx] = new MenuItem("-");
            ++idx;
            mis[idx] = new MenuItem();
            mis[idx].Text = "About " + AssemblyProduct;
            mis[idx].Click += new System.EventHandler(aboutToolStripMenuItem_Click);
            ++idx;
            mis[idx] = new MenuItem("-");
            ++idx;
            mis[idx] = new MenuItem();
            mis[idx].Text = "Options";
            // TODO: fix the entry handler from About to Options
            mis[idx].Click += new System.EventHandler(aboutToolStripMenuItem_Click);
            ++idx;
            mis[idx] = new MenuItem("-");
            ++idx;
            mis[idx] = new MenuItem();
            mis[idx].Text = "Auto Mode";
            mis[idx].Click += new System.EventHandler(UserNotify_WorkModeAutoEnabled);
            mis[idx].RadioCheck = true;
            mis[idx].Checked = false;
            m_listModeMenuItems.Add(mis[idx]);  // [0] Auto Mode
            ++idx;
            mis[idx] = new MenuItem();
            mis[idx].Text = "Direct Mode";
            mis[idx].Click += new System.EventHandler(UserNotify_WorkModeDirectEnabled);
            mis[idx].RadioCheck = true;
            mis[idx].Checked = false;
            m_listModeMenuItems.Add(mis[idx]);  // [1] Direct Mode
            ++idx;
            mis[idx] = new MenuItem();
            mis[idx].Text = "Proxy Mode";
            mis[idx].Click += new System.EventHandler(UserNotify_WorkModeProxyEnabled);
            mis[idx].RadioCheck = true;
            mis[idx].Checked = false;
            m_listModeMenuItems.Add(mis[idx]);  // [2] Proxy Mode
            ++idx;
            mis[idx] = new MenuItem("-");
            ++idx;
            mis[idx] = new MenuItem();
            mis[idx].Text = "Open " + AssemblyProduct;
            mis[idx].Click += new System.EventHandler(UserNotify_FormMainActived);
            mis[idx].DefaultItem = true;

            notifyIcon.ContextMenu = new ContextMenu(mis);
        }

        private void UpdateGuiNetworkChanged()
        {
            // update text box
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

            // update notify icon
            string str = AssemblyProduct + " ("
                + m_appManagerRef.AppProfile.m_workMode + " Mode)"
                + Environment.NewLine;
            str += "Network Status: "
                + (m_appManagerRef.Detector.IsNetworkActive() ? "Active" : "Inactive");
            notifyIcon.Text = str;

        }

        private void UpdateGuiProxyChanged()
        {
            // update group box title
            gbWorkMode.Text = "Current Work Mode: "
                + m_appManagerRef.AppProfile.m_workMode
                + " Mode";

            // update notify icon
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
            UpdateGuiNetworkChanged();
        }

        public void AppMgrNotify_ProxyChanged(object sender, EventArgs e)
        {
            UpdateGuiProxyChanged();
        }

        public void AppMgrNotify_NetworkAndProxyChanged(object sender, EventArgs e)
        {
            UpdateGuiProxyChanged();
            UpdateGuiNetworkChanged();
        }

        #endregion

        #region User Notifications

        private void UserNotify_WorkModeAutoEnabled(object sender, EventArgs e)
        {
            m_appManagerRef.ProfileChangedWorkMode(WorkMode.Auto);
            m_appManagerRef.AutoSwitchProxy();
        }

        private void UserNotify_WorkModeDirectEnabled(object sender, EventArgs e)
        {
            m_appManagerRef.ProfileChangedWorkMode(WorkMode.Direct);
            m_appManagerRef.DisableProxy();
        }

        private void UserNotify_WorkModeProxyEnabled(object sender, EventArgs e)
        {
            m_appManagerRef.ProfileChangedWorkMode(WorkMode.Proxy);
            m_appManagerRef.EnableProxy();
        }

        private void UserNotify_FormMainActived(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Minimized) {
                this.Show();                    // step 1 - show
                this.ShowInTaskbar = true;      // step 2 - show
                this.WindowState = m_prevState; // step 3 - show
            } else {
                this.Activate();
            }
        }

        #endregion

        #region Event Handlers to GUI Components

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            m_appManagerRef.Detector.DetectActiveNetwork();
            UpdateGuiNetworkChanged();
        }

        private void btnAutoMode_Click(object sender, EventArgs e)
        {
            UserNotify_WorkModeAutoEnabled(sender, e);
        }

        private void btnDirectMode_Click(object sender, EventArgs e)
        {
            UserNotify_WorkModeDirectEnabled(sender, e);
        }

        private void btnProxyMode_Click(object sender, EventArgs e)
        {
            UserNotify_WorkModeProxyEnabled(sender, e);
        }

        private void minimizeToTrayToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.ShowInTaskbar = false;                     // step 1 - hide
            this.WindowState = FormWindowState.Minimized;   // step 2 - hide
            this.Hide();                                    // step 3 - hide
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DlgAboutBox.Instance.StartPosition =
                (this.WindowState == FormWindowState.Minimized)
                ? FormStartPosition.CenterScreen
                : FormStartPosition.CenterParent;
            DlgAboutBox.Instance.ShowDialog(this);
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
    }
}
