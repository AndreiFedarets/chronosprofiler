using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;

namespace Adenium.Layouting
{
    internal sealed class MenuCollection : IMenuCollection, IDisposable
    {
        private readonly ObservableCollection<Menu> _collection;

        public MenuCollection()
        {
            _collection = new ObservableCollection<Menu>();
        }

        public IMenu this[string id]
        {
            get { return _collection.FirstOrDefault(x => string.Equals(x.Id, id, StringComparison.Ordinal)); }
        }

        public event NotifyCollectionChangedEventHandler CollectionChanged
        {
            add { _collection.CollectionChanged += value; }
            remove { _collection.CollectionChanged -= value; }
        }

        public void Add(Menu menu)
        {
            Menu existingMenu = (Menu)this[menu.Id];
            if (existingMenu != null)
            {
                _collection.Remove(existingMenu);
            }
            _collection.Add(menu);
        }

        public IEnumerator<IMenu> GetEnumerator()
        {
            return _collection.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void Dispose()
        {
            foreach (Menu control in _collection)
            {
                control.Dispose();
            }
            _collection.Clear();
        }
    }
}
