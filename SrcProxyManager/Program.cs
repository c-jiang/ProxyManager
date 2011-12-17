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

                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new FormMain(appManager));
                
                instance.ReleaseMutex();
            } else {
                MessageBox.Show(
                    "Error: ProxyManager is already running.",
                    "Proxy Manager",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }
    }
}
