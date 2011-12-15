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
        public event NotifyGuiNetworkChanged NetworkChanged;

        public void NotificationNetworkChanged(object sender, EventArgs e)
        {
            NetworkChanged(this, new EventArgs());
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
