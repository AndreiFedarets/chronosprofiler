using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;

namespace Chronos.Client.Win.Menu
{
    internal sealed class MenuCollection : IMenuCollection
    {
        private readonly ObservableCollection<IMenu> _items;

        public MenuCollection()
        {
            _items = new ObservableCollection<IMenu>();
        }

        public IMenu this[string id]
        {
            get { return _items.FirstOrDefault(x => string.Equals(x.Id, id, StringComparison.OrdinalIgnoreCase)); }
        }

        public event NotifyCollectionChangedEventHandler CollectionChanged
        {
            add { _items.CollectionChanged += value; }
            remove { _items.CollectionChanged -= value; }
        }

        public void Add(IMenu menu)
        {
            IMenu existingMenu = this[menu.Id];
            if (existingMenu == null)
            {
                _items.Add(menu);
            }
            else
            {
                int index = _items.IndexOf(existingMenu);
                _items.RemoveAt(index);
                _items.Insert(index, menu);
            }
        }

        public IEnumerator<IMenu> GetEnumerator()
        {
            return _items.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
