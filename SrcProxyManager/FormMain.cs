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

            InitializeComponent();
            UpdateTextBoxContent();
            UpdateGroupBoxTitle();
            this.Text = AssemblyProduct;
            aboutToolStripMenuItem.Text = "&About " + AssemblyProduct;
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


        private AppManager m_appManagerRef;
    }
}
