using System;
using System.Globalization;
using System.Windows.Forms;
using System.Threading;
using MGV.RocRailStartup.Forms;
using MGV.RocRailStartup.Preferences;

namespace MGV.RocRailStartup
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // Setup language
            try
            {
                var culture = CultureInfo.GetCultureInfo(UserPreferences.Preferences.Language);
                if (culture != null)
                {
                    Thread.CurrentThread.CurrentCulture = culture;
                    Thread.CurrentThread.CurrentUICulture = culture;
                }
            }
            catch
            {
                // Ignore
            }



            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm());
        }
    }
}
