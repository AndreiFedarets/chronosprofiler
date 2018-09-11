using Chronos.Client.Win.DotNet.EventsTree;
using Chronos.Client.Win.ViewModels;

namespace Chronos.Client.Win.Common.EventsTree
{
    internal class ProfilingTypeSession : IProfilingTypeSession
    {
        private readonly EventsTreeCollection _eventsTrees;
        private readonly EventsFormatter _eventsFormatter;

        public ProfilingTypeSession()
        {
            _eventsTrees = new EventsTreeCollection();
            _eventsFormatter = new EventsFormatter();
        }

        public void IntegrateViewModel(object profilingResultsViewModel)
        {
            ProfilingResultsViewModel viewModel = (ProfilingResultsViewModel)profilingResultsViewModel;
            IMenuItem performanceMenuItem = viewModel.Menu.AddMenuItem(new PerformanceMenuItem());
            performanceMenuItem.AddMenuItem(new EventsTreeMenuItem(_eventsTrees, _eventsFormatter));
            performanceMenuItem.AddMenuItem(new ThreadsTreeMenuItem(_eventsTrees, _eventsFormatter));
        }

        public void ExportServices(IServiceContainer container)
        {
            container.Register<IEventsFormatter>(_eventsFormatter);
        }

        public void ImportServices(IServiceContainer container)
        {
            Daemon.Common.EventsTree.IEventsTreeCollection eventsTrees = container.Resolve<Daemon.Common.EventsTree.IEventsTreeCollection>();
            _eventsTrees.Initialize(eventsTrees);
        }

        public void ReloadData()
        {
            _eventsTrees.Refresh();
        }
    }
}
