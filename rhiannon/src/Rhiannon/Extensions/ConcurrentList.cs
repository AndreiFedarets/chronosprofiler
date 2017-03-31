using System.Collections.Generic;

namespace Rhiannon.Extensions
{
    public class ConcurrentList<T>
    {
        private readonly List<T> _list;

        public ConcurrentList()
        {
            _list = new List<T>();
        }

        public void Add(T item)
        {
            lock (_list)
            {
                _list.Add(item);
            }
        }

        public void Remove(T item)
        {
            lock (_list)
            {
                _list.Remove(item);
            }
        }

        public bool Contains(T item)
        {
            lock (_list)
            {
                return _list.Contains(item);
            }
        }
    }
}
