using System;
using System.Diagnostics;
using System.Net.NetworkInformation;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading;
using Microsoft.Win32;


namespace ProxyManager
{
    public class AppManager
    {
        public const string PROXY_MANAGER_FILE_NAME = "ProxyManager.exe";
        public const string PROXY_AGENT_FILE_NAME = "ProxyAgent.exe";
        public const string APP_NEW_LOG_FILE_NAME = "ProxyManager-New.log";
        public const string APP_OLD_LOG_FILE_NAME = "ProxyManager-Old.log";
        // Refer to [assembly: AssemblyProduct]
        public const string ASSEMBLY_PRODUCT = "Proxy Manager";

        public AppManager(string appDir)
        {
            Logger.V(">> AppManager.AppManager");

            m_semaphore = new Semaphore(1, 1);
            m_szAppDir = appDir;
            m_detector = new NetworkDetector();
            m_profile = null;
            m_currWorkMode = WorkMode.Direct;

            // link NetworkDetector to AppManager
            m_detector.NetworkChanged +=
                new NetworkDetector.NotifyAppManagerNetworkChanged(
                    this.DetectorNotify_NetworkChanged);

            // link OS to NetworkDetector
            NetworkChange.NetworkAddressChanged +=
                new NetworkAddressChangedEventHandler(
                    m_detector.OsNotify_NetworkChanged);

            Logger.V("<< AppManager.AppManager");
        }

        ~AppManager()
        {
            Logger.V(">> AppManager.~AppManager");
            if (!IsLoadAppProfileFailed()) {
                Logger.I("AppManager.~AppManager :: Save the profile to local disk.");
                Profile.Save(m_profile);
            }
            Logger.V("<< AppManager.~AppManager");
        }

        public bool LoadAppEnvironment()
        {
            Logger.V(">> AppManager.LoadAppEnvironment");
            string path = Path.Combine(m_szAppDir, PROXY_AGENT_FILE_NAME);
            bool ret = File.Exists(path);
            Logger.V("<< AppManager.LoadAppEnvironment : " + ret.ToString());
            return ret;
        }

        public bool LoadAppProfile()
        {
            bool createdNew;
            m_profile = Profile.Load(m_szAppDir, out createdNew);
            m_currWorkMode = m_profile.m_defWorkMode;
            if (m_profile.m_isLogToFile) {
                Logger.Enable(m_profile.m_logLevel);
            }
            Logger.V(">> AppManager.LoadAppProfile");   // move to here as a countermeasure
            if (createdNew) {
                Logger.I("AppManager.LoadAppProfile :: Profile is created and loaded.");
            } else {
                Logger.I("AppManager.LoadAppProfile :: Profile exists and is loaded.");
            }
            Logger.V("<< AppManager.LoadAppProfile : " + createdNew.ToString());
            return createdNew;
        }

        public bool IsLoadAppProfileFailed()
        {
            Logger.V(">> AppManager.IsLoadAppProfileFailed");
            bool ret = Profile.IsLoadFailed();
            Logger.V("<< AppManager.IsLoadAppProfileFailed : " + ret.ToString());
            return ret;
        }


        public delegate void NotifyNetworkChanged(object sender, EventArgs e);
        public event NotifyNetworkChanged NotifyGuiNetworkChanged;

        public void DetectorNotify_NetworkChanged(object sender, EventArgs e)
        {
            Logger.V(">> AppManager.DetectorNotify_NetworkChanged");
            switch (m_currWorkMode) {
            case WorkMode.Auto:
                Logger.I("AppManager.DetectorNotify_NetworkChanged :: "
                    + "Apply Auto Mode by the following routines.");
                AutoSwitchProxy();
                break;
            case WorkMode.Direct:
                Logger.I("AppManager.DetectorNotify_NetworkChanged :: "
                    + "Apply Direct Mode by disabling system proxy option.");
                DisableProxy();
                break;
            case WorkMode.Proxy:
                Logger.I("AppManager.DetectorNotify_NetworkChanged :: "
                    + "Apply Proxy Mode by enabling system proxy option.");
                EnableProxy();
                break;
            }
            Logger.V("<< AppManager.DetectorNotify_NetworkChanged");
        }

