using Layex.ViewModels;

namespace Chronos.Client.Win.ViewModels.Home
{
    internal class ProfilingTargetsViewModel : ViewModel
    {
        private readonly IMainApplication _application;
        private readonly IViewModelManager _viewModelManager;

        public ProfilingTargetsViewModel(IMainApplication application)
        {
            _application = application;
        }

        public override string DisplayName
        {
            get { return "Start Profiling"; }
        }

        public IProfilingTargetCollection ProfilingTargets
        {
            get { return _application.ProfilingTargets; }
        }

        public void CreateConfiguration(IProfilingTarget profilingTarget)
        {
            _viewModelManager.Activate<IProfilingTarget>(Constants.ViewModels.Start, profilingTarget);
        }

        public bool CanCreateConfiguration(IProfilingTarget profilingTarget)
        {
            return profilingTarget != null;
        }
    }
}
