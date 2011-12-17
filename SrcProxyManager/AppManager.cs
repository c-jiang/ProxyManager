﻿using System;
using System.Diagnostics;
using System.Net.NetworkInformation;
using System.IO;
using System.Text.RegularExpressions;


namespace ProxyManager
{
    public class AppManager
    {
        public const string PROXY_AGENT_FILE_NAME = "ProxyAgent.exe";

        public AppManager()
        {
            string path = Process.GetCurrentProcess().MainModule.FileName;
            m_szAppDir = Path.GetDirectoryName(path);
            m_detector = new NetworkDetector();
            m_profile = null;
        }

        public bool InitAppEnvironment()
        {
            string path = Path.Combine(m_szAppDir, PROXY_AGENT_FILE_NAME);
            return File.Exists(path);
        }

        public bool InitAppProfile()
        {
            bool createdNew;
            m_profile = Profile.Load(m_szAppDir, out createdNew);
            if (createdNew) {
                return false;
            } else {
                if (m_profile.m_workMode == WorkMode.Auto) {
                    RegisterLowLevelCallbacks();
                }
                return true;
            }
        }


        public delegate void NotifyGuiNetworkChanged(object sender, EventArgs e);
        public delegate void NotifyGuiProxyChanged(object sender, EventArgs e);
        public delegate void NotifyGuiNetworkAndProxyChanged(object sender, EventArgs e);
        public event NotifyGuiNetworkChanged NetworkChanged;
        public event NotifyGuiProxyChanged ProxyChanged;
        public event NotifyGuiNetworkAndProxyChanged NetworkAndProxyChanged;

        public void NotificationNetworkChanged(object sender, EventArgs e)
        {
            bool bChanged = AutoSwitchProxy();

            // GUI notifications
            if (bChanged) {
                // case - proxy settings need to be changed
                NetworkAndProxyChanged(this, new EventArgs());
            } else {
                // case - no proxy settings changed
                NetworkChanged(this, new EventArgs());
            }
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

            ProxyChanged(this, new EventArgs());
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

            ProxyChanged(this, new EventArgs());
        }

        public void DisableProxy()
        {
            Process process = PrepareProxyAgentProcess();
            SetArgsProxyAgentProcess(process, false.ToString());
            ExecuteProxyAgentProcess(process);

            ProxyChanged(this, new EventArgs());
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

        public void UserChangeWorkMode(WorkMode newMode)
        {
            WorkMode oldMode = m_profile.m_workMode;
            if (oldMode != newMode) {
                m_profile.m_workMode = newMode;
                Profile.Save(m_profile);

                if (oldMode == WorkMode.Auto) {
                    DeregisterLowLevelCallbacks();
                } else if (newMode == WorkMode.Auto) {
                    RegisterLowLevelCallbacks();
                }
            }
        }

        private ProxyItem FindMatchedProxyItem()
        {
            ProxyItem ret = null;
            foreach (ProxyGroup pg in m_profile.m_listProxyGroups) {
                if (!pg.m_isEnabled) {
                    continue;
                }
                if (!IsRuleMatched(pg.m_applyRule)) {
                    continue;
                }
                foreach (ProxyItem pi in pg.m_listProxyItems) {
                    if (pi.m_isEnabled) {
                        ret = pi;
                        break;
                    }
                }
                if (ret != null) {
                    break;
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

        private void RegisterLowLevelCallbacks()
        {
            // Link NetworkDetector.NetworkChanged to AppManager
            m_detector.NetworkChanged +=
                new NetworkDetector.NotifyAppManagerNetworkChanged(
                    this.NotificationNetworkChanged);

            // Link system NetworkChange.NetworkAddressChanged to NetworkDetector
            NetworkChange.NetworkAddressChanged +=
                new NetworkAddressChangedEventHandler(
                    m_detector.NetworkAddressChangedCallback);
        }

        private void DeregisterLowLevelCallbacks()
        {
            // Remove link between NetworkDetector.NetworkChanged and AppManager
            m_detector.NetworkChanged -=
                new NetworkDetector.NotifyAppManagerNetworkChanged(
                    this.NotificationNetworkChanged);

            // Remove link between system NetworkChange.NetworkAddressChanged and NetworkDetector
            NetworkChange.NetworkAddressChanged -=
                new NetworkAddressChangedEventHandler(
                    m_detector.NetworkAddressChangedCallback);
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
        private Profile m_profile;
        private NetworkDetector m_detector;
    }
}