        public Profile AppProfile
        {
            get { return m_profile; }
            set { m_profile = value; }
        }

        public WorkMode CurrWorkMode
        {
            get { return m_currWorkMode; }
        }

        public NetworkDetector Detector
        {
            get { return m_detector; }
        }

        public int AutoModeProxyGroupIndex
        {
            get { return m_idxProxyGroup; }
        }

        public void ApplyProfileUpdate()
        {
            Logger.V(">> AppManager.ApplyProfileUpdate");
            // XPath: /Profile/StartAuto
            ApplyProfileItemAutoStart();
            // XPath: /Profile/LogToFile
            if (m_profile.m_isLogToFile) {
                Logger.Enable(m_profile.m_logLevel);
            } else {
                Logger.Disable();
            }
            // restart current work mode, and update GUI
            Logger.I("AppManager.ApplyProfileUpdate :: "
                + "Trigger the current work mode restarting.");
            StartCurrentWorkMode();
            Logger.V("<< AppManager.ApplyProfileUpdate");
        }

        public void ApplyProfileItemAutoStart()
        {
            Logger.V(">> AppManager.ApplyProfileItemAutoStart");
            RegistryKey rk = Registry.CurrentUser.OpenSubKey(
                @"Software\Microsoft\Windows\CurrentVersion\Run", true);
            if (m_profile.m_isStartAuto) {
                rk.SetValue(ASSEMBLY_PRODUCT,
                    Path.Combine(m_szAppDir, PROXY_MANAGER_FILE_NAME));
            } else if (rk.GetValue(ASSEMBLY_PRODUCT) != null) {
                rk.DeleteValue(ASSEMBLY_PRODUCT);
            }
            rk.Close();
            Logger.V("<< AppManager.ApplyProfileItemAutoStart");
        }

        public void SetCurrentWorkMode(WorkMode newMode)
        {
            Logger.V(">> AppManager.SetCurrentWorkMode(@1:" + newMode.ToString() + ")");
            m_currWorkMode = newMode;
            Logger.V("<< AppManager.SetCurrentWorkMode(@1:" + newMode.ToString() + ")");
        }

        public void StartCurrentWorkMode()
        {
            Logger.V(">> AppManager.StartCurrentWorkMode");
            switch (m_currWorkMode) {
            case WorkMode.Auto:
                Logger.I("AppManager.StartCurrentWorkMode :: "
                    + "Apply Auto Mode by the following routines.");
                AutoSwitchProxy();
                break;
            case WorkMode.Direct:
                Logger.I("AppManager.StartCurrentWorkMode :: "
                    + "Apply Direct Mode by disabling system proxy option.");
                DisableProxy();
                break;
            case WorkMode.Proxy:
                Logger.I("AppManager.StartCurrentWorkMode :: "
                    + "Apply Proxy Mode by enabling system proxy option.");
                EnableProxy();
                break;
            }
            Logger.V("<< AppManager.StartCurrentWorkMode");
        }

        public void EnableProxy()
        {
            Logger.V(">> AppManager.EnableProxy");
            m_semaphore.WaitOne();
            string szProxyAddr = IeProxyOptions.ProxyAddr;
            string szBypass = IeProxyOptions.Bypass;
            string args = true.ToString() + " "
                + "\"" + szProxyAddr + "\" "
                + "\"" + szBypass + "\" "
                + true.ToString();
            RunProcessProxyAgent(args);
            if (IeProxyOptions.ProxyEnable != true
                || IeProxyOptions.AutoConfDisabled != true) {
                Logger.W("AppManager.EnableProxy :: "
                    + "Failed to enable system proxy for the 1st round.");
                System.Threading.Thread.Sleep(1000);
                RunProcessProxyAgent(args);
                if (IeProxyOptions.ProxyEnable != true
                    || IeProxyOptions.AutoConfDisabled != true) {
                    Logger.E("AppManager.EnableProxy :: "
                        + "Failed to enable system proxy for the 2nd round.");
                } else {
                    Logger.I("AppManager.EnableProxy :: "
                    + "Correctly enable system proxy for the 2nd round.");
                }
            } else {
                Logger.I("AppManager.EnableProxy :: "
                    + "Correctly enable system proxy for the 1st round.");
            }
            NotifyGuiNetworkChanged(this, new EventArgs());
            m_semaphore.Release();
            Logger.V("<< AppManager.EnableProxy");
        }

