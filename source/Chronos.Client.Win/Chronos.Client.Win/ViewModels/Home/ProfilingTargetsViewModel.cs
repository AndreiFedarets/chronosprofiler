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
            _application.ViewModelManager.ShowDialog<Start.StartPageViewModel, IProfilingTarget>(null, profilingTarget);
        }

        public bool CanCreateConfiguration(IProfilingTarget profilingTarget)
        {
            return profilingTarget != null;
        }
    }
}
