using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;

namespace Chronos.Client
{
    internal class DictionaryChangedBase<TKey, TValue> : BaseObject, IEnumerable<TValue>, INotifyCollectionChanged
    {
        private readonly ObservableCollection<TValue> _collection;
        private readonly Dictionary<TKey, TValue> _dictionary;

        public DictionaryChangedBase()
        {
            _collection = new ObservableCollection<TValue>();
            _collection.CollectionChanged += OnItemsCollectionChanged;
            _dictionary = new Dictionary<TKey, TValue>();
        }

        protected sealed override object Lock
        {
            get { return _collection; }
        }

        public int Count
        {
            get
            {
                lock (Lock)
                {
                    VerifyDisposed();
                    return _collection.Count;
                }
            }
        }

        public event NotifyCollectionChangedEventHandler CollectionChanged;

        public IEnumerator<TValue> GetEnumerator()
        {
            List<TValue> collection;
            lock (Lock)
            {
                VerifyDisposed();
                collection = new List<TValue>(_collection);
            }
            return collection.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        protected TValue GetItem(TKey key)
        {
            return _dictionary[key];
        }

        protected void Add(TKey key, TValue value)
        {
            _dictionary.Add(key, value);
            _collection.Add(value);
        }

        protected bool Remove(TKey key)
        {
            TValue value;
            if (_dictionary.TryGetValue(key, out value))
            {
                _dictionary.Remove(key);
                _collection.Remove(value);
                return true;
            }
            return false;
        }

        protected bool TryGetValue(TKey key, out TValue value)
        {
            return _dictionary.TryGetValue(key, out value);
        }

        protected bool ContainsKey(TKey key)
        {
            return _dictionary.ContainsKey(key);
        }

        protected void Clear()
        {
            _dictionary.Clear();
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
                foreach (TValue item in _collection)
                {
                    item.TryDispose();
                }
                _dictionary.Clear();
                _collection.Clear();
                base.Dispose();
            }
        }
    }
}
