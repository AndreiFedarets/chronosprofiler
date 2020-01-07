using Layex.ViewModels;

namespace Chronos.Client.Win.ViewModels.Home
{
    internal class ProfilingTargetsViewModel : ViewModel
    {
        private readonly IMainApplication _application;
        private readonly IViewModelManager _viewModelManager;

        public ProfilingTargetsViewModel(IMainApplication application, IViewModelManager viewModelManager)
        {
            _application = application;
            _viewModelManager = viewModelManager;
        }

        public override string DisplayName
        {
            get { return Properties.Resources.ProfilingTargetsViewModel_DisplayName; }
            set { }
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
