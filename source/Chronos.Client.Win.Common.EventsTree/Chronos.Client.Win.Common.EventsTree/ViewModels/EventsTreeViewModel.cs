using Caliburn.Micro;
using Chronos.Client.Win.Controls.Common.EventsTree;
using Chronos.Common.EventsTree;
using Layex.ViewModels;
using System.Collections.Generic;
using System.Threading;

namespace Chronos.Client.Win.Common.EventsTree.ViewModels
{
    [ViewModel(Constants.ViewModels.EventsTreeViewModel)]
    public sealed class EventsTreeViewModel : ViewModel
    {
        private readonly EventTreeMerger _eventTreeMerger;
        private readonly IEventTreeCollection _eventTrees;
        private EventTreeMergeType _selectedMergeType;
        private EventTreeSortType _selectedSortType;
        private IEnumerable<IEventTree> _events;

        public EventsTreeViewModel(IEventTreeCollection eventTrees, IEventMessageBuilder eventMessageBuilder)
        {
            _eventTrees = eventTrees;
            EventMessageBuilder = eventMessageBuilder;
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

        public IEventMessageBuilder EventMessageBuilder { get; private set; }

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
            get { return _events; }
            private set
            {
                _events = value;
                NotifyOfPropertyChange(() => Events);
            }
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
