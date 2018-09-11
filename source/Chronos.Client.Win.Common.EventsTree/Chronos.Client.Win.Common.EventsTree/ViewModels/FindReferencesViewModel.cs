using Chronos.Common.EventsTree;
using System.Windows.Input;

namespace Chronos.Client.Win.ViewModels.Common.EventsTree
{
    public class FindReferencesViewModel : ViewModel
    {
        private object _selectedNode;

        public FindReferencesViewModel(HeaderReference<uint, IEvent> headerReference, IEventsTreeViewModel eventsTreeViewModel)
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
