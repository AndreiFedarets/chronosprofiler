using System;
using System.Diagnostics;
using System.Threading;
using System.Windows;

namespace Chronos.Client.Win.Application
{
    public partial class App
    {
        private IApplicationBase _application;

        public App()
        {
            ApplicationManager.Initialize();
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            SetupOutput();
            using (Process current = Process.GetCurrentProcess())
            {
                try
                {
                    string sessionUidString = current.StartInfo.EnvironmentVariables[Chronos.Constants.SessionUidEnvironmentVariableName];
                    if (string.IsNullOrWhiteSpace(sessionUidString))
                    {
                        _application = ApplicationManager.Main.RunInplace(true);
                    }
                    else
                    {
                        Guid sessionUid = new Guid(sessionUidString);
                        _application = ApplicationManager.Profiling.RunInplace(sessionUid, true);
                    }
                }
                catch (Exception exception)
                {
                    ReportFatalError(exception);
                }
            }
            base.OnStartup(e);
        }

        private void SetupOutput()
        {
            PresentationTraceSources.Refresh();
            PresentationTraceSources.DataBindingSource.Listeners.Add(new ConsoleTraceListener());
            PresentationTraceSources.DataBindingSource.Listeners.Add(new DefaultTraceListener());
            PresentationTraceSources.DataBindingSource.Switch.Level = SourceLevels.Warning | SourceLevels.Error;
        }

        protected override void OnExit(ExitEventArgs e)
        {
            _application.Close();
            base.OnExit(e);
        }

        private void ReportFatalError(Exception exception)
        {
            LoggingProvider.Current.Log(TraceEventType.Critical, exception);
            Thread thread = new Thread(() =>
            {
                MessageBox.Show(string.Format("Unable to run application.{0}{1}", Environment.NewLine, exception), "Fatal Error", MessageBoxButton.OK, MessageBoxImage.Error);
                Environment.Exit(0);
            });
            thread.SetApartmentState(ApartmentState.STA);
            thread.Start();
        }
    }
}
