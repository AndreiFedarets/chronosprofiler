using System;
using System.Collections.Generic;
using System.Linq;

namespace Adenium.Menu
{
    internal abstract class CompositeControl<T> : MenuControl, ICompositeControl where T : IMenuControl
    {
        protected CompositeControl(T control)
        {
            UndrelyingControls = new List<T>();
            MergeWith(control);
        }

        protected List<T> UndrelyingControls { get; private set; }

        public sealed override string Id
        {
            get { return UndrelyingControls.First().Id; }
        }

        public sealed override bool? IsEnabled
        {
            get { return CompositeAggregator.GetIsEnabled(UndrelyingControls.Select(x => (IMenuControl)x)); }
            protected set { }
        }

        public sealed override bool? IsVisible
        {
            get { return CompositeAggregator.GetIsVisible(UndrelyingControls.Select(x => (IMenuControl)x)); }
            protected set { }
        }

        public void MergeWith(IMenuControl control)
        {
            if (!(control is T))
            {
                throw new Exception("Invalid control type");
            }
            if (UndrelyingControls.Count > 0 && !string.Equals(Id, control.Id, StringComparison.Ordinal))
            {
                throw new Exception("Invalid Id");
            }
            UndrelyingControls.Add((T)control);
        }

        public override void Dispose()
        {
            foreach (T control in UndrelyingControls)
            {
                control.Dispose();
            }
            UndrelyingControls.Clear();
            base.Dispose();
        }
    }
}
