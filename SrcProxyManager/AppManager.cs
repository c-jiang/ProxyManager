﻿using System;
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
            switch (m_profile.m_workMode) {
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
            NotifyGuiNetworkChanged(sender, e);
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

        public bool AutoSwitchProxy()
        {
            // TODO: determines whether the proxy settings should be changed, based on lastProxy
            if (m_detector.IsNetworkActive()) {
                ProxyItem pi = FindMatchedProxyItem();
                // here find method is not completed...
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
            return true;
        }

        public void ProfileChangedWorkMode(WorkMode newMode)
        {
            // TODO: change to profile changed all
            WorkMode oldMode = m_profile.m_workMode;
            if (oldMode != newMode) {
                m_profile.m_workMode = newMode;
                Profile.Save(m_profile);
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
            if (ret && rule.m_bIpAddrFilter) {
                ret &= IsExpressionMatched(rule.m_szIpAddrFilter,
                    m_detector.ActiveNetworkIPAddress());
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
            return (match.Success && match.Value.Length > 0);
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
        private int m_idxProxyGroup;    // current proxy group index if Auto Mode
    }
}
