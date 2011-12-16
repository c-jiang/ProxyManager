using System;
using System.Runtime.InteropServices;
using Microsoft.Win32;


namespace ProxyManager
{
    class IeProxyOptions
    {
        public static bool ProxyEnable
        {
            get
            {
                OpenInternetSettings(false);
                int value = (int)m_rkIeOpt.GetValue("ProxyEnable", 0);
                m_rkIeOpt.Close();
                return (value > 0);
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
