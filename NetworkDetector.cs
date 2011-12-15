﻿using System;
using System.Net.NetworkInformation;
using Microsoft.Win32;


namespace ProxyManager
{
    public class NetworkDetector
    {
        public NetworkDetector()
        {
            DetectActiveNetwork();
        }

        public delegate void NotifyAppManagerNetworkChanged(object sender, EventArgs e);
        public event NotifyAppManagerNetworkChanged NetworkChanged;

        #region Active Network Properties

        public bool IsNetworkActive()
        {
            return ((m_activeNetwork != null) && (m_activeIP != null));
        }

        public string ActiveNetworkId()
        {
            return m_activeNetwork.Id;
        }

        public string ActiveNetworkName()
        {
            return m_activeNetwork.Name;
        }

        public string ActiveNetworkType()
        {
            return m_activeNetwork.NetworkInterfaceType.ToString();
        }

        public string ActiveNetworkDescription()
        {
            return m_activeNetwork.Description;
        }

        public string ActiveNetworkIPAddress()
        {
            if (m_activeIP.UnicastAddresses.Count > 0) {
                return m_activeIP.UnicastAddresses[0].Address.ToString();
            } else {
                return String.Empty;
            }
        }

        public string ActiveNetworkSubMask()
        {
            if (m_activeIP.UnicastAddresses.Count > 0) {
                return m_activeIP.UnicastAddresses[0].IPv4Mask.ToString();
            } else {
                return String.Empty;
            }
        }

        public string ActiveNetworkGateway()
        {
            if (m_activeIP.GatewayAddresses.Count > 0) {
                return m_activeIP.GatewayAddresses[0].Address.ToString();
            } else {
                return String.Empty;
            }
        }

        public string ActiveNetworkDnsAddress()
        {
            if (m_activeIP.DnsAddresses.Count > 0) {
                return m_activeIP.DnsAddresses[0].ToString();
            } else {
                return String.Empty;
            }
        }

        public string[] ActiveNetworkDnsAddresses()
        {
            if (m_activeIP.DnsAddresses.Count > 0) {
                string[] ret = new string[m_activeIP.DnsAddresses.Count];
                for (int i = 0; i < m_activeIP.DnsAddresses.Count; ++i) {
                    ret[i] = m_activeIP.DnsAddresses[i].ToString();
                }
                return ret;
            } else {
                return null;    // TODO: reasonable?
            }
        }

        public string ActiveNetworkDnsSuffix()
        {
            return m_activeIP.DnsSuffix;
        }

        #endregion


        public void NetworkAddressChangedCallback(object sender, EventArgs e)
        {
            DetectActiveNetwork();
            NetworkChanged(this, new EventArgs());
        }

        public void DetectActiveNetwork()
        {
            m_activeNetwork = null;
            m_activeIP = null;
            NetworkInterface[] adapters = NetworkInterface.GetAllNetworkInterfaces();
            
            foreach (NetworkInterface ni in adapters) {
                // skip the non-up network adaptor
                if (!ni.OperationalStatus.Equals(OperationalStatus.Up)) {
                    continue;
                }
                // skip the loopback (localhost) network adaptor
                if (ni.NetworkInterfaceType.Equals(NetworkInterfaceType.Loopback)) {
                    continue;
                }
                // skip the unknown network adaptor
                if (ni.NetworkInterfaceType.Equals(NetworkInterfaceType.Unknown)) {
                    continue;
                }

                #region Determines physical network by Registry
                string key =
                    @"SYSTEM\CurrentControlSet\Control\Network\{4D36E972-E325-11CE-BFC1-08002BE10318}\"
                    + ni.Id + @"\Connection";
                RegistryKey entry = Registry.LocalMachine.OpenSubKey(key, false);
                if (entry != null) {
                    // Try to get PnpInstanceID; it's a physical LAN card if containing prefix "PCI".
                    string pnpInstanceId = entry.GetValue("PnpInstanceID", "").ToString();
                    // Try to get MediaSubType; virtual network if 1, wireless network if 2.
                    int mediaSubType = Convert.ToInt32(entry.GetValue("MediaSubType", 0));
                    if (pnpInstanceId.Length > 3 && pnpInstanceId.Substring(0, 3).Equals("PCI")) {
                        ;   // Physical Network
                    } else if (mediaSubType == 1) {
                        ;   // Virtual Network, not verified
                    } else if (mediaSubType == 2) {
                        ;   // Wirless Network, not verified
                    } else {
                        // VirtualBox Host-Only falls into this case
                        continue;
                    }
                } else {
                    ;   // F5 connection falls into this case, since no regkey corresponding to Id.
                }
                #endregion

                m_activeNetwork = ni;
                m_activeIP = ni.GetIPProperties();
                break;
            }
        }


        private NetworkInterface m_activeNetwork;
        private IPInterfaceProperties m_activeIP;
    }
}
