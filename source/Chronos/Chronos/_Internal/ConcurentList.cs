using System.Collections.Generic;

namespace Chronos
{
    internal sealed class ConcurentList<T> : IList<T>
    {
        private readonly List<T> _items;

        public ConcurentList()
        {
            _items = new List<T>();
        }

        public int IndexOf(T item)
        {
            lock (_items)
            {
                return _items.IndexOf(item);
            }
        }

        public void Insert(int index, T item)
        {
            lock (_items)
            {
                _items.Insert(index, item);
            }
        }

        public void RemoveAt(int index)
        {
            lock (_items)
            {
                _items.RemoveAt(index);
            }
        }

        public T this[int index]
        {
            get
            {
                lock (_items)
                {
                    return _items[index];
                }
            }
            set
            {
                lock (_items)
                {
                    _items[index] = value;
                }
            }
        }

        public void Add(T item)
        {
            lock (_items)
            {
                _items.Add(item);
            }
        }

        public void Clear()
        {
            lock (_items)
            {
                _items.Clear();
            }
        }

        public bool Contains(T item)
        {
            lock (_items)
            {
                return _items.Contains(item);
            }
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            lock (_items)
            {
                _items.CopyTo(array, arrayIndex);
            }
        }

        public int Count
        {
            get
            {
                lock (_items)
                {
                    return _items.Count;
                }
            }
        }

        public bool IsReadOnly
        {
            get
            {
                lock (_items)
                {
                    return false;
                }
            }
        }

        public bool Remove(T item)
        {
            lock (_items)
            {
                 return _items.Remove(item);
            }
        }

        public IEnumerator<T> GetEnumerator()
        {
            List<T> items;
            lock (_items)
            {
                items = new List<T>(_items);
            }
            return items.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
