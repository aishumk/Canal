﻿using Logging;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Threading;
using System.Windows.Forms;

namespace Canal
{
    static class Program
    {
        // ReSharper disable once UnusedMember.Local Object has to stay alive
        private static ConsoleLogger _log = new ConsoleLogger();

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            try
            {
                Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;
                Thread.CurrentThread.CurrentUICulture = CultureInfo.InvariantCulture;

                File.AppendAllLines("canal.log", new List<string> { DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss ") + "Program start." });
                Logger.Singleton.Errors += (sender, eventArgs) => File.AppendAllLines("canal.log", new List<string> { DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss ") + eventArgs.Message });

                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new MainWindow(args));
            }
            catch (Exception exception)
            {
                Logger.Error(exception, "Error in application.");
                MessageBox.Show(string.Format("An error occured: {0}.\n\nStack Trace:\n{1}", exception.Message, exception.StackTrace));
            }
            finally
            {
                File.AppendAllLines("canal.log", new List<string> { DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss ") + "Program end." });
            }
        }
    }
}
