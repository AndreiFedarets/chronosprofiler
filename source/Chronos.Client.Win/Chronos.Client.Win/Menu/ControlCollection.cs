using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;

namespace Chronos.Client.Win.Menu
{
    public abstract class ControlCollection : Control, IControlCollection
    {
        private readonly ObservableCollection<IControl> _children;

        protected ControlCollection()
        {
            _children = new ObservableCollection<IControl>();
            _children.CollectionChanged += ChildrenCollectionChanged;
        }

        public IControl this[string id]
        {
            get { return _children.FirstOrDefault(x => string.Equals(x.Id, id, StringComparison.Ordinal)); }
        }

        public event NotifyCollectionChangedEventHandler CollectionChanged;

        public virtual IControlCollection Add(IControl control)
        {
            _children.Add(control);
            return this;
        }

        public virtual IControlCollection Remove(IControl control)
        {
            _children.Add(control);
            return this;
        }

        public IControl FindChild(string id)
        {
            foreach (IControl control in _children)
            {
                if (string.Equals(id, control.Id, StringComparison.Ordinal))
                {
                    return control;
                }
            }
            return null;
        }

        public override void Dispose()
        {
            foreach (IControl control in _children)
            {
                control.TryDispose();
            }
            _children.Clear();
            base.Dispose();
        }

        public IEnumerator<IControl> GetEnumerator()
        {
            return _children.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        private void ChildrenCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            CollectionChanged.SafeInvoke(this, e);
        }
    }
}
