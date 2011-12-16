using System;


namespace ProxyAgent
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length < 1) {
                Console.Error.WriteLine("Error: Incorrect parameter(s).");
                return;
            }

            int index = 0;
            bool bEnableProxy = Boolean.Parse(args[index++]);
            if (bEnableProxy) {
                // Case - Enable Proxy
                if (args.Length < 4) {
                    Console.Error.WriteLine("Error: Incorrect parameter(s).");
                    return;
                }
                string szProxyAddr = args[index++];
                string szBypass = args[index++];
                bool bDisableAutoConf = Boolean.Parse(args[index++]);

                // Set Proxy
                if (bDisableAutoConf) {
                    IeProxyOptions.DisableAutoConf();
                }
                IeProxyOptions.ProxyEnable = true;
                IeProxyOptions.ProxyAddr = szProxyAddr;
                IeProxyOptions.Bypass = szBypass;
                IeProxyOptions.CommitChange();
            } else {
                // Case - Disable Proxy
                IeProxyOptions.ProxyEnable = false;
                IeProxyOptions.CommitChange();
            }
        }
    }
}
