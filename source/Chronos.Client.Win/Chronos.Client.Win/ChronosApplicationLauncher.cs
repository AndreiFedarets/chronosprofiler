using System;
using System.Threading;
using System.Windows;

namespace Chronos.Client.Win
{
    public sealed class ChronosApplicationLauncher<T> : RemoteBaseObject, IDisposable where T : ChronosApplication
    {
        private readonly ManualResetEvent _applicationReadyEvent;
        private ChronosApplication _application;
        private object[] _applicationArguments;

        public ChronosApplicationLauncher()
        {
            _applicationReadyEvent = new ManualResetEvent(false);
        }

        public void Run(params object[] args)
        {
            _applicationArguments = args;
            Thread thread = new Thread(RunWindowsApplication);
            thread.SetApartmentState(ApartmentState.STA);
            thread.Start();
            _applicationReadyEvent.WaitOne();
        }

        public ChronosApplication GetApplication()
        {
            return _application;
        }

        public override object InitializeLifetimeService()
        {
            return null;
        }

        public override void Dispose()
        {
            base.Dispose();
            _application.Close();
        }

        private void RunWindowsApplication()
        {
            Application application = new Application();
            application.Startup += OnWindowsApplicationStartup;
            application.Run();
        }

        private void OnWindowsApplicationStartup(object sender, StartupEventArgs e)
        {
            _application = (ChronosApplication)Activator.CreateInstance(typeof(T), _applicationArguments);
            _application.Run();
            _applicationReadyEvent.Set();
        }
    }
}
