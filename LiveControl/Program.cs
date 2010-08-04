using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;



namespace Kevsoft.LiveControl
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Form frm = new MainForm();
            Application.Run(frm);
            //Stop the movement Thread
            Show.Instance.FixPatch.StopShapeMovementsThread();
            frm = null;
        }
    }
}
