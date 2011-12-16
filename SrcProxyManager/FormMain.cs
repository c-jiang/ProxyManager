﻿using System;
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
            m_appManagerRef.ProxyChanged +=
                new AppManager.NotifyGuiProxyChanged(
                    NotificationProxyChanged);
            m_appManagerRef.NetworkAndProxyChanged +=
                new AppManager.NotifyGuiNetworkAndProxyChanged(
                    NotificationNetworkAndProxyChanged);

            InitializeComponent();
            UpdateTextBoxContent();
            UpdateGroupBoxTitle();
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


        private AppManager m_appManagerRef;
    }
}
