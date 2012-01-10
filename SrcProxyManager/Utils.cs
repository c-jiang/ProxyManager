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

        public static long GetFileSize(string path)
        {
            long ret = -1;
            FileInfo fi = new FileInfo(path);
            if (fi.Exists) {
                ret = fi.Length;
            }
            return ret;
        }

        public static bool MoveFile(string oldPath, string newPath)
        {
            bool ret = false;
            FileInfo fi = new FileInfo(oldPath);
            if (fi.Exists) {
                fi.MoveTo(newPath);
                ret = true;
            }
            return ret;
        }

        public static bool RemoveFile(string path)
        {
            bool ret = false;
            FileInfo fi = new FileInfo(path);
            if (fi.Exists) {
                fi.Delete();
                ret = true;
            }
            return ret;
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

        public static bool Initialize(string newLogPath, string oldLogPath)
        {
            bool ret = false;
            if (m_semaphore == null) {
                m_semaphore = new Semaphore(1, 1);
                m_semaphore.WaitOne();
                m_newLogPath = newLogPath;
                m_oldLogPath = oldLogPath;
                if (m_logger == null) {
                    if (!File.Exists(m_newLogPath)) {
                        m_logger = new StreamWriter(m_newLogPath);
                    } else {
                        m_logger = File.AppendText(m_newLogPath);
                    }
                    m_logLevel = Category.NONE;
                }
                m_semaphore.Release();
                ret = true;
            }
            return ret;
        }

        public static bool Terminate()
        {
            if (m_semaphore == null) {
                return false;   // uninitialized
            }
            m_semaphore.WaitOne();
            if (m_logger != null) {
                m_logger.Close();
                m_logger = null;
            }
            m_semaphore.Release();
            return true;
        }

        public static bool Enable(Category logLevel)
        {
            if (m_semaphore == null) {
                return false;   // uninitialized
            }
            m_semaphore.WaitOne();
            m_logLevel = logLevel;
            m_semaphore.Release();
            return true;
        }

        public static bool Disable()
        {
            if (m_semaphore == null) {
                return false;   // uninitialized
            }
            m_semaphore.WaitOne();
            m_logLevel = Category.NONE;
            m_semaphore.Release();
            return true;
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
            string buf = sb.ToString().Trim();
            if (Utils.GetFileSize(m_newLogPath) + buf.Length + 2 > MAX_LOG_FILE_SIZE) {
                // 2 bytes is caused by the new line from 'WriteLine'.
                SwitchLogFile();
            }
            m_logger.WriteLine(buf);
            m_logger.Flush();
            m_semaphore.Release();
        }

        private static void SwitchLogFile()
        {
            m_logger.Close();
            Utils.RemoveFile(m_oldLogPath);
            Utils.MoveFile(m_newLogPath, m_oldLogPath);
            m_logger = new StreamWriter(m_newLogPath);
        }

        private static Category m_logLevel = Category.NONE;
        private static StreamWriter m_logger = null;
        private static Semaphore m_semaphore = null;

        private static string m_newLogPath = String.Empty;
        private static string m_oldLogPath = String.Empty;

        private static string[] m_separators = new string[] { Environment.NewLine };
        private static string m_blankPrefix = "                          "; // 26-chars
        private const long MAX_LOG_FILE_SIZE = 4194304;     // 4MB
    }
}
