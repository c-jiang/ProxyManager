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
            appManager.NetworkChanged +=
                new AppManager.NotifyGuiNetworkChanged(NotificationNetworkChanged);
            InitializeComponent();

            string ui = "[" + Utils.GetDateTime() + "]" + "\r\n";
            tbStatus.Text = ui;
        }

        public void NotificationNetworkChanged(object sender, EventArgs e)
        {
            NetworkDetector nd = ((AppManager)sender).Detector;
            string ui = "[" + Utils.GetDateTime() + "]" + "\r\n";
            if (nd.IsNetworkActive()) {
                ui += "Network Active" + "\r\n";
                ui += "IP . . . . . . . : " + nd.ActiveNetworkIPAddress() + "\r\n";
                ui += "Mask . . . . . . : " + nd.ActiveNetworkSubMask() + "\r\n";
                ui += "Gateway. . . . . : " + nd.ActiveNetworkGateway() + "\r\n";
                ui += "DNS. . . . . . . : " + nd.ActiveNetworkDnsAddress() + "\r\n";
                ui += "DNS Suffix . . . : " + nd.ActiveNetworkDnsAddress() + "\r\n";
                ui += "\r\n";
            } else {
                ui += "Network Inactive" + "\r\n";
                ui += "\r\n";
            }
            tbStatus.Text = ui;
        }
    }
}
