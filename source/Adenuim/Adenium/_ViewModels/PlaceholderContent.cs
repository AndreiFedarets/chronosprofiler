using System;

namespace Adenium
{
    public abstract class PlaceholderContent
    {
        public abstract string DisplayName { get; }

        public abstract IViewModel CreateViewModel();

        public event EventHandler ViewModelChanged;

        protected void NotifyViewModelChanged()
        {
            EventHandler handler = ViewModelChanged;
            if (handler != null)
            {
                handler(this, EventArgs.Empty);
            }
        }
    }
}
