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
    public partial class FormMain : Form
    {
        public FormMain(AppManager appManager)
        {
            m_appManagerRef = appManager;
            // TODO: move to a seperate method for callback registration
            m_appManagerRef.NetworkChanged +=
                new AppManager.NotifyGuiNetworkChanged(
                    NotificationNetworkChanged);
            m_appManagerRef.NetworkAndProxyChanged +=
                new AppManager.NotifyGuiNetworkAndProxyChanged(
                    NotificationNetworkAndProxyChanged);

            InitializeComponent();
            UpdateTextBoxContent();
        }

        public void NotificationNetworkChanged(object sender, EventArgs e)
        {
            UpdateTextBoxContent();
        }

        public void NotificationNetworkAndProxyChanged(object sender, EventArgs e)
        {

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
            ui += (IeProxyOptions.ProxyEnable ? "Proxy Enabled" : "Proxy Disabled");
            ui += "\r\n";
            ui += "Proxy Addr: " + IeProxyOptions.ProxyAddr;
            ui += "\r\n";
            ui += "Bypass: " + IeProxyOptions.Bypass;
            ui += "\r\n";
            tbStatus.Text = ui;
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            m_appManagerRef.Detector.DetectActiveNetwork();
            UpdateTextBoxContent();
        }

        private AppManager m_appManagerRef;
    }
}
