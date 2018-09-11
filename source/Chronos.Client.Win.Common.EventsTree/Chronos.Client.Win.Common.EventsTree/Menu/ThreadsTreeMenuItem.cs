using Chronos.Client.Win.Common.EventsTree.Properties;
using Chronos.Client.Win.ViewModels.Common.EventsTree;
using Chronos.Client.Win.ViewModels.Profiling;
using Chronos.Common.EventsTree;

namespace Chronos.Client.Win.Menu.Common.EventsTree
{
    internal class ThreadsTreeMenuItem : MenuItem
    {
        private readonly ProfilingViewModel _profilingViewModel;
        private ThreadTreeCollectionViewModel _viewModel;

        public ThreadsTreeMenuItem(ProfilingViewModel profilingViewModel)
        {
            _profilingViewModel = profilingViewModel;
        }

        public override string Text
        {
            get { return Resources.ThreadsTreeMenuItem_Text; }
        }

        private ThreadTreeCollectionViewModel ViewModel
        {
            get
            {
                if (_viewModel == null)
                {
                    IProfilingApplication application = _profilingViewModel.Application;
                    IThreadTreeCollection threadTrees = application.Session.ServiceContainer.Resolve<IThreadTreeCollection>();
                    IEventMessageBuilder messageBuilder = application.Session.ServiceContainer.Resolve<IEventMessageBuilder>();
                    _viewModel = new ThreadTreeCollectionViewModel(threadTrees, messageBuilder);
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
