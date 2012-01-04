using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using System.Xml.Serialization;


namespace ProxyManager
{
    public enum WorkMode
    {
        Direct = 0,
        Proxy,
        Auto
    }


    [XmlInclude(typeof(ProxyGroup))]
    [XmlInclude(typeof(ProxyItem))]
    [XmlInclude(typeof(ApplyRule))]
    public class Profile
    {
        public const string PROFILE_FILE_NAME = "ProxyManagerProfile.xml";

        // settings - options
        [XmlElement("WorkMode")]
        public WorkMode m_defWorkMode;
        [XmlElement("StartAuto")]
        public bool m_isStartAuto;
        [XmlElement("StartMinimized")]
        public bool m_isStartMinimized;
        [XmlElement("LogToFile")]
        public bool m_isLogToFile;

        // settings - proxy group list
        [XmlArrayAttribute("ProxyGroupList")]
        public List<ProxyGroup> m_listProxyGroups;

        // Contructor Method
        public Profile()
        {
            m_defWorkMode = WorkMode.Auto;
            m_isStartAuto = true;
            m_isStartMinimized = true;
            m_isLogToFile = false;
            m_listProxyGroups = null;

            m_szProfilePath = String.Empty;
            s_bLoadFailed = false;
        }

        public Profile(Profile profile)
        {
            m_defWorkMode = profile.m_defWorkMode;
            m_isStartAuto = profile.m_isStartAuto;
            m_isStartMinimized = profile.m_isStartMinimized;
            m_isLogToFile = profile.m_isLogToFile;
            if (profile.m_listProxyGroups != null) {
                m_listProxyGroups = new List<ProxyGroup>(profile.m_listProxyGroups);
            } else {
                m_listProxyGroups = null;
            }
            m_szProfilePath = profile.m_szProfilePath;
            s_bLoadFailed = false;
        }

        public override bool Equals(object obj)
        {
            if (!(obj is Profile)) {
                return false;
            }

            var profile = obj as Profile;
            if (profile == null) {
                return false;
            }

            if (m_defWorkMode != profile.m_defWorkMode) {
                return false;
            }
            if (m_isStartAuto != profile.m_isStartAuto) {
                return false;
            }
            if (m_isStartMinimized != profile.m_isStartMinimized) {
                return false;
            }
            if (m_szProfilePath != profile.m_szProfilePath) {
                return false;
            }

            if (m_listProxyGroups != null && profile.m_listProxyGroups != null) {
                if (m_listProxyGroups.Count != profile.m_listProxyGroups.Count) {
                    return false;
                }
                for (int i = 0; i < m_listProxyGroups.Count; ++i) {
                    if (!m_listProxyGroups[i].Equals(profile.m_listProxyGroups[i])) {
                        return false;
                    }
                }
            } else if (m_listProxyGroups != null || profile.m_listProxyGroups != null) {
                return false;
            }

            return true;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        // Method: Load from local profile
        public static Profile Load(string appDir, out bool createdNew)
        {
            Profile profile = null;
            string profilePath = Path.Combine(appDir, PROFILE_FILE_NAME);
            if (File.Exists(profilePath)) {
                XmlSerializer xs = new XmlSerializer(typeof(Profile));
                StreamReader reader = new StreamReader(profilePath);
                createdNew = false;
                try {
                    profile = (Profile)xs.Deserialize(reader.BaseStream);
                    profile.m_szProfilePath = profilePath;
                    reader.Close();
                } catch (Exception) {
                    reader.Close();
                    DialogResult dr = MessageBox.Show(
                        "Error occurs in loading the profile."
                            + Environment.NewLine + Environment.NewLine
                            + "- Press 'Yes' to load the default profile settings, "
                            + "but user settings will be lost."
                            + Environment.NewLine
                            + "- Press 'No' to exit for manually fixing the error in the editor.",
                        AppManager.ASSEMBLY_PRODUCT,
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Error);
                    if (dr == DialogResult.Yes) {
                        // create an initial profile
                        profile = new Profile();
                        profile.m_szProfilePath = profilePath;
                        Save(profile);
                        createdNew = true;
                    } else {
                        // exit application
                        s_bLoadFailed = true;
                        Application.Exit();
                    }
                }
            } else {
                profile = new Profile();
                profile.m_szProfilePath = profilePath;
                Save(profile);
                createdNew = true;
            }
            return profile;
        }

        // Method: Save to local profile
        public static void Save(Profile profile)
        {
            XmlSerializer xs = new XmlSerializer(typeof(Profile));
            StreamWriter writer = new StreamWriter(profile.m_szProfilePath, false);
            try {
                xs.Serialize(writer.BaseStream, profile);
            } catch (Exception x) {
                System.Diagnostics.Debug.WriteLine(x.Message);
            }
            writer.Close();
        }

        public static bool IsLoadFailed()
        {
            return s_bLoadFailed;
        }

        private string m_szProfilePath;
        private static bool s_bLoadFailed;
    }

