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
            // Add listener for NetworkDetector's event 'NetworkChanged'
            m_detector.NetworkChanged +=
                new NetworkDetector.NotifyAppManagerNetworkChanged(
                    NotificationNetworkChanged);

            // Add listener for NetworkChange's event 'NetworkAddressChanged'
            NetworkChange.NetworkAddressChanged +=
                new NetworkAddressChangedEventHandler(
                    m_detector.NetworkAddressChangedCallback);
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

        private Profile m_profile;
        private NetworkDetector m_detector;
    }
}
