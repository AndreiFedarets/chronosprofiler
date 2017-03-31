using Chronos.Common.EventsTree;

namespace Chronos.Client.Win.ViewModels.Common.EventsTree
{
    public class ThreadTreeCollectionViewModel : ViewModel
    {
        private readonly IEventMessageBuilder _eventMessageBuilder;
        private IThreadTreeCollection _threads;

        public ThreadTreeCollectionViewModel(IThreadTreeCollection threads, IEventMessageBuilder eventMessageBuilder)
        {
            _threads = threads;
            _eventMessageBuilder = eventMessageBuilder;
        }

        public IThreadTreeCollection Threads
        {
            get { return _threads; }
            private set
            {
                _threads = value;
                NotifyOfPropertyChange(() => Threads);
            }
        }

        public IEventMessageBuilder EventMessageBuilder
        {
            get { return _eventMessageBuilder; }
        }

        public override string DisplayName
        {
            get { return "Threads Tree"; }
        }

        public void Reload()
        {
            IThreadTreeCollection threads = Threads;
            Threads = null;
            Threads = threads;
        }
    }
}
