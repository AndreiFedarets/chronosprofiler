using System.Collections.Generic;
using Adenium;
using Chronos.Client.Win.Controls.Common.EventsTree;
using Chronos.Common.EventsTree;

namespace Chronos.Client.Win.Common.EventsTree.ViewModels
{
    [ViewModelAttribute(Constants.ViewModels.EventsTreeViewModel)]
    public sealed class EventsTreeViewModel : ViewModel
    {
        private readonly EventTreeMerger _eventTreeMerger;
        private readonly IEventTreeCollection _eventTrees;
        private EventTreeMergeType _selectedMergeType;

        public EventsTreeViewModel(IEventTreeCollection eventTrees, IEventMessageBuilder eventMessageBuilder)
        {
            _eventTrees = eventTrees;
            View = new EventsTreeView();
            View.EventMessageBuilder = eventMessageBuilder;
            _eventTrees.CollectionUpdated += OnEventTreesCollectionUpdated;
            _eventTreeMerger = new EventTreeMerger();
            AvailableMergeTypes = new List<EventTreeMergeType> { EventTreeMergeType.None, EventTreeMergeType.Root, EventTreeMergeType.Thread };
            SelectedMergeType = EventTreeMergeType.Root;
            AvailableSortTypes = new List<EventTreeSortType> { EventTreeSortType.None, EventTreeSortType.Time, EventTreeSortType.Hits };
            SelectedSortType = EventTreeSortType.Time;
        }

        public override string DisplayName
        {
            get { return "Events Tree"; }
        }

        public IEnumerable<EventTreeMergeType> AvailableMergeTypes { get; private set; }

        public IEnumerable<EventTreeSortType> AvailableSortTypes { get; private set; }

        public EventsTreeView View { get; private set; }

        public EventTreeMergeType SelectedMergeType
        {
            get { return _selectedMergeType; }
            set
            {
                bool changed = _selectedMergeType != value;
                _selectedMergeType = value;
                NotifyOfPropertyChange(() => SelectedMergeType);
                if (changed)
                {
                    Events = _eventTreeMerger.Merge(_eventTrees, _selectedMergeType);
                }
            }
        }

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

        public override void Dispose()
        {
            _eventTrees.CollectionUpdated -= OnEventTreesCollectionUpdated;
            base.Dispose();
        }

        private void OnEventTreesCollectionUpdated(object sender, EventTreeEventArgs e)
        {
            Events = _eventTreeMerger.Merge(_eventTrees, _selectedMergeType);
        }
    }
}
