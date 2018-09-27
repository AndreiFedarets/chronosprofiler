using Chronos.Client.Win.ViewModels;
using System.Windows;

namespace Chronos.Client.Win.Views
{
    public abstract class PageView : View
    {
        protected PageView()
        {
            DataContextChanged += OnDataContextChanged;
        }

        private void OnDataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (e.OldValue == null && e.NewValue == null)
            {
                return;
            }
            OnViewModelChanged((ViewModel)e.OldValue, (ViewModel)e.NewValue);
        }

        protected abstract void OnViewModelChanged(ViewModel oldValue, ViewModel newValue);
    }
}
