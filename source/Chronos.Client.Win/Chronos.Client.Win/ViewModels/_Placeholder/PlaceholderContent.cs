using System;

namespace Chronos.Client.Win.ViewModels
{
    public abstract class PlaceholderContent
    {
        public abstract string DisplayName { get; }

        public abstract ViewModel CreateViewModel();

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
