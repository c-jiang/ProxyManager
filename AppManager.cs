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
            if (m_profile.CurrentWorkMode.Equals(WorkMode.Auto)) {
                RegisterCallbacks();
            }
        }

        public delegate void NotifyGuiNetworkChanged(object sender, EventArgs e);
        public delegate void NotifyGuiNetworkAndProxyChanged(object sender, EventArgs e);
        public event NotifyGuiNetworkChanged NetworkChanged;
        public event NotifyGuiNetworkAndProxyChanged NetworkAndProxyChanged;

        public void NotificationNetworkChanged(object sender, EventArgs e)
        {
            // TODO: determines whether the proxy settings should be changed
            // Handle m_profile here.

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