        public void EnableProxy(ProxyItem pi)
        {
            Logger.V(">> AppManager.EnableProxy(@1.ProxyAddr:" + pi.m_szProxyAddr + ")");
            m_semaphore.WaitOne();
            string args = true.ToString() + " "
                + "\"" + pi.m_szProxyAddr + "\" "
                + "\"" + pi.m_szBypass + "\" "
                + pi.m_isAutoConfDisabled.ToString();
            RunProcessProxyAgent(args);
            if (IeProxyOptions.ProxyEnable != true
                || IeProxyOptions.ProxyAddr != pi.m_szProxyAddr
                || IeProxyOptions.Bypass != pi.m_szBypass
                || IeProxyOptions.AutoConfDisabled != pi.m_isAutoConfDisabled) {
                Logger.W("AppManager.EnableProxy :: "
                    + "Failed to enable system proxy as specified for the 1st round.");
                RunProcessProxyAgent(args);
                if (IeProxyOptions.ProxyEnable != true
                    || IeProxyOptions.ProxyAddr != pi.m_szProxyAddr
                    || IeProxyOptions.Bypass != pi.m_szBypass
                    || IeProxyOptions.AutoConfDisabled != pi.m_isAutoConfDisabled) {
                    Logger.E("AppManager.EnableProxy :: "
                        + "Failed to enable system proxy as specified for the 2nd round.");
                } else {
                    Logger.I("AppManager.EnableProxy :: "
                        + "Correctly enable system proxy as specified for the 2nd round.");
                }
            } else {
                Logger.I("AppManager.EnableProxy :: "
                    + "Correctly enable system proxy as specified for the 1st round.");
            }
            NotifyGuiNetworkChanged(this, new EventArgs());
            m_semaphore.Release();
            Logger.V("<< AppManager.EnableProxy(@1.ProxyAddr:" + pi.m_szProxyAddr + ")");
        }

        public void DisableProxy()
        {
            Logger.V(">> AppManager.DisableProxy");
            m_semaphore.WaitOne();
            RunProcessProxyAgent(false.ToString());
            if (IeProxyOptions.ProxyEnable != false) {
                Logger.W("AppManager.DisableProxy :: "
                    + "Failed to disable system proxy for the 1st round.");
                RunProcessProxyAgent(false.ToString());
                if (IeProxyOptions.ProxyEnable != false) {
                    Logger.E("AppManager.DisableProxy :: "
                        + "Failed to disable system proxy for the 2nd round.");
                } else {
                    Logger.I("AppManager.DisableProxy :: "
                        + "Correctly disable system proxy for the 2nd round.");
                }
            } else {
                Logger.I("AppManager.DisableProxy :: "
                    + "Correctly disable system proxy for the 1st round.");
            }
            NotifyGuiNetworkChanged(this, new EventArgs());
            m_semaphore.Release();
            Logger.V("<< AppManager.DisableProxy");
        }

        public void AutoSwitchProxy()
        {
            Logger.V(">> AppManager.AutoSwitchProxy");
            if (m_detector.IsNetworkActive()) {
                Logger.I("AppManager.AutoSwitchProxy :: "
                    + "Network is active and now trying to find an appropriate proxy item.");
                ProxyItem pi = FindMatchedProxyItem();
                if (pi != null) {
                    // Apply the found proxy since the rule matched
                    Logger.I("AppManager.AutoSwitchProxy :: "
                        + "The appropriate proxy item is found.");
                    EnableProxy(pi);
                } else {
                    // Disable proxy since no rule applied
                    Logger.I("AppManager.AutoSwitchProxy :: "
                        + "Failed to find an appropriate proxy item.");
                    DisableProxy();
                }
            } else {
                // Disable proxy if no active network
                Logger.I("AppManager.AutoSwitchProxy :: Network is inactive.");
                DisableProxy();
            }
            Logger.V("<< AppManager.AutoSwitchProxy");
        }

