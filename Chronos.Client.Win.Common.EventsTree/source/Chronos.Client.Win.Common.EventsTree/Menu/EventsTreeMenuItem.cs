using Chronos.Client.Win.Common.EventsTree.Properties;
using Chronos.Client.Win.ViewModels.Common.EventsTree;
using Chronos.Client.Win.ViewModels.Profiling;
using Chronos.Common.EventsTree;

namespace Chronos.Client.Win.Menu.Common.EventsTree
{
    internal sealed class EventsTreeMenuItem : MenuItem
    {
        private readonly ProfilingViewModel _profilingViewModel;
        private EventsTreeViewModel _viewModel;

        public EventsTreeMenuItem(ProfilingViewModel profilingViewModel)
        {
            _profilingViewModel = profilingViewModel;
        }

        public override string Text
        {
            get { return Resources.EventsTreeMenuItem_Text; }
        }

        private EventsTreeViewModel ViewModel
        {
            get
            {
                if (_viewModel == null)
                {
                    IProfilingApplication application = _profilingViewModel.Application;
                    IEventTreeCollection eventTrees = application.ServiceContainer.Resolve<IEventTreeCollection>();
                    IEventMessageBuilder messageBuilder = application.ServiceContainer.Resolve<IEventMessageBuilder>();
                    _viewModel = new EventsTreeViewModel(eventTrees, messageBuilder);
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
