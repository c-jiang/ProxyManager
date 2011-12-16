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
            // TODO: move to a seperate method for callback registration
            appManager.NetworkChanged +=
                new AppManager.NotifyGuiNetworkChanged(
                    NotificationNetworkChanged);
            appManager.NetworkAndProxyChanged +=
                new AppManager.NotifyGuiNetworkAndProxyChanged(
                    NotificationNetworkAndProxyChanged);

            InitializeComponent();
            UpdateTextBoxContent(appManager);
        }

        public void NotificationNetworkChanged(object sender, EventArgs e)
        {
            UpdateTextBoxContent((AppManager)sender);
        }

        public void NotificationNetworkAndProxyChanged(object sender, EventArgs e)
        {

        }

        private void UpdateTextBoxContent(AppManager appManager)
        {
            NetworkDetector nd = appManager.Detector;
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
    }
}
