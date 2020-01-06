using Layex.ViewModels;
using System;
using System.Threading.Tasks;

namespace Chronos.Client.Win.ViewModels.Profiling
{
    [ViewModel(Constants.ViewModels.Profiling)]
    public class ProfilingPageViewModel : ItemsViewModel
    {
        private bool _isEnabled;
        private readonly IProfilingApplication _application;

        public ProfilingPageViewModel(IProfilingApplication application)
        {
            _application = application;
            _application.ApplicationStateChanged += OnApplicationStateChanged;
            _application.SessionStateChanged += OnSessionStateChanged;
            _isEnabled = true;
        }

        public bool IsEnabled
        {
            get { return _isEnabled; }
            private set
            {
                _isEnabled = value;
                NotifyOfPropertyChange(() => IsEnabled);
            }
        }

        public bool IsProfilingActive
        {
            get { return _application.SessionState == SessionState.Profiling; }
        }

        public double StartupTime
        {
            get { return Math.Round(_application.StartupTime.TotalSeconds, 3); }
        }

        public override string DisplayName
        {
            get { return "Profiling Results"; }
            set { }
        }

        public void ReloadSnapshot()
        {
            IsEnabled = false;
            Task.Factory.StartNew(() => _application.FlushData()).ContinueWith(t => IsEnabled = true);
        }

        public override void Dispose()
        {
            _application.ApplicationStateChanged -= OnApplicationStateChanged;
            _application.SessionStateChanged -= OnSessionStateChanged;
            base.Dispose();
        }

        private void OnApplicationStateChanged(object sender, ApplicationStateEventArgs e)
        {
            if (e.CurrentState == ApplicationState.Started)
            {
                NotifyOfPropertyChange(() => StartupTime);
            }
        }

        private void OnSessionStateChanged(object sender, SessionStateEventArgs e)
        {
            //NotifyOfPropertyChange(() => IsProfilingActive);
        }
    }
}
