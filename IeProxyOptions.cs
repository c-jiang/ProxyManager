using System;
using Microsoft.Win32;


namespace ProxyManager
{
    class IeProxyOptions
    {
        public static bool IsProxyEnabled
        {
            get
            {
                OpenInternetSettings();
                int value = (int)m_rkIeOpt.GetValue("ProxyEnable", 0);
                m_rkIeOpt.Close();
                return (value > 0);
            }
            set
            {
                OpenInternetSettings();
                int setValue = (value ? 1 : 0);
                m_rkIeOpt.SetValue("ProxyEnable", setValue);
                m_rkIeOpt.Close();
            }
        }

        public static string ProxyAddr
        {
            get
            {
                OpenInternetSettings();
                string value = (string)m_rkIeOpt.GetValue(
                    "ProxyServer", String.Empty);
                m_rkIeOpt.Close();
                return value;
            }
            set
            {
                OpenInternetSettings();
                m_rkIeOpt.SetValue("ProxyServer", value);
                m_rkIeOpt.Close();
            }
        }

        public static string Bypass
        {
            get
            {
                OpenInternetSettings();
                string value = (string)m_rkIeOpt.GetValue(
                    "ProxyOverride", string.Empty);
                m_rkIeOpt.Close();

                int idx = value.IndexOf(BYPASS_LOCAL);
                if (idx >= 0) {
                    value.Remove(idx);
                    value = value.TrimEnd(';'); // TODO: test
                }
                return value;
            }
            set
            {
                OpenInternetSettings();
                string str = value.TrimEnd(';');
                str += (";" + BYPASS_LOCAL);
                m_rkIeOpt.SetValue("ProxyOverride", str);
                m_rkIeOpt.Close();
            }
        }

        public static bool IsAutoConfEnabled()
        {
            OpenInternetSettings();
            string value = (string)m_rkIeOpt.GetValue(
                "AutoConfigURL", null);
            m_rkIeOpt.Close();
            return (value != null) ? true : false;
        }

        public static void DisableAutoConf()
        {
            OpenInternetSettings();
            string value = (string)m_rkIeOpt.GetValue(
                "AutoConfigURL", null);
            if (value != null) {
                m_rkIeOpt.DeleteValue("AutoConfigURL");
            }
            m_rkIeOpt.Close();
        }


        private static void OpenInternetSettings()
        {
            m_rkIeOpt = Registry.CurrentUser.OpenSubKey(
                @"Software\Microsoft\Windows\CurrentVersion\Internet Settings", true);
        }

        private static RegistryKey m_rkIeOpt;
        private const string BYPASS_LOCAL = "<local>";
    }
}
