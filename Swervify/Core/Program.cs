using System;
using System.Windows.Forms;
using Swervify.UI;

namespace Swervify.Core
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            FormMain formMain = null;
            if (args.Length == 1)
                if(args[0]=="-startup")
                    formMain = new FormMain(true);

            Application.Run(formMain?? new FormMain());
        }
    }
}
