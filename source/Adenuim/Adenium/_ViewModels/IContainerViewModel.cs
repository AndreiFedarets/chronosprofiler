using System;
using System.Collections.Generic;
using System.Collections.Specialized;

namespace Adenium
{
    public interface IContainerViewModel : IViewModel, IEnumerable<IViewModel>, INotifyCollectionChanged
    {
        event EventHandler<ViewModelEventArgs> ViewModelAttached;

        event EventHandler<ViewModelEventArgs> ViewModelDeattached;

        void ActivateItem(IViewModel viewModel);

        void DeactivateItem(IViewModel viewModel, bool close);

        T FindFirstChild<T>(Func<T, bool> condition = null) where T : IViewModel;
    }
}
