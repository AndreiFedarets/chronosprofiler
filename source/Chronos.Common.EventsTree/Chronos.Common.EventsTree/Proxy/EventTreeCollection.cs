using Chronos.Client;
using Chronos.Proxy;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;

namespace Chronos.Common.EventsTree.Proxy
{
    internal sealed class EventTreeCollection : ProxyBaseObject<IEventTreeCollection>, IEventTreeCollection, INotifyCollectionChanged
    {
        private readonly Dictionary<ulong, ISingleEventTree> _dictionaryByUid;
        private readonly ObservableCollection<ISingleEventTree> _collection;
        private readonly SafeEventHandlerCollection<EventTreeEventArgs> _collectionUpdatedEvent;
        private readonly RemoteEventSubscription<EventTreeEventArgs> _collectionUpdatedSubscriber;

        public EventTreeCollection(IEventTreeCollection remoteObject)
            : base(remoteObject)
        {
            _dictionaryByUid = new Dictionary<ulong, ISingleEventTree>();
            _collectionUpdatedEvent = new SafeEventHandlerCollection<EventTreeEventArgs>();
            _collectionUpdatedSubscriber = new RemoteEventSubscription<EventTreeEventArgs>(remoteObject, "CollectionUpdated", OnRemoteCollectionUpdated);
            _collection = new ObservableCollection<ISingleEventTree>();
            _collection.CollectionChanged += OnCollectionChanged;
            InitializeCollection();
        }

        public event EventHandler<EventTreeEventArgs> CollectionUpdated
        {
            add { _collectionUpdatedEvent.Add(value); }
            remove { _collectionUpdatedEvent.Remove(value); }
        }

        public event NotifyCollectionChangedEventHandler CollectionChanged;

        public IEnumerator<ISingleEventTree> GetEnumerator()
        {
            List<ISingleEventTree> items;
            lock (_dictionaryByUid)
            {
                items = new List<ISingleEventTree>(_dictionaryByUid.Values);
            }
            return items.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public override void Dispose()
        {
            _collectionUpdatedSubscriber.Unsubscribe();
            _collectionUpdatedEvent.Dispose();
            _collection.CollectionChanged -= OnCollectionChanged;
            base.Dispose();
        }

        public void OnRemoteCollectionUpdated(object sender, EventTreeEventArgs e)
        {
            UpdateCollection(e.Collection);
            Action action = () => _collectionUpdatedEvent.Raise(this, e);
            DispatcherHolder.BeginInvoke(action);
        }

        private void UpdateCollection(List<ISingleEventTree> collection)
        {
            foreach (ISingleEventTree eventTree in collection)
            {
                _dictionaryByUid.Add(eventTree.EventTreeUid, eventTree);
                _collection.Add(eventTree);
            }
        }

        private void InitializeCollection()
        {
            lock (_dictionaryByUid)
            {
                _collectionUpdatedSubscriber.Subscribe();
                UpdateCollection(RemoteObject.ToList());
            }
        }

        private void OnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            CollectionChanged.SafeInvoke(this, e);
        }
    }
}
