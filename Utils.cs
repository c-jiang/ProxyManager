using System;
using System.IO;


namespace ProxyManager
{
    public class Utils
    {
        public static string GetDateTime()
        {
            return DateTime.Now.ToString(@"MM/dd HH:mm:ss");
        }

        public static void RemoveFile(string path)
        {
            FileInfo fi = new FileInfo(path);
            if (fi.Exists) {
                fi.Delete();
            }
        }
    }
}
