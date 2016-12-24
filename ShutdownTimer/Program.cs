using System;
using System.Windows.Forms;

namespace ShutdownTimer
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        private static void Main()
        {

            if (!SingleInstance.Start())
            {
                SingleInstance.ShowFirstInstance();
                return;
            }
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            try
            {
                Form1 main_form = new Form1();
                Application.Run(main_form);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
            SingleInstance.Stop();
        }
    }
}
