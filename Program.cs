using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace LoadBingPicture
{
    static class Program
    {
        private static System.Threading.Mutex m;

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            bool ok;
            m = new System.Threading.Mutex(true, "LoadBingPicture", out ok);

            if (!ok)
            {
                MessageBox.Show("Another instance is already running.");
                return;
            }

            Application.Run(new frmMain());
            GC.KeepAlive(m);    // important!
            
        }
    }
}
