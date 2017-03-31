namespace Chronos.Client.Win.ViewModels.Start
{
    public class ProfilingTargetContent : PlaceholderContent
    {
        private readonly StartPageViewModel _pageViewModel;
        private readonly IProfilingTarget _profilingTarget;

        public ProfilingTargetContent(IProfilingTarget profilingTarget, StartPageViewModel page)
        {
            _profilingTarget = profilingTarget;
            _pageViewModel = page;
        }

        public override string DisplayName
        {
            get { return "Application Settings"; }
        }

        public override ViewModel CreateViewModel()
        {
            ViewModel viewModel = null;
            IProfilingTargetAdapter adapter = _profilingTarget.GetWinAdapter();
            if (adapter != null)
            {
                viewModel = adapter.CreateConfigurationViewModel(_pageViewModel);
            }
            return viewModel;
        }
    }
}
