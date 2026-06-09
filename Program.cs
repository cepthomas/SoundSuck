using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;


namespace SoundSuck
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            // Handle unexpected exceptions.
            Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);
            Application.ThreadException += (sender, e) => { HandleException(e.Exception, "UI Thread Exception"); };
            AppDomain.CurrentDomain.UnhandledException += (sender, e) => { HandleException((Exception)e.ExceptionObject, "Background Thread Exception"); };

            Application.SetHighDpiMode(HighDpiMode.SystemAware);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            try
            {
                var host = new MainForm();
                Application.Run(host);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "!!!");
                Environment.Exit(1);
            }
        }

        static void HandleException(Exception ex, string type)
        {
            MessageBox.Show(ex.ToString(), type);
            Environment.Exit(1);
        }
    }
}
