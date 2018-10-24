using Adenium.Menu;
using Chronos.Client.Win.Common.EventsTree.Properties;
using Chronos.Client.Win.ViewModels.Common.EventsTree;

namespace Chronos.Client.Win.Menu.Common.EventsTree
{
    internal sealed class EventsTreeMenuItem : MenuItem
    {
        private readonly IProfilingApplication _application;

        public EventsTreeMenuItem(IProfilingApplication application)
        {
            _application = application;
        }

        public override string Text
        {
            get { return Resources.EventsTreeMenuItem_Text; }
        }

        public override void OnAction()
        {
            IEventsTreeViewModelCollection viewModels = _application.ServiceContainer.Resolve<IEventsTreeViewModelCollection>();
            viewModels.Open();
        }
    }
}