    public class ProxyGroup
    {
        [XmlAttribute("Name")]
        public string m_szName;
        [XmlAttribute("Enable")]
        public bool m_isEnabled;
        [XmlAttribute("SelectedIndex")]
        public int m_iSelectedIndex;
        [XmlElement("ProxyItem")]
        public List<ProxyItem> m_listProxyItems;
        [XmlElement("ApplyRule")]
        public ApplyRule m_applyRule;

        public ProxyGroup()
        {
            m_szName = String.Empty;
            m_isEnabled = false;
            m_iSelectedIndex = 0;
            m_listProxyItems = new List<ProxyItem>();
            m_applyRule = new ApplyRule();
        }

        public ProxyGroup(ProxyGroup pg)
        {
            m_szName = pg.m_szName;
            m_isEnabled = pg.m_isEnabled;
            m_iSelectedIndex = pg.m_iSelectedIndex;
            m_applyRule = new ApplyRule(pg.m_applyRule);
            if (pg.m_listProxyItems != null) {
                m_listProxyItems = new List<ProxyItem>(pg.m_listProxyItems);
            } else {
                m_listProxyItems = null;
            }
        }

        public ProxyGroup(string name, bool isEnabled, List<ProxyItem> listProxyItems)
        {
            m_szName = name;
            m_isEnabled = isEnabled;
            m_iSelectedIndex = 0;
            m_listProxyItems = new List<ProxyItem>(listProxyItems);
            m_applyRule = new ApplyRule();
        }

        public ProxyGroup(string name, bool isEnabled, List<ProxyItem> listProxyItems, ApplyRule ruleCondition)
        {
            m_szName = name;
            m_isEnabled = isEnabled;
            m_iSelectedIndex = 0;
            m_listProxyItems = new List<ProxyItem>(listProxyItems);
            m_applyRule = new ApplyRule(ruleCondition);
        }

        public override bool Equals(object obj)
        {
            if (!(obj is ProxyGroup)) {
                return false;
            }

            var pg = obj as ProxyGroup;
            if (pg == null) {
                return false;
            }

            if (m_szName != pg.m_szName) {
                return false;
            }
            if (m_isEnabled != pg.m_isEnabled) {
                return false;
            }
            if (m_iSelectedIndex != pg.m_iSelectedIndex) {
                return false;
            }
            if (!m_applyRule.Equals(pg.m_applyRule)) {
                return false;
            }

            if (m_listProxyItems != null && pg.m_listProxyItems != null) {
                if (m_listProxyItems.Count != pg.m_listProxyItems.Count) {
                    return false;
                }
                for (int i = 0; i < m_listProxyItems.Count; ++i) {
                    if (!m_listProxyItems[i].Equals(pg.m_listProxyItems[i])) {
                        return false;
                    }
                }
            } else if (m_listProxyItems != null || pg.m_listProxyItems != null) {
                return false;
            }

            return true;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }

    public class ProxyItem
    {
        [XmlAttribute("Enable")]
        public bool m_isEnabled;

        [XmlElement("ProxyAddr")]
        public string m_szProxyAddr;
        [XmlElement("Bypass")]
        public string m_szBypass;
        [XmlElement("DisableAutoConf")]
        public bool m_isAutoConfDisabled;

        public ProxyItem()
        {
            m_isEnabled = false;
            m_szProxyAddr = String.Empty;
            m_szBypass = String.Empty;
            m_isAutoConfDisabled = true;
        }

        public ProxyItem(bool isEnabled, string proxyAddr, string bypass)
        {
            m_isEnabled = isEnabled;
            m_szProxyAddr = proxyAddr;
            m_szBypass = bypass;
            m_isAutoConfDisabled = true;
        }

        public ProxyItem(bool isEnabled, string proxyAddr, string bypass, bool isAutoConfDisabled)
        {
            m_isEnabled = isEnabled;
            m_szProxyAddr = proxyAddr;
            m_szBypass = bypass;
            m_isAutoConfDisabled = isAutoConfDisabled;
        }

        public override bool Equals(object obj)
        {
            if (!(obj is ProxyItem)) {
                return false;
            }

            var pi = obj as ProxyItem;
            if (pi == null) {
                return false;
            }

            if (m_isEnabled != pi.m_isEnabled) {
                return false;
            }
            if (m_szProxyAddr != pi.m_szProxyAddr) {
                return false;
            }
            if (m_szBypass != pi.m_szBypass) {
                return false;
            }
            if (m_isAutoConfDisabled != pi.m_isAutoConfDisabled) {
                return false;
            }
            return true;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }

