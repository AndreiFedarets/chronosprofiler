using System;
using System.Threading.Tasks;
using Chronos.Client.Win.Menu;

namespace Chronos.Client.Win.ViewModels.Profiling
{
    public class ProfilingViewModel : TabViewModel
    {
        private bool _isEnabled;

        public ProfilingViewModel(IProfilingApplication application)
        {
            Application = application;
            Application.ApplicationStateChanged += OnApplicationStateChanged;
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

        public IProfilingApplication Application { get; private set; }

        public IMenu Menu { get; private set; }

        public double StartupTime
        {
            get { return Math.Round(Application.StartupTime.TotalSeconds, 3); }
        }

        public override string DisplayName
        {
            get { return "Profiling Results"; }
            set { }
        }

        private void OnApplicationStateChanged(object sender, ApplicationStateEventArgs e)
        {
            if (e.CurrentState == ApplicationState.Started)
            {
                NotifyOfPropertyChange(() => StartupTime);
            }
        }

        protected override void BuildLayout()
        {
            BuildMenu();
            base.BuildLayout();
        }

        private void BuildMenu()
        {
            MenuBuilder builder = new MenuBuilder();
            Menu = builder.BuildMenuForApplication(Application, this);
        }

        public void ReloadSnapshot()
        {
            IsEnabled = false;
            Task.Factory.StartNew(() => Application.FlushData()).ContinueWith(t => IsEnabled = true);
        }

        public override void Dispose()
        {
            Menu.TryDispose();
            base.Dispose();
        }
    }
}
