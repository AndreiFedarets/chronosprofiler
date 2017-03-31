using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;

namespace Chronos.Client
{
    internal class CollectionChangedBase<T> : BaseObject, IEnumerable<T>, INotifyCollectionChanged
    {
        private readonly ObservableCollection<T> _collection;

        public CollectionChangedBase()
        {
            _collection = new ObservableCollection<T>();
            _collection.CollectionChanged += OnItemsCollectionChanged;
        }

        protected sealed override object Lock
        {
            get { return _collection; }
        }

        public event NotifyCollectionChangedEventHandler CollectionChanged;

        public IEnumerator<T> GetEnumerator()
        {
            List<T> collection;
            lock (Lock)
            {
                collection = new List<T>(_collection);
            }
            return collection.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        protected void Add(T item)
        {
            _collection.Add(item);
        }

        protected bool Remove(T item)
        {
            return _collection.Remove(item);
        }

        protected void Clear()
        {
            _collection.Clear();
        }

        private void OnItemsCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            NotifyCollectionChangedEventHandler handler = CollectionChanged;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        public override void Dispose()
        {
            lock (Lock)
            {
                VerifyDisposed();
                foreach (T item in _collection)
                {
                    item.TryDispose();
                }
                _collection.Clear();
                base.Dispose();
            }
        }
    }
}
