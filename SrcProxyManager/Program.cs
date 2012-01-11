using System;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Windows.Forms;


namespace ProxyManager
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            try {
                string path = Process.GetCurrentProcess().MainModule.FileName;
                path = Path.GetDirectoryName(path);

                bool createdNew;
                Mutex instance = new Mutex(true,
                    Process.GetCurrentProcess().ProcessName, out createdNew);

                if (createdNew) {
                    Logger.Initialize(
                        Path.Combine(path, AppManager.APP_NEW_LOG_FILE_NAME),
                        Path.Combine(path, AppManager.APP_OLD_LOG_FILE_NAME));
                    AppManager appManager = new AppManager(path);

                    if (!appManager.LoadAppEnvironment()) {
                        string msg = @"'" + AppManager.PROXY_AGENT_FILE_NAME
                            + "' is missing." + Environment.NewLine
                            + @"Failed to launch " + AppManager.ASSEMBLY_PRODUCT + @".";
                        Logger.E(msg);
                        MessageBox.Show(msg,
                            AppManager.ASSEMBLY_PRODUCT,
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                    } else {
                        Application.EnableVisualStyles();
                        Application.SetCompatibleTextRenderingDefault(false);
                        if (appManager.LoadAppProfile()) {
                            string msg = @"New profile '" + Profile.PROFILE_FILE_NAME
                                + @"' has been created successfully.";
                            Logger.I(msg);
                            DialogResult dr = MessageBox.Show(msg
                                + Environment.NewLine
                                + @"It is strongly recommended to set the options before using "
                                + AppManager.ASSEMBLY_PRODUCT + @"."
                                + Environment.NewLine
                                + @"Would you like to set the options right now?",
                                AppManager.ASSEMBLY_PRODUCT,
                                MessageBoxButtons.YesNo,
                                MessageBoxIcon.Question);

                            if (dr == DialogResult.Yes) {
                                dr = DlgOptions.Instance.ShowDialog(
                                    appManager.AppProfile);
                                if ((dr == DialogResult.OK) &&
                                        (!appManager.AppProfile.Equals(DlgOptions.DlgProfile))) {
                                    appManager.AppProfile = new Profile(DlgOptions.DlgProfile);
                                    Profile.Save(DlgOptions.DlgProfile);
                                }
                            }
                        }
                        if (!appManager.IsLoadAppProfileFailed()) {
                            Application.Run(new FormMain(appManager));
                        }
                    }

                    instance.ReleaseMutex();
                } else {
                    MessageBox.Show(
                        "ProxyManager is already running.",
                        AppManager.ASSEMBLY_PRODUCT,
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                }
            } catch (Exception x) {
                string msg = x.Message + Environment.NewLine + x.StackTrace;
                Logger.E(msg);
                MessageBox.Show(msg,
                        AppManager.ASSEMBLY_PRODUCT,
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
            } finally {
                Logger.Terminate();
            }
        }
    }
}
