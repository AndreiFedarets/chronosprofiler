using Chronos.Client.Win.Controls.Common.EventsTree;
using Chronos.Common.EventsTree;
using System.Collections.Generic;

namespace Chronos.Client.Win.ViewModels.Common.EventsTree
{
    public sealed class EventsTreeViewModel : ViewModel
    {
        private readonly IEventMessageBuilder _eventMessageBuilder;
        private readonly EventTreeMerger _eventTreeMerger;
        private readonly IEventTreeCollection _eventTrees;
        private IEnumerable<IEventTree> _mergedEventTrees;
        private EventTreeMergeType _selectedMergeType;
        private EventTreeSortType _selectedSortType;

        public EventsTreeViewModel(IEventTreeCollection eventTrees, IEventMessageBuilder eventMessageBuilder)
        {
            _eventTrees = eventTrees;
            _eventTrees.CollectionUpdated += OnEventTreesCollectionUpdated;
            _eventMessageBuilder = eventMessageBuilder;
            _eventTreeMerger = new EventTreeMerger();
            AvailableMergeTypes = new List<EventTreeMergeType> { EventTreeMergeType.None, EventTreeMergeType.Root, EventTreeMergeType.Thread };
            SelectedMergeType = EventTreeMergeType.Root;
            AvailableSortTypes = new List<EventTreeSortType> { EventTreeSortType.None, EventTreeSortType.Time, EventTreeSortType.Hits };
            SelectedSortType = EventTreeSortType.Time;
        }

        public IEnumerable<EventTreeMergeType> AvailableMergeTypes { get; private set; }

        public IEnumerable<EventTreeSortType> AvailableSortTypes { get; private set; }

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
            get { return _selectedSortType; }
            set
            {
                _selectedSortType = value;
                NotifyOfPropertyChange(() => SelectedSortType);
            }
        }

        public IEnumerable<IEventTree> Events
        {
            get { return _mergedEventTrees; }
            private set
            {
                _mergedEventTrees = null;
                NotifyOfPropertyChange(() => Events);
                if (value != null)
                {
                    _mergedEventTrees = value;
                    NotifyOfPropertyChange(() => Events);
                }
            }
        }

        public IEventMessageBuilder EventMessageBuilder
        {
            get { return _eventMessageBuilder; }
        }

        public override string DisplayName
        {
            get { return "Events Tree"; }
        }

        private void OnEventTreesCollectionUpdated(object sender, EventTreeEventArgs e)
        {
            Events = _eventTreeMerger.Merge(_eventTrees, _selectedMergeType);
        }
    }
}
