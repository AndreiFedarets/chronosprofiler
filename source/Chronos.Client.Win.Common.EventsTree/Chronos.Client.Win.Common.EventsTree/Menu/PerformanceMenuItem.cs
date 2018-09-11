using Chronos.Client.Win.Common.EventsTree.Properties;
using Chronos.Client.Win.ViewModels.Profiling;

namespace Chronos.Client.Win.Menu.Common.EventsTree
{
    internal class PerformanceMenuItem : MenuItem
    {
        private readonly ProfilingViewModel _profilingViewModel;
        //private EventsTreeViewModel _viewModel;

        public PerformanceMenuItem(ProfilingViewModel profilingViewModel)
        {
            _profilingViewModel = profilingViewModel;
        }

        public override string Text
        {
            get { return Resources.PerformanceMenuItem_Text; }
        }

        //private EventsTreeViewModel ViewModel
        //{
        //    get
        //    {
        //        if (_viewModel == null)
        //        {
        //            IProfilingApplication application = _profilingViewModel.Application;
        //            IEventTreeCollection eventTrees = application.Container.Resolve<IEventTreeCollection>();
        //            IEventMessageBuilder messageBuilder = application.Container.Resolve<IEventMessageBuilder>();
        //            _viewModel = new EventsTreeViewModel(eventTrees, messageBuilder);
        //            _profilingViewModel.Add(_viewModel);
        //        }
        //        return _viewModel;
        //    }
        //}

        public override void OnAction()
        {
            //_profilingViewModel.Activate(ViewModel);
        }
    }
}
