using Chronos.Common.EventsTree;
using System.Windows.Input;
using Adenium;

namespace Chronos.Client.Win.ViewModels.Common.EventsTree
{
    public class FindEventsViewModel : ViewModel
    {
        private object _selectedNode;

        public FindEventsViewModel(HeaderReference<uint, IEvent> headerReference, IEventsTreeViewModel eventsTreeViewModel)
        {
            HeaderReference = headerReference;
        }

        public ICommand NavigateToEventCommand { get; private set; }

        public HeaderReference<uint, IEvent> HeaderReference { get; private set; }

        public object SelectedNode
        {
            get { return _selectedNode; }
            set
            {
                _selectedNode = value;
                NotifyOfPropertyChange(() => SelectedNode);
            }
        }
    }
}
