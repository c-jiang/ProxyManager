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
            m_appManagerRef = appManager;
            m_prevState = FormWindowState.Normal;

            // TODO: move to a seperate method for callback registration
            m_appManagerRef.NetworkChanged +=
                new AppManager.NotifyGuiNetworkChanged(
                    this.NotificationNetworkChanged);
            m_appManagerRef.ProxyChanged +=
                new AppManager.NotifyGuiProxyChanged(
                    this.NotificationProxyChanged);
            m_appManagerRef.NetworkAndProxyChanged +=
                new AppManager.NotifyGuiNetworkAndProxyChanged(
                    this.NotificationNetworkAndProxyChanged);

            // Init GUI components
            InitializeComponent();
            this.Text = AssemblyProduct;
            aboutToolStripMenuItem.Text = "&About " + AssemblyProduct;
            InitNotifyIcon();

            // Update GUI components
            UpdateTextBoxContent();
            UpdateGroupBoxTitle();
            UpdateNotifyIcon();
        }

        public void NotificationNetworkChanged(object sender, EventArgs e)
        {
            UpdateTextBoxContent();
        }

        public void NotificationProxyChanged(object sender, EventArgs e)
        {
            UpdateTextBoxContent();
        }

        public void NotificationNetworkAndProxyChanged(object sender, EventArgs e)
        {
            UpdateTextBoxContent();
        }

        private void InitNotifyIcon()
        {
            notifyIcon.ContextMenu = new ContextMenu();
            notifyIcon.Visible = true;
        }

        private void UpdateNotifyIcon()
        {
            string str = AssemblyProduct + " ("
                + m_appManagerRef.AppProfile.m_workMode + " Mode)"
                + Environment.NewLine;
            str += "Network Status: "
                + (m_appManagerRef.Detector.IsNetworkActive() ? "Active" : "Inactive");
            notifyIcon.Text = str;
        }

        private void UpdateTextBoxContent()
        {
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
            ui += "Proxy Stat: ";
            ui += (IeProxyOptions.ProxyEnable ? "Enable" : "Disable");
            ui += "\r\n";
            ui += "Proxy Addr: " + IeProxyOptions.ProxyAddr;
            ui += "\r\n";
            ui += "Bypass: " + IeProxyOptions.Bypass;
            ui += "\r\n";
            tbStatus.Text = ui;
        }

        private void UpdateGroupBoxTitle()
        {
            gbWorkMode.Text = "Current Work Mode: "
                + m_appManagerRef.AppProfile.m_workMode
                + " Mode";
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            m_appManagerRef.Detector.DetectActiveNetwork();
            UpdateTextBoxContent();
        }

        private void btnAutoMode_Click(object sender, EventArgs e)
        {
            m_appManagerRef.AutoSwitchProxy();
            m_appManagerRef.UserChangeWorkMode(WorkMode.Auto);
            UpdateGroupBoxTitle();
        }

        private void btnDirectMode_Click(object sender, EventArgs e)
        {
            m_appManagerRef.DisableProxy();
            m_appManagerRef.UserChangeWorkMode(WorkMode.Direct);
            UpdateGroupBoxTitle();
        }

        private void btnProxyMode_Click(object sender, EventArgs e)
        {
            m_appManagerRef.EnableProxy();
            m_appManagerRef.UserChangeWorkMode(WorkMode.Proxy);
            UpdateGroupBoxTitle();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DlgAboutBox.Instance.ShowDialog(this);
        }

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

        private void notifyIcon_Click(object sender, EventArgs e)
        {
            var evt = e as MouseEventArgs;
            switch (evt.Button) {
            case MouseButtons.Left:
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
                break;
            case MouseButtons.Right:
                // TODO: show the context menu
                break;
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


        private AppManager m_appManagerRef;
        private FormWindowState m_prevState;
    }
}
