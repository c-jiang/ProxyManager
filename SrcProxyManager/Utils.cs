using System;
using System.IO;
using System.Threading;
using System.Text;


namespace ProxyManager
{
    public class Utils
    {
        public static string GetDateTime()
        {
            return DateTime.Now.ToString(@"yyyy/MM/dd HH:mm:ss");
        }

        public static void RemoveFile(string path)
        {
            FileInfo fi = new FileInfo(path);
            if (fi.Exists) {
                fi.Delete();
            }
        }
    }

    public class Logger
    {
        public enum Category
        {
            NONE = 0,
            Error,
            Warning,
            Information,
            Verbose,
        }

        public static void Initialize(string logPath)
        {
            if (m_semaphore == null) {
                m_semaphore = new Semaphore(1, 1);
            }
            m_semaphore.WaitOne();
            if (m_logger == null) {
                if (!File.Exists(logPath)) {
                    m_logger = new StreamWriter(logPath);
                } else {
                    m_logger = File.AppendText(logPath);
                }
                m_logLevel = Category.NONE;
            }
            m_semaphore.Release();
        }

        public static void Terminate()
        {
            if (m_semaphore == null) {
                return; // uninitialized
            }
            m_semaphore.WaitOne();
            if (m_logger != null) {
                m_logger.Close();
                m_logger = null;
            }
            m_semaphore.Release();
        }

        public static void Enable(Category logLevel)
        {
            m_semaphore.WaitOne();
            m_logLevel = logLevel;
            m_semaphore.Release();
        }

        public static void Disable()
        {
            m_semaphore.WaitOne();
            m_logLevel = Category.NONE;
            m_semaphore.Release();
        }

        public static void E(string content)
        {
            if (m_logLevel >= Category.Error) {
                AddToLog('E', content);
            }
        }

        public static void W(string content)
        {
            if (m_logLevel >= Category.Warning) {
                AddToLog('W', content);
            }
        }

        public static void I(string content)
        {
            if (m_logLevel >= Category.Information) {
                AddToLog('I', content);
            }
        }

        public static void V(string content)
        {
            if (m_logLevel >= Category.Verbose) {
                AddToLog('V', content);
            }
        }

        private static void AddToLog(char category, string content)
        {
            if (m_semaphore == null) {
                return; // uninitialized
            }
            m_semaphore.WaitOne();
            if (m_logger == null) {
                return;
            }
            StringBuilder sb = new StringBuilder();
            sb.Append("[" + Utils.GetDateTime() + "] [" + category + "] ");
            string[] lines = content.Split(m_separators, StringSplitOptions.RemoveEmptyEntries);
            if (lines.Length <= 1) {
                sb.Append(content);
            } else {
                sb.Append(lines[0] + Environment.NewLine);
                for (int i = 1; i < lines.Length - 1; ++i) {
                    sb.Append(m_blankPrefix + lines[i] + Environment.NewLine);
                }
                sb.Append(m_blankPrefix + lines[lines.Length - 1]);
            }
            m_logger.WriteLine(sb.ToString().Trim());
            m_logger.Flush();
            m_semaphore.Release();
        }

        private static Category m_logLevel = Category.NONE;
        private static StreamWriter m_logger = null;
        private static Semaphore m_semaphore = null;
        private static string[] m_separators = new string[] { Environment.NewLine };
        private static string m_blankPrefix = "                          "; // 26-chars
    }
}
