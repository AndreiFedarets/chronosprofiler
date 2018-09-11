using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;

namespace Chronos.Client.Win
{
    internal class ObservableDictionary<TKey, TValue> : IDictionary<TKey, TValue>, INotifyCollectionChanged, INotifyPropertyChanged, IDisposable
    {
        private readonly IDictionary<TKey, TValue> _dictionary;
        private readonly ObservableCollection<TValue> _collection;

        public ObservableDictionary()
        {
            _dictionary = new Dictionary<TKey, TValue>();
            _collection = new ObservableCollection<TValue>();
            _collection.CollectionChanged += OnCollectionChanged;
            ((INotifyPropertyChanged)_collection).PropertyChanged += OnPropertyChanged;
        }

        public TValue this[TKey key]
        {
            get { return _dictionary[key]; }
            set
            {
                _dictionary[key] = value;
                _collection.Remove(value);
                _collection.Add(value);
            }
        }

        public int Count
        {
            get { return _dictionary.Count; }
        }

        public bool IsReadOnly
        {
            get { return _dictionary.IsReadOnly; }
        }

        public ICollection<TValue> Values
        {
            get { return _dictionary.Values; }
        }

        public ICollection<TKey> Keys
        {
            get { return _dictionary.Keys; }
        }

        public event NotifyCollectionChangedEventHandler CollectionChanged;

        public event PropertyChangedEventHandler PropertyChanged;

        public void Add(TKey key, TValue value)
        {
            _dictionary.Add(key, value);
            _collection.Add(value);
        }

        public bool ContainsKey(TKey key)
        {
            return _dictionary.ContainsKey(key);
        }

        public bool Remove(TKey key)
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

        public bool TryGetValue(TKey key, out TValue value)
        {
            return _dictionary.TryGetValue(key, out value);
        }

        public void Add(KeyValuePair<TKey, TValue> item)
        {
            _dictionary.Add(item);
            _collection.Add(item.Value);
        }

        public void Clear()
        {
            _dictionary.Clear();
            _collection.Clear();
        }

        public bool Contains(KeyValuePair<TKey, TValue> item)
        {
            return _dictionary.Contains(item);
        }

        public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
        {
            _dictionary.CopyTo(array, arrayIndex);
        }

        public bool Remove(KeyValuePair<TKey, TValue> item)
        {
            _dictionary.Remove(item);
            return _collection.Remove(item.Value);
        }

        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
        {
            return _dictionary.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        private void OnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            CollectionChanged.SafeInvoke(this, e);
        }

        private void OnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            PropertyChanged.SafeInvoke(this, e);
        }

        public void Dispose()
        {
            _collection.CollectionChanged -= OnCollectionChanged;
            ((INotifyPropertyChanged)_collection).PropertyChanged -= OnPropertyChanged;
        }
    }
}
