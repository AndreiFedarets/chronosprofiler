using Layex.ViewModels;
using System;

namespace Chronos.Client.Win.ViewModels.Home
{
    [ViewModel(Constants.ViewModels.Home)]
    public class HomePageViewModel : ItemsViewModel
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
