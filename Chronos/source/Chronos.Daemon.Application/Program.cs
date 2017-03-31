using System;
using System.Diagnostics;
using System.Threading;
using System.Windows.Forms;

namespace Chronos.Daemon.Application
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            using (Process current = Process.GetCurrentProcess())
            {
                try
                {
                    string sessionUidString = current.StartInfo.EnvironmentVariables[Constants.SessionUidEnvironmentVariableName];
                    Guid sessionUid = new Guid(sessionUidString);
                    if (sessionUid == Guid.Empty)
                    {
                        return;
                    }
                    ApplicationManager.RunInplace(true, sessionUid);
                    System.Windows.Forms.Application.Run();
                }
                catch (Exception exception)
                {
                    ReportFatalError(exception);
                }
            }
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