    public class ApplyRule
    {
        [XmlAttribute("IdFilter")]
        public bool m_bIdFilter;
        [XmlElement("IdFilterExpression")]
        public string m_szIdFilter;

        [XmlAttribute("NameFilter")]
        public bool m_bNameFilter;
        [XmlElement("NameFilterExpression")]
        public string m_szNameFilter;

        [XmlAttribute("IpAddrFilter")]
        public bool m_bIpAddrFilter;
        [XmlElement("IpAddrFilterExpression")]
        public string m_szIpAddrFilter;

        [XmlAttribute("SubMaskFilter")]
        public bool m_bSubMaskFilter;
        [XmlElement("SubMaskFilterExpression")]
        public string m_szSubMaskFilter;

        [XmlAttribute("GatewayFilter")]
        public bool m_bGatewayFilter;
        [XmlElement("GatewayFilterExpression")]
        public string m_szGatewayFilter;

        [XmlAttribute("DnsAddrFilter")]
        public bool m_bDnsAddrFilter;
        [XmlElement("DnsAddrFilterExpression")]
        public string m_szDnsAddrFilter;

        [XmlAttribute("DnsSuffixFilter")]
        public bool m_bDnsSuffixFilter;
        [XmlElement("DnsSuffixFilterExpression")]
        public string m_szDnsSuffixFilter;

        public ApplyRule()
        {
            m_bIdFilter = false;
            m_szIdFilter = String.Empty;
            m_bNameFilter = false;
            m_szNameFilter = String.Empty;
            m_bIpAddrFilter = false;
            m_szIpAddrFilter = String.Empty;
            m_bSubMaskFilter = false;
            m_szSubMaskFilter = String.Empty;
            m_bGatewayFilter = false;
            m_szGatewayFilter = String.Empty;
            m_bDnsAddrFilter = false;
            m_szDnsAddrFilter = String.Empty;
            m_bDnsSuffixFilter = false;
            m_szDnsSuffixFilter = String.Empty;
        }

        public ApplyRule(ApplyRule ar)
        {
            m_bIdFilter = ar.m_bIdFilter;
            m_szIdFilter = ar.m_szIdFilter;
            m_bNameFilter = ar.m_bNameFilter;
            m_szNameFilter = ar.m_szNameFilter;
            m_bIpAddrFilter = ar.m_bIpAddrFilter;
            m_szIpAddrFilter = ar.m_szIpAddrFilter;
            m_bSubMaskFilter = ar.m_bSubMaskFilter;
            m_szSubMaskFilter = ar.m_szSubMaskFilter;
            m_bGatewayFilter = ar.m_bGatewayFilter;
            m_szGatewayFilter = ar.m_szGatewayFilter;
            m_bDnsAddrFilter = ar.m_bDnsAddrFilter;
            m_szDnsAddrFilter = ar.m_szDnsAddrFilter;
            m_bDnsSuffixFilter = ar.m_bDnsSuffixFilter;
            m_szDnsSuffixFilter = ar.m_szDnsSuffixFilter;
        }

        public override bool Equals(object obj)
        {
            if (!(obj is ApplyRule)) {
                return false;
            }

            var ar = obj as ApplyRule;
            if (ar == null) {
                return false;
            }

            if (m_bIdFilter != ar.m_bIdFilter) {
                return false;
            }
            if (m_szIdFilter != ar.m_szIdFilter) {
                return false;
            }
            if (m_bNameFilter != ar.m_bNameFilter) {
                return false;
            }
            if (m_szNameFilter != ar.m_szNameFilter) {
                return false;
            }
            if (m_bIpAddrFilter != ar.m_bIpAddrFilter) {
                return false;
            }
            if (m_szIpAddrFilter != ar.m_szIpAddrFilter) {
                return false;
            }
            if (m_bSubMaskFilter != ar.m_bSubMaskFilter) {
                return false;
            }
            if (m_szSubMaskFilter != ar.m_szSubMaskFilter) {
                return false;
            }
            if (m_bGatewayFilter != ar.m_bGatewayFilter) {
                return false;
            }
            if (m_szGatewayFilter != ar.m_szGatewayFilter) {
                return false;
            }
            if (m_bDnsAddrFilter != ar.m_bDnsAddrFilter) {
                return false;
            }
            if (m_szDnsAddrFilter != ar.m_szDnsAddrFilter) {
                return false;
            }
            if (m_bDnsSuffixFilter != ar.m_bDnsSuffixFilter) {
                return false;
            }
            if (m_szDnsSuffixFilter != ar.m_szDnsSuffixFilter) {
                return false;
            }
            return true;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
