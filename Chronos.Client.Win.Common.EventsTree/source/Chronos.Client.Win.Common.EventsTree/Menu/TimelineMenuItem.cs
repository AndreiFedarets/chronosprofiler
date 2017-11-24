using Chronos.Client.Win.Common.EventsTree.Properties;
using Chronos.Client.Win.ViewModels.Common.EventsTree;
using Chronos.Client.Win.ViewModels.Profiling;
using Chronos.Common.EventsTree;

namespace Chronos.Client.Win.Menu.Common.EventsTree
{
    internal sealed class TimelineMenuItem : MenuItem
    {
        private readonly ProfilingViewModel _profilingViewModel;
        private TimelineViewModel _viewModel;

        public TimelineMenuItem(ProfilingViewModel profilingViewModel)
        {
            _profilingViewModel = profilingViewModel;
        }

        public override string Text
        {
            get { return Resources.TimelineMenuItem_Text; }
        }

        private TimelineViewModel ViewModel
        {
            get
            {
                if (_viewModel == null)
                {
                    IProfilingApplication application = _profilingViewModel.Application;
                    IEventTreeCollection eventTrees = application.ServiceContainer.Resolve<IEventTreeCollection>();
                    _viewModel = new TimelineViewModel(eventTrees);
                    _profilingViewModel.Add(_viewModel);
                }
                return _viewModel;
            }
        }

        public override void OnAction()
        {
            _profilingViewModel.Activate(ViewModel);
        }
    }
}
