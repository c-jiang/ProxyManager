using System;
using System.Diagnostics;
using System.Net.NetworkInformation;
using System.IO;


namespace ProxyManager
{
    public class AppManager
    {
        public AppManager()
        {
            string path = Process.GetCurrentProcess().MainModule.FileName;
            path = Path.GetDirectoryName(path);
            m_profile = Profile.Load(path);

            m_detector = new NetworkDetector();
            if (m_profile.m_workMode.Equals(WorkMode.Auto)) {
                RegisterCallbacks();
            }
        }

        public delegate void NotifyGuiNetworkChanged(object sender, EventArgs e);
        public delegate void NotifyGuiNetworkAndProxyChanged(object sender, EventArgs e);
        public event NotifyGuiNetworkChanged NetworkChanged;
        public event NotifyGuiNetworkAndProxyChanged NetworkAndProxyChanged;

        public void NotificationNetworkChanged(object sender, EventArgs e)
        {
            // TODO: determines whether the proxy settings should be changed, based on lastProxy
            if (m_detector.IsNetworkActive()) {
                ProxyItem pi = FindMatchedProxyItem();
                // here find method is not completed...
                if (pi != null) {
                    // Apply the found proxy since the rule matched
                    EnableProxy(pi);
                } else {
                    // Disable proxy since no rule applied
                    DisableProxy();
                }
            } else {
                // Disable proxy if no active network
                // TODO: it seems proxy cannot be changed if network is unavailable
                DisableProxy();
            }

            // GUI notifications
            // case - no proxy settings changed
            NetworkChanged(this, new EventArgs());
            // case - proxy settings need to be changed
            NetworkAndProxyChanged(this, new EventArgs());
        }

        public Profile AppProfile
        {
            get { return m_profile; }
            //set { m_profile = value; }
        }

        public NetworkDetector Detector
        {
            get { return m_detector; }
        }


        private void EnableProxy(ProxyItem pi)
        {
            // TODO: tricky temp countermeasure
            if (pi.m_isAutoConfDisabled) {
                IeProxyOptions.DisableAutoConf();
            }
            IeProxyOptions.ProxyEnable = true;
            IeProxyOptions.ProxyAddr = pi.m_szProxyAddr;
            IeProxyOptions.Bypass = pi.m_szBypass;
            IeProxyOptions.CommitChange();

            //if (IeProxyOptions.ProxyEnable == false) {
            //    IeProxyOptions.ProxyEnable = true;
            //    IeProxyOptions.ProxyAddr = pi.m_szProxyAddr;
            //    IeProxyOptions.Bypass = pi.m_szBypass;
            //}
        }

        private void DisableProxy()
        {
            // TODO: tricky temp countermeasure
            IeProxyOptions.ProxyEnable = false;
            IeProxyOptions.CommitChange();

            //if (IeProxyOptions.ProxyEnable == true) {
            //    IeProxyOptions.ProxyEnable = false;
            //}
        }

        private ProxyItem FindMatchedProxyItem()
        {
            ProxyItem ret = null;
            foreach (ProxyGroup pg in m_profile.m_listProxyGroups) {
                if (!pg.m_isEnabled) {
                    continue;
                }

                // handle pg.m_applyRule ...
                // suppose it is matched based on applyRule

                foreach (ProxyItem pi in pg.m_listProxyItems) {
                    if (pi.m_isEnabled) {
                        ret = pi;
                        break;
                    }
                }
                if (ret != null) {
                    break;
                }
            }
            return ret;
        }

        private void RegisterCallbacks()
        {
            // Link NetworkDetector.NetworkChanged to AppManager
            m_detector.NetworkChanged +=
                new NetworkDetector.NotifyAppManagerNetworkChanged(
                    NotificationNetworkChanged);

            // Link system NetworkChange.NetworkAddressChanged to NetworkDetector
            NetworkChange.NetworkAddressChanged +=
                new NetworkAddressChangedEventHandler(
                    m_detector.NetworkAddressChangedCallback);
        }

        private void DeregisterCallbacks()
        {
            // Remove link between NetworkDetector.NetworkChanged and AppManager
            m_detector.NetworkChanged -=
                new NetworkDetector.NotifyAppManagerNetworkChanged(
                    NotificationNetworkChanged);

            // Remove link between system NetworkChange.NetworkAddressChanged and NetworkDetector
            NetworkChange.NetworkAddressChanged -=
                new NetworkAddressChangedEventHandler(
                    m_detector.NetworkAddressChangedCallback);
        }

        private Profile m_profile;
        private NetworkDetector m_detector;
    }
}
