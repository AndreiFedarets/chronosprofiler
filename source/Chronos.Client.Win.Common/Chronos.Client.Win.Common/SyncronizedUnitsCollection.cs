using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using Adenium;

namespace Chronos.Client.Win.Common
{
    public sealed class SyncronizedUnitsCollection<T> : IEnumerable<T>, INotifyCollectionChanged, IDisposable
    {
        private readonly INotifyCollectionChanged _collectionChanged;
        private readonly IEnumerable<T> _collection;

        public SyncronizedUnitsCollection(IEnumerable<T> collection)
        {
            _collection = collection;
            _collectionChanged = collection as INotifyCollectionChanged;
            if (_collectionChanged != null)
            {
                _collectionChanged.CollectionChanged += OnCollectionChanged;
            }
        }

        private void OnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            SmartDispatcher.Main.BeginInvoke(() =>
            {
                CollectionChanged.SafeInvoke(this, e);
            });
        }

        public event NotifyCollectionChangedEventHandler CollectionChanged;

        public IEnumerator<T> GetEnumerator()
        {
            return _collection.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void Dispose()
        {
            if (_collectionChanged != null)
            {
                _collectionChanged.CollectionChanged -= OnCollectionChanged;
            }
        }
    }
}
