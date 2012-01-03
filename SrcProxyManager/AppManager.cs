using System;
using System.Diagnostics;
using System.Net.NetworkInformation;
using System.IO;
using System.Text.RegularExpressions;
using Microsoft.Win32;


namespace ProxyManager
{
    public class AppManager
    {
        public const string PROXY_MANAGER_FILE_NAME = "ProxyManager.exe";
        public const string PROXY_AGENT_FILE_NAME = "ProxyAgent.exe";
        // Refer to [assembly: AssemblyProduct]
        public const string ASSEMBLY_PRODUCT = "Proxy Manager";

        public AppManager()
        {
            string path = Process.GetCurrentProcess().MainModule.FileName;
            m_szAppDir = Path.GetDirectoryName(path);
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
        }

        ~AppManager()
        {
            if (!IsLoadAppProfileFailed()) {
                Profile.Save(m_profile);
            }
        }

        public bool LoadAppEnvironment()
        {
            string path = Path.Combine(m_szAppDir, PROXY_AGENT_FILE_NAME);
            return File.Exists(path);
        }

        public bool LoadAppProfile()
        {
            bool createdNew;
            m_profile = Profile.Load(m_szAppDir, out createdNew);
            m_currWorkMode = m_profile.m_defWorkMode;
            return (!createdNew);
        }

        public bool IsLoadAppProfileFailed()
        {
            return Profile.IsLoadFailed();
        }


        public delegate void NotifyNetworkChanged(object sender, EventArgs e);
        public event NotifyNetworkChanged NotifyGuiNetworkChanged;

        public void DetectorNotify_NetworkChanged(object sender, EventArgs e)
        {
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
        }

        public Profile AppProfile
        {
            get { return m_profile; }
            //set { m_profile = value; }
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

        public void ApplyProfileItemsAll()
        {
            // TODO:
        }

        public void ApplyProfileItemAutoStart()
        {
            RegistryKey rk = Registry.CurrentUser.OpenSubKey(
                @"Software\Microsoft\Windows\CurrentVersion\Run", true);
            if (m_profile.m_isStartAuto) {
                rk.SetValue(ASSEMBLY_PRODUCT,
                    Path.Combine(m_szAppDir, PROXY_MANAGER_FILE_NAME));
            } else {
                rk.DeleteValue(ASSEMBLY_PRODUCT);
            }
            rk.Close();
        }

        public void SetCurrentWorkMode(WorkMode newMode)
        {
            m_currWorkMode = newMode;
        }

        public void StartCurrentWorkMode()
        {
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
        }

        public void EnableProxy()
        {
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
        }

        public void EnableProxy(ProxyItem pi)
        {
            string args = true.ToString() + " "
                + "\"" + pi.m_szProxyAddr + "\" "
                + "\"" + pi.m_szBypass + "\" "
                + pi.m_isAutoConfDisabled.ToString();
            
            Process process = PrepareProxyAgentProcess();
            SetArgsProxyAgentProcess(process, args);
            ExecuteProxyAgentProcess(process);

            NotifyGuiNetworkChanged(this, new EventArgs());
        }

        public void DisableProxy()
        {
            Process process = PrepareProxyAgentProcess();
            SetArgsProxyAgentProcess(process, false.ToString());
            ExecuteProxyAgentProcess(process);

            NotifyGuiNetworkChanged(this, new EventArgs());
        }

        public void AutoSwitchProxy()
        {
            if (m_detector.IsNetworkActive()) {
                ProxyItem pi = FindMatchedProxyItem();
                Profile.Save(m_profile);    // 'SelectedIndex' may be changed
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
        }

        private ProxyItem FindMatchedProxyItem()
        {
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

        private string m_szAppDir;

        private NetworkDetector m_detector;
        private Profile m_profile;

        private WorkMode m_currWorkMode;// current work mode
        private int m_idxProxyGroup;    // current proxy group index if Auto Mode
    }
}
