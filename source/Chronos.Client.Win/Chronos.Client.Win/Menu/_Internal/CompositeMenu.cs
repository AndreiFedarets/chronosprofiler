using System.Collections.Generic;

namespace Chronos.Client.Win.Menu
{
    internal sealed class CompositeMenu : ControlCollection, ICompositeControlCollection, IMenu
    {
        private readonly List<IMenu> _undrelyingControls;

        public CompositeMenu()
        {
            _undrelyingControls = new List<IMenu>();
        }

        public override string Id
        {
            get { return string.Empty; }
        }

        public override bool? IsEnabled
        {
            get { return true; }
        }

        public override bool? IsVisible
        {
            get { return true; }
        }

        public void MergeWith(IControlCollection control)
        {
            if (!(control is IMenu))
            {
                throw new TempException("Invalid control type");
            }
            _undrelyingControls.Add((IMenu)control);
            CompositeAggregator.MergeUnsafe(this, control);
        }

        public void MergeWith(IControl control)
        {
            throw new System.NotImplementedException();
        }

        public override void Dispose()
        {
            foreach (IMenu menu in _undrelyingControls)
            {
                menu.TryDispose();
            }
            _undrelyingControls.Clear();
            base.Dispose();
        }
    }
}
