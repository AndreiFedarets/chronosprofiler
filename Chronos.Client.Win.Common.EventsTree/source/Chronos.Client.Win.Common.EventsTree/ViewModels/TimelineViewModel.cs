using Chronos.Common.EventsTree;

namespace Chronos.Client.Win.ViewModels.Common.EventsTree
{
    public sealed class TimelineViewModel : ViewModel
    {
        private IEventTreeCollection _eventTrees;

        public TimelineViewModel(IEventTreeCollection eventTrees)
        {
            _eventTrees = eventTrees;
            _eventTrees.CollectionUpdated += OnEventTreesCollectionUpdated;
        }

        public IEventTreeCollection Events
        {
            get { return _eventTrees; }
            private set
            {
                _eventTrees = null;
                NotifyOfPropertyChange(() => Events);
                if (value != null)
                {
                    _eventTrees = value;
                    NotifyOfPropertyChange(() => Events);
                }
            }
        }

        private void OnEventTreesCollectionUpdated(object sender, EventTreeEventArgs e)
        {
            Events = _eventTrees;
        }
    }
}
