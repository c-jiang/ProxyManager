using System;
using System.Runtime.InteropServices;
using Microsoft.Win32;


namespace ProxyAgent
{
    class IeProxyOptions
    {
        [DllImport("wininet.dll")]
        public static extern bool InternetSetOption(IntPtr hInternet, int dwOption, IntPtr lpBuffer, int dwBufferLength);
        public const int INTERNET_OPTION_SETTINGS_CHANGED = 39;
        public const int INTERNET_OPTION_REFRESH = 37;

        public static void CommitChange()
        {
            InternetSetOption(IntPtr.Zero, INTERNET_OPTION_SETTINGS_CHANGED, IntPtr.Zero, 0);
            InternetSetOption(IntPtr.Zero, INTERNET_OPTION_REFRESH, IntPtr.Zero, 0);
        }

        public static bool ProxyEnable
        {
            get
            {
                OpenInternetSettings(false);
                int value = (int)m_rkIeOpt.GetValue("ProxyEnable", 0);
                m_rkIeOpt.Close();
                return (value > 0);
            }
            set
            {
                OpenInternetSettings(true);
                int setValue = (value ? 1 : 0);
                m_rkIeOpt.SetValue("ProxyEnable", setValue);
                m_rkIeOpt.Close();
            }
        }

        public static string ProxyAddr
        {
            get
            {
                OpenInternetSettings(false);
                string value = (string)m_rkIeOpt.GetValue(
                    "ProxyServer", String.Empty);
                m_rkIeOpt.Close();
                return value;
            }
            set
            {
                OpenInternetSettings(true);
                m_rkIeOpt.SetValue("ProxyServer", value);
                m_rkIeOpt.Close();
            }
        }

        public static string Bypass
        {
            get
            {
                OpenInternetSettings(false);
                string value = (string)m_rkIeOpt.GetValue(
                    "ProxyOverride", string.Empty);
                m_rkIeOpt.Close();

                int idx = value.IndexOf(BYPASS_LOCAL);
                if (idx >= 0) {
                    value = value.Remove(idx);
                    value = value.TrimEnd(';'); // TODO: test
                }
                return value;
            }
            set
            {
                OpenInternetSettings(true);
                string str = String.Empty;
                if (!value.Equals(String.Empty)) {
                    str = value.TrimEnd(';');
                    str += ";";
                }
                str += BYPASS_LOCAL;
                m_rkIeOpt.SetValue("ProxyOverride", str);
                m_rkIeOpt.Close();
            }
        }

        public static bool IsAutoConfEnabled()
        {
            OpenInternetSettings(false);
            string value = (string)m_rkIeOpt.GetValue(
                "AutoConfigURL", null);
            m_rkIeOpt.Close();
            return (value != null) ? true : false;
        }

        public static void DisableAutoConf()
        {
            OpenInternetSettings(true);
            string value = (string)m_rkIeOpt.GetValue(
                "AutoConfigURL", null);
            if (value != null) {
                m_rkIeOpt.DeleteValue("AutoConfigURL");
            }
            m_rkIeOpt.Close();
        }


        private static void OpenInternetSettings(bool writable)
        {
            m_rkIeOpt = Registry.CurrentUser.OpenSubKey(
                @"Software\Microsoft\Windows\CurrentVersion\Internet Settings", writable);
        }

        private static RegistryKey m_rkIeOpt;
        private const string BYPASS_LOCAL = "<local>";
    }
}
