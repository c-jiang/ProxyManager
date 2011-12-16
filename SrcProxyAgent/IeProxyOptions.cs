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
            set
            {
                OpenInternetSettings(true);
                m_rkIeOpt.SetValue("ProxyServer", value);
                m_rkIeOpt.Close();
            }
        }

        public static string Bypass
        {
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
