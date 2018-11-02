using System.Collections.Generic;
using Adenium;
using Chronos.Client.Win.Controls.Common.EventsTree;
using Chronos.Common.EventsTree;

namespace Chronos.Client.Win.Common.EventsTree.ViewModels
{
    [ViewModelAttribute(Constants.ViewModels.EventTreeViewModel)]
    public sealed class EventTreeViewModel : ViewModel
    {
        public EventTreeViewModel(ISingleEventTree eventTree, IEventMessageBuilder eventMessageBuilder)
        {
            View = new EventsTreeView();
            View.EventMessageBuilder = eventMessageBuilder;
            AvailableSortTypes = new List<EventTreeSortType> { EventTreeSortType.None, EventTreeSortType.Time, EventTreeSortType.Hits };
            SelectedSortType = EventTreeSortType.Time;
            Events = new[] { eventTree };
        }

        public override string DisplayName
        {
            get { return "Event Tree" ; }
        }

        public IEnumerable<EventTreeSortType> AvailableSortTypes { get; private set; }

        public EventsTreeView View { get; private set; }

        public EventTreeSortType SelectedSortType
        {
            get { return View.EventsSortType; }
            set
            {
                View.EventsSortType = value;
                NotifyOfPropertyChange(() => SelectedSortType);
            }
        }

        public IEnumerable<IEventTree> Events
        {
            get { return View.Events; }
            private set { View.Events = value; }
        }
    }
}
