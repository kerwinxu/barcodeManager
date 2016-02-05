using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace BarcodeTerminator
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            if (args.Length > 0)
            {
                Application.Run(new FrmBarcodeEdit(args[0]));

            }
            else
            {
                Application.Run(new FrmBarcodeEdit());
            }
            
        }
    }
}
