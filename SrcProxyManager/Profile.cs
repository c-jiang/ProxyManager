using System;
using System.Collections.Generic;
using System.IO;
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
        public WorkMode m_workMode;
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
            m_workMode = WorkMode.Direct;
            m_isStartAuto = true;
            m_isStartMinimized = true;
            m_isLogToFile = false;
            m_listProxyGroups = null;
        }

        // Method: Load from local profile
        public static Profile Load(string appDir, out bool createdNew)
        {
            Profile profile = null;
            string profilePath = Path.Combine(appDir, PROFILE_FILE_NAME);
            if (File.Exists(profilePath)) {
                XmlSerializer xs = new XmlSerializer(typeof(Profile));
                StreamReader reader = new StreamReader(profilePath);
                profile = (Profile)xs.Deserialize(reader.BaseStream);
                // TODO: deserialization may cause exception
                profile.m_szProfilePath = profilePath;
                reader.Close();
                createdNew = false;
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

        private string m_szProfilePath;
    }

    public class ProxyGroup
    {
        [XmlAttribute("Name")]
        public string m_szName;
        [XmlAttribute("Enable")]
        public bool m_isEnabled;
        [XmlElement("ProxyItemId")]
        public List<ProxyItem> m_listProxyItems;
        [XmlElement("ApplyRule")]
        public ApplyRule m_applyRule;

        public ProxyGroup()
        {
            m_szName = String.Empty;
            m_isEnabled = false;
            m_listProxyItems = new List<ProxyItem>();
            m_applyRule = null;
        }

        public ProxyGroup(string name, bool isEnabled, List<ProxyItem> listProxyItems)
        {
            m_szName = name;
            m_isEnabled = isEnabled;
            m_listProxyItems = new List<ProxyItem>(listProxyItems);
            m_applyRule = null;
        }

        public ProxyGroup(string name, bool isEnabled, List<ProxyItem> listProxyItems, ApplyRule ruleCondition)
        {
            m_szName = name;
            m_isEnabled = isEnabled;
            m_listProxyItems = new List<ProxyItem>(listProxyItems);
            m_applyRule = new ApplyRule(ruleCondition);
        }
    }

    public class ProxyItem
    {
        [XmlAttribute("Id")]
        public int m_iId;
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
            m_iId = 0;
            m_isEnabled = false;
            m_szProxyAddr = String.Empty;
            m_szBypass = String.Empty;
            m_isAutoConfDisabled = true;
        }

        public ProxyItem(int id, bool isEnabled, string proxyAddr, string bypass)
        {
            m_iId = id;
            m_isEnabled = isEnabled;
            m_szProxyAddr = proxyAddr;
            m_szBypass = bypass;
            m_isAutoConfDisabled = true;
        }

        public ProxyItem(int id, bool isEnabled, string proxyAddr, string bypass, bool isAutoConfDisabled)
        {
            m_iId = id;
            m_isEnabled = isEnabled;
            m_szProxyAddr = proxyAddr;
            m_szBypass = bypass;
            m_isAutoConfDisabled = isAutoConfDisabled;
        }
    }

    public class ApplyRule
    {
        [XmlAttribute("IpAddrFilter")]
        public bool m_bIpAddrFilter;
        [XmlElement("IpAddrFilterExpression")]
        public string m_szIpAddrFilter;
        [XmlAttribute("DnsSuffixFilter")]
        public bool m_bDnsSuffixFilter;
        [XmlElement("DnsSuffixFilterExpression")]
        public string m_szDnsSuffixFilter;

        public ApplyRule()
        {
            m_bIpAddrFilter = false;
            m_szIpAddrFilter = String.Empty;
            m_bDnsSuffixFilter = false;
            m_szDnsSuffixFilter = String.Empty;
        }

        public ApplyRule(ApplyRule rc)
        {
            m_bIpAddrFilter = rc.m_bIpAddrFilter;
            m_szIpAddrFilter = rc.m_szIpAddrFilter;
            m_bDnsSuffixFilter = rc.m_bDnsSuffixFilter;
            m_szDnsSuffixFilter = rc.m_szDnsSuffixFilter;
        }

        public ApplyRule(bool bIpAddrFilter, string szIpAddrFilter, bool bDnsSuffixFilter, string szDnsSuffixFilter)
        {
            m_bIpAddrFilter = bIpAddrFilter;
            m_szIpAddrFilter = szIpAddrFilter;
            m_bDnsSuffixFilter = bDnsSuffixFilter;
            m_szDnsSuffixFilter = szDnsSuffixFilter;
        }
    }
}
