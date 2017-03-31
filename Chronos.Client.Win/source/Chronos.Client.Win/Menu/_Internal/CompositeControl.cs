using System;
using System.Collections.Generic;
using System.Linq;

namespace Chronos.Client.Win.Menu
{
    internal abstract class CompositeControl<T> : Control, ICompositeControl where T : IControl
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
            get { return CompositeAggregator.GetIsEnabled(UndrelyingControls.Select(x => (IControl)x)); }
            protected set { }
        }

        public sealed override bool? IsVisible
        {
            get { return CompositeAggregator.GetIsVisible(UndrelyingControls.Select(x => (IControl)x)); }
            protected set { }
        }

        public void MergeWith(IControl control)
        {
            if (!(control is T))
            {
                throw new TempException("Invalid control type");
            }
            if (UndrelyingControls.Count > 0 && !string.Equals(Id, control.Id, StringComparison.Ordinal))
            {
                throw new TempException("Invalid Id");
            }
            UndrelyingControls.Add((T)control);
        }

        public override void Dispose()
        {
            foreach (T control in UndrelyingControls)
            {
                control.TryDispose();
            }
            UndrelyingControls.Clear();
            base.Dispose();
        }
    }
}
