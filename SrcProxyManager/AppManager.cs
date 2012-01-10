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
            Logger.V(">> AppManager.LoadAppProfile");
            bool createdNew;
            m_profile = Profile.Load(m_szAppDir, out createdNew);
            m_currWorkMode = m_profile.m_defWorkMode;
            if (m_profile.m_isLogToFile) {
                Logger.Enable(m_profile.m_logLevel);
            }
            bool ret = !createdNew;
            Logger.V("<< AppManager.LoadAppProfile : " + ret.ToString());
            return ret;
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
                AutoSwitchProxy();
                break;
            case WorkMode.Direct:
                DisableProxy();
                break;
            case WorkMode.Proxy:
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
                AutoSwitchProxy();
                break;
            case WorkMode.Direct:
                DisableProxy();
                break;
            case WorkMode.Proxy:
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
            Process process = PrepareProxyAgentProcess();
            SetArgsProxyAgentProcess(process, args);
            ExecuteProxyAgentProcess(process);
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
            Process process = PrepareProxyAgentProcess();
            SetArgsProxyAgentProcess(process, args);
            ExecuteProxyAgentProcess(process);
            NotifyGuiNetworkChanged(this, new EventArgs());
            m_semaphore.Release();
            Logger.V("<< AppManager.EnableProxy(@1.ProxyAddr:" + pi.m_szProxyAddr + ")");
        }

        public void DisableProxy()
        {
            Logger.V(">> AppManager.DisableProxy");
            m_semaphore.WaitOne();
            Process process = PrepareProxyAgentProcess();
            SetArgsProxyAgentProcess(process, false.ToString());
            ExecuteProxyAgentProcess(process);
            NotifyGuiNetworkChanged(this, new EventArgs());
            m_semaphore.Release();
            Logger.V("<< AppManager.DisableProxy");
        }

        public void AutoSwitchProxy()
        {
            Logger.V(">> AppManager.AutoSwitchProxy");
            if (m_detector.IsNetworkActive()) {
                ProxyItem pi = FindMatchedProxyItem();
                if (pi != null) {
                    // Apply the found proxy since the rule matched
                    EnableProxy(pi);
                } else {
                    // Disable proxy since no rule applied
                    DisableProxy();
                }
            } else {
                // Disable proxy if no active network
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

        private Process PrepareProxyAgentProcess()
        {
            Process process = new Process();
            process.StartInfo.CreateNoWindow = true;
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.WorkingDirectory = m_szAppDir;
            process.StartInfo.FileName = PROXY_AGENT_FILE_NAME;
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.RedirectStandardError = true;
            return process;
        }

        private void SetArgsProxyAgentProcess(Process process, string args)
        {
            process.StartInfo.Arguments = args;
        }

        private void ExecuteProxyAgentProcess(Process process)
        {
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
