using Chronos.Client.Win.Common.EventsTree.Properties;
using Chronos.Client.Win.ViewModels.Common.EventsTree;

namespace Chronos.Client.Win.Menu.Common.EventsTree
{
    internal sealed class EventsTreeMenuItem : MenuItem
    {
        private readonly IEventsTreeViewModelCollection _eventsTreeViewModels;

        public EventsTreeMenuItem(IEventsTreeViewModelCollection eventsTreeViewModels)
        {
            _eventsTreeViewModels = eventsTreeViewModels;
        }

        public override string Text
        {
            get { return Resources.EventsTreeMenuItem_Text; }
        }

        public override void OnAction()
        {
            _eventsTreeViewModels.Open();
        }
    }
}
