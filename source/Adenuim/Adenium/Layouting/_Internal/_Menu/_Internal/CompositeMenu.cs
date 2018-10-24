using System;
using System.Collections.Generic;

namespace Adenium.Menu
{
    public sealed class CompositeMenu : MenuControlCollection, ICompositeControlCollection, IMenu
    {
        private readonly List<IMenu> _undrelyingControls;
        private readonly string _id;

        public CompositeMenu(string id, List<IMenu> menus)
        {
            _id = id;
            _undrelyingControls = new List<IMenu>();
            foreach (IMenu menu in menus)
            {
                MergeWith(menu);
            }
        }

        public CompositeMenu(string id)
        {
            _id = id;
            _undrelyingControls = new List<IMenu>();
        }

        public override string Id
        {
            get { return _id; }
        }

        public override bool? IsEnabled
        {
            get { return true; }
        }

        public override bool? IsVisible
        {
            get { return true; }
        }

        public void MergeWith(IMenuControlCollection control)
        {
            if (!(control is IMenu))
            {
                throw new Exception();
            }
            _undrelyingControls.Add((IMenu)control);
            CompositeAggregator.MergeUnsafe(this, control);
        }

        public void MergeWith(IMenuControl control)
        {
            throw new NotImplementedException();
        }

        public override void Dispose()
        {
            foreach (IMenu menu in _undrelyingControls)
            {
                menu.Dispose();
            }
            _undrelyingControls.Clear();
            base.Dispose();
        }
    }
}
