using System;
using System.Collections.Generic;
using System.Linq;
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
            AppManager appManager = new AppManager();

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new FormMain(appManager));
        }
    }
}