        private ProxyItem FindMatchedProxyItem()
        {
            Logger.V(">> AppManager.FindMatchedProxyItem");
            ProxyItem ret = null;
            for (int i = 0; i < m_profile.m_listProxyGroups.Count; ++i) {
                ProxyGroup pg = m_profile.m_listProxyGroups[i];

                if (!pg.m_isEnabled) {
                    continue;
                }
                if (!IsRuleMatched(pg.m_applyRule)) {
                    continue;
                }

                if (pg.m_iSelectedIndex <= 0 ||
                    pg.m_iSelectedIndex > pg.m_listProxyItems.Count) {
                    // reset the invalid SelectedIndex value
                    pg.m_iSelectedIndex = 0;
                } else if (pg.m_listProxyItems[pg.m_iSelectedIndex - 1].m_isEnabled) {
                    // directly get the specified selected proxy item
                    ret = pg.m_listProxyItems[pg.m_iSelectedIndex - 1];
                    m_idxProxyGroup = i;
                    break;
                }

                // try to get the proxy item by default routine
                for (int j = 0; j < pg.m_listProxyItems.Count; ++j) {
                    ProxyItem pi = pg.m_listProxyItems[j];
                    if (pi.m_isEnabled) {
                        ret = pi;
                        pg.m_iSelectedIndex = j + 1;
                        m_idxProxyGroup = i;
                        break;
                    }
                }
                if (ret != null) {
                    break;
                } else {
                    pg.m_iSelectedIndex = 0;
                }
            }
            Logger.V("<< AppManager.FindMatchedProxyItem");
            return ret;
        }

        private bool IsRuleMatched(ApplyRule rule)
        {
            bool ret = true;
            if (ret && rule.m_bIdFilter) {
                ret &= IsExpressionMatched(rule.m_szIdFilter,
                    m_detector.ActiveNetworkId());
            }
            if (ret && rule.m_bNameFilter) {
                ret &= IsExpressionMatched(rule.m_szNameFilter,
                    m_detector.ActiveNetworkName());
            }
            if (ret && rule.m_bIpAddrFilter) {
                ret &= IsExpressionMatched(rule.m_szIpAddrFilter,
                    m_detector.ActiveNetworkIPAddress());
            }
            if (ret && rule.m_bSubMaskFilter) {
                ret &= IsExpressionMatched(rule.m_szSubMaskFilter,
                    m_detector.ActiveNetworkSubMask());
            }
            if (ret && rule.m_bGatewayFilter) {
                ret &= IsExpressionMatched(rule.m_szGatewayFilter,
                    m_detector.ActiveNetworkGateway());
            }
            if (ret && rule.m_bDnsAddrFilter) {
                ret &= IsExpressionMatched(rule.m_szDnsAddrFilter,
                    m_detector.ActiveNetworkDnsAddress());
            }
            if (ret && rule.m_bDnsSuffixFilter) {
                ret &= IsExpressionMatched(rule.m_szDnsSuffixFilter,
                    m_detector.ActiveNetworkDnsSuffix());
            }
            return ret;
        }

        private bool IsExpressionMatched(string exp, string target)
        {
            string pattern = exp.Replace(@".", @"\.");
            pattern = pattern.Replace(@"*", @".*");
            pattern = pattern.Replace(@"?", @".");
            Regex regex = new Regex(pattern);
            Match match = regex.Match(target);
            return (match.Success && match.Value.Length == target.Length);
        }

        private void RunProcessProxyAgent(string args)
        {
            Process process = new Process();
            process.StartInfo.CreateNoWindow = true;
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.WorkingDirectory = m_szAppDir;
            process.StartInfo.FileName = PROXY_AGENT_FILE_NAME;
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.RedirectStandardError = true;
            process.StartInfo.Arguments = args;
            process.Start();
            process.WaitForExit();
            process.Close();
        }


        private Semaphore m_semaphore;
        private string m_szAppDir;

        private NetworkDetector m_detector;
        private Profile m_profile;

        private WorkMode m_currWorkMode;// current work mode
        private int m_idxProxyGroup;    // current proxy group index if Auto Mode
    }
}
