using System;
using System.Threading.Tasks;
using Adenium;
using Adenium.Layouting;

namespace Chronos.Client.Win.ViewModels.Profiling
{
    public class ProfilingViewModel : TabViewModel
    {
        private bool _isEnabled;
        private readonly IProfilingApplication _application;

        public ProfilingViewModel(IProfilingApplication application)
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

        public IMenu Menu { get; private set; }

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
