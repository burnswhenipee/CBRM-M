using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace CbrConverter
{
    static class Program
    {
        /// <summary>
        /// The program icon, used by every window
        /// </summary>
        public static System.Drawing.Icon AppIcon
        {
            get
            {
                using (var stream = typeof(Program).Assembly.GetManifestResourceStream("CbrConverter.cbrmm.ico"))
                    return new System.Drawing.Icon(stream);
            }
        }

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            Application.SetHighDpiMode(HighDpiMode.SystemAware);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm(args.Length > 0 ? args[0] : null));
        }
    }
}
