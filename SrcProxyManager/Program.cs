using System;
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
                bool createdNew;
                Mutex instance = new Mutex(true,
                    System.Diagnostics.Process.GetCurrentProcess().ProcessName,
                    out createdNew);

                if (createdNew) {
                    AppManager appManager = new AppManager();

                    if (!appManager.LoadAppEnvironment()) {
                        MessageBox.Show(
                            @"'" + AppManager.PROXY_AGENT_FILE_NAME + "' is missing."
                            + Environment.NewLine
                            + @"Failed to launch " + AppManager.ASSEMBLY_PRODUCT + @".",
                            AppManager.ASSEMBLY_PRODUCT,
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                    } else {
                        Application.EnableVisualStyles();
                        Application.SetCompatibleTextRenderingDefault(false);
                        if (!appManager.LoadAppProfile()) {
                            DialogResult dr = MessageBox.Show(
                                @"New profile '" + Profile.PROFILE_FILE_NAME
                                + @"' has been created successfully."
                                + Environment.NewLine
                                + @"It is strongly recommended to set the options before using "
                                + AppManager.ASSEMBLY_PRODUCT + @"."
                                + Environment.NewLine
                                + @"Would you like to set the options right now?",
                                AppManager.ASSEMBLY_PRODUCT,
                                MessageBoxButtons.YesNo,
                                MessageBoxIcon.Question);

                            if (dr == DialogResult.Yes) {
                                if (DlgOptions.Instance.ShowDialog(
                                        appManager.AppProfile) == DialogResult.OK) {
                                    Profile.Save(DlgOptions.DlgProfile);
                                    appManager.LoadAppProfile();
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
                        @"ProxyManager is already running.",
                        AppManager.ASSEMBLY_PRODUCT,
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                }
            } catch (Exception x) {
                MessageBox.Show(
                        x.Message + Environment.NewLine + x.StackTrace,
                        AppManager.ASSEMBLY_PRODUCT,
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
            }
        }
    }
}
