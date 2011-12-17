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
                        + @"Failed to launch " + ASSEMBLY_PRODUCT + @".",
                        ASSEMBLY_PRODUCT,
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                } else {
                    Application.EnableVisualStyles();
                    Application.SetCompatibleTextRenderingDefault(false);
                    if (!appManager.LoadAppProfile()) {
                        DialogResult dr = MessageBox.Show(
                            @"New profile '" + Profile.PROFILE_FILE_NAME
                            + @"' has been created since there was no profile."
                            + Environment.NewLine
                            + @"It is strongly recommended to set the options before using "
                            + ASSEMBLY_PRODUCT + @"."
                            + Environment.NewLine
                            + @"Would you like to set the options right now?",
                            ASSEMBLY_PRODUCT,
                            MessageBoxButtons.YesNo,
                            MessageBoxIcon.Question);

                        if (dr == DialogResult.Yes) {
                            // TODO: Add the entry to Options dialog.
                            MessageBox.Show(
                                @"The entry to Options is not implemented.",
                                ASSEMBLY_PRODUCT,
                                MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                    Application.Run(new FormMain(appManager));
                }

                instance.ReleaseMutex();
            } else {
                MessageBox.Show(
                    @"ProxyManager is already running.",
                    ASSEMBLY_PRODUCT,
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        // Refer to [assembly: AssemblyProduct]
        private const string ASSEMBLY_PRODUCT = "Proxy Manager";
    }
}
