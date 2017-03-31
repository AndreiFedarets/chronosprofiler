using System;
using System.Diagnostics;
using System.Threading;
using System.Windows.Forms;

namespace Chronos.Host.Application
{
    static class Program
    {
        private static IApplication _application;

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        private static void Main()
        {
            using (Process process = Process.GetCurrentProcess())
            {
                try
                {
                    string arguments = process.StartInfo.Arguments;
                    _application = ApplicationManager.RunInplace(true);
                    if (string.IsNullOrEmpty(arguments) || !arguments.Contains("-h"))
                    {
                        NotifyIcon notifyIcon = new NotifyIcon();
                        notifyIcon.Icon = Properties.Resources.Main;
                        notifyIcon.Visible = true;
                        MenuItem exitMenuItem = new MenuItem(Properties.Resources.CloseServerMenuItem_Content, OnExitClick);
                        notifyIcon.ContextMenu = new ContextMenu(new[] { exitMenuItem });
                    }
                    System.Windows.Forms.Application.Run();
                }
                catch (Exception exception)
                {
                    ReportFatalError(exception);
                }
            }
        }

        private static void OnExitClick(object sender, EventArgs eventArgs)
        {
            _application.Close();
        }

        private static void ReportFatalError(Exception exception)
        {
            LoggingProvider.Current.Log(TraceEventType.Critical, exception);
            Thread thread = new Thread(() =>
            {
                MessageBox.Show(string.Format("Unable to run application.{0}{1}", Environment.NewLine, exception), "Fatal Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Environment.Exit(0);
            });
            thread.SetApartmentState(ApartmentState.STA);
            thread.Start();
        }
    }
}
