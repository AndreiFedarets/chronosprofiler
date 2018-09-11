using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Windows.Threading;

namespace Chronos.Client.Win
{
    internal class ObservableCollectionBase<T> : PropertyChangedBaseEx, INotifyCollectionChanged, IEnumerable<T>
    {
        private readonly ObservableCollection<T> _collection;

        protected ObservableCollectionBase()
        {
            _collection = new ObservableCollection<T>();
            _collection.CollectionChanged += OnInternalCollectionChanged;
        }

        protected int Count
        {
            get { return _collection.Count; }
        }

        protected object CollectionLock
        {
            get { return _collection; }
        }

        protected void Add(T item)
        {
            _collection.Add(item);
        }

        protected bool Remove(T item)
        {
            return _collection.Remove(item);
        }

        protected bool Contains(T item)
        {
            return _collection.Contains(item);
        }

        protected void Clear()
        {
            _collection.Clear();
        }

        protected T FindFirst(Func<T, bool> filter)
        {
            foreach (T item in _collection)
            {
                if (filter(item))
                {
                    return item;
                }
            }
            return default(T);
        }

        public event NotifyCollectionChangedEventHandler CollectionChanged;

        public IEnumerator<T> GetEnumerator()
        {
            List<T> collection;
            lock (CollectionLock)
            {
                collection = new List<T>(_collection);
            }
            return collection.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        private void OnInternalCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            NotifyCollectionChangedEventHandler handler = CollectionChanged;
            if (handler != null)
            {
                System.Action action = () =>handler(this, e);
                if (Dispatcher.CheckAccess())
                {
                    action();
                }
                else
                {
                    Dispatcher.BeginInvoke(action);
                }
            }
        }
    }
}
