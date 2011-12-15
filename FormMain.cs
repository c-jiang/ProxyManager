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
        }

        public void NotificationNetworkChanged(object sender, EventArgs e)
        {
            //NetworkDetector nd = ((AppManager)sender).Detector;
            //if (nd.IsNetworkActive()) {
            //    string ui = "NetworkActive\n";
            //    ui += "Ip: " + nd.ActiveNetworkIPAddress() + "\n";
            //    ui += "Mask: " + nd.ActiveNetworkSubMask() + "\n";
            //    ui += "Gateway: " + nd.ActiveNetworkGateway() + "\n";
            //    ui += "Dns: " + nd.ActiveNetworkDnsAddress() + "\n";
            //    MessageBox.Show(ui);
            //} else {
            //    MessageBox.Show("Network Inactive");
            //}
        }
    }
}
