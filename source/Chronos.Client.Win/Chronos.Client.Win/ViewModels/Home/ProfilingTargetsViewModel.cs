using Adenium;

namespace Chronos.Client.Win.ViewModels.Home
{
    internal class ProfilingTargetsViewModel : ViewModel
    {
        private readonly IMainApplication _application;

        public ProfilingTargetsViewModel(IMainApplication application)
        {
            _application = application;
        }

        public override string DisplayName
        {
            get { return "Start Profiling"; }
            set { }
        }

        public IProfilingTargetCollection ProfilingTargets
        {
            get { return _application.ProfilingTargets; }
        }

        public void CreateConfiguration(IProfilingTarget profilingTarget)
        {
            Start.StartPageViewModel startPageViewModel = new Start.StartPageViewModel(_application, profilingTarget);
            _application.ViewModelManager.ShowDialog(startPageViewModel);
        }

        public bool CanCreateConfiguration(IProfilingTarget profilingTarget)
        {
            return profilingTarget != null;
        }
    }
}
