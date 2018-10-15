using System;

namespace Chronos.Client.Win.ViewModels.Home
{
    public class HomePageViewModel : GridViewModel
    {
        private readonly IMainApplication _application;

        public HomePageViewModel(IMainApplication application)
        {
            _application = application;
            _application.ApplicationStateChanged += OnApplicationStateChanged;
        }

        public double StartupTime
        {
            get { return Math.Round(_application.StartupTime.TotalSeconds, 3); }
        }

        public override string DisplayName
        {
            get { return "Chronos Profiler"; }
            set { }
        }

        protected override void BuildLayout()
        {
            base.BuildLayout();
            TryAdd(new ProfilingTargetsViewModel(_application));
            TryAdd(new ActiveSessionsViewModel(_application));
            TryAdd(new RecentSessionsViewModel(_application));
        }

        private void OnApplicationStateChanged(object sender, ApplicationStateEventArgs e)
        {
            if (e.CurrentState == ApplicationState.Started)
            {
                _application.ApplicationStateChanged -= OnApplicationStateChanged;
                NotifyOfPropertyChange(() => StartupTime);
            }
        }
    }
}
