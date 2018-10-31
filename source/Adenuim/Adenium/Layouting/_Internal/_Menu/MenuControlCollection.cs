using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;

namespace Adenium.Layouting
{
    internal abstract class MenuControlCollection : MenuControl, IMenuControlCollection, INotifyCollectionChanged
    {
        private readonly ObservableCollection<MenuControl> _collection;

        protected MenuControlCollection(string id)
            : base (id)
        {
            _collection = new ObservableCollection<MenuControl>();
        }

        public IMenuControl this[string id]
        {
            get { return _collection.FirstOrDefault(x => string.Equals(x.Id, id, StringComparison.Ordinal)); }
        }

        public event NotifyCollectionChangedEventHandler CollectionChanged
        {
            add { _collection.CollectionChanged += value; }
            remove { _collection.CollectionChanged -= value; }
        }

        public void Add(MenuControl control)
        {
            MenuControl existingControl = (MenuControl)this[control.Id];
            if (existingControl != null)
            {
                existingControl.Merge(control);
            }
            else
            {
                _collection.Add(control);
            }
        }

        public IEnumerator<IMenuControl> GetEnumerator()
        {
            return _collection.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public override void Invalidate()
        {
            base.Invalidate();
            foreach (MenuControl control in _collection)
            {
                control.Invalidate();
            }
        }

        public override void Dispose()
        {
            foreach (MenuControl control in _collection)
            {
                control.Dispose();
            }
            _collection.Clear();
            base.Dispose();
        }

        internal override void NotifyControlAttached()
        {
            base.NotifyControlAttached();
            foreach (MenuControl control in _collection)
            {
                control.NotifyControlAttached();
            }
        }

        internal override void Merge(MenuControl control)
        {
            base.Merge(control);
            MenuControlCollection controlCollection = control as MenuControlCollection;
            if (controlCollection != null)
            {
                foreach (MenuControl childControl in controlCollection.Cast<MenuControl>())
                {
                    Add(childControl);
                }
            }
        }
    }
}
