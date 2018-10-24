using System.Windows.Input;
using Adenium;
using Chronos.Common.EventsTree;

namespace Chronos.Client.Win.ViewModels.Common.EventsTree
{
    public sealed class TimelineViewModel : ViewModel
    {
        private readonly IEventsTreeViewModelCollection _eventsTreeViewModel;
        private IEventTreeCollection _eventTrees;

        public TimelineViewModel(IEventTreeCollection eventTrees, IProfilingTimer profilingTimer, IEventMessageBuilder eventMessageBuilder, IEventsTreeViewModelCollection eventsTreeViewModel)
        {
            _eventTrees = eventTrees;
            ProfilingTimer = profilingTimer;
            EventMessageBuilder = eventMessageBuilder;
            _eventsTreeViewModel = eventsTreeViewModel;
            OpenEventTreeCommand = new SyncCommand<ISingleEventTree>(OpenEventTree);
            _eventTrees.CollectionUpdated += OnEventTreesCollectionUpdated;
        }

        public IProfilingTimer ProfilingTimer { get; private set; }

        public ICommand OpenEventTreeCommand { get; private set; }

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

        public IEventMessageBuilder EventMessageBuilder { get; private set; }

        public override string DisplayName
        {
            get { return "Timeline"; }
        }

        private void OnEventTreesCollectionUpdated(object sender, EventTreeEventArgs e)
        {
            Events = _eventTrees;
        }

        private void OpenEventTree(ISingleEventTree eventTree)
        {
            IEventTreeCollection collection = new StaticEventTreeCollection(eventTree);
            _eventsTreeViewModel.Open(collection);
        }
    }
}
