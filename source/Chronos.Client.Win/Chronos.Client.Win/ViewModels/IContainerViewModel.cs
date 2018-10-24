using System;
using System.Collections.Generic;
using System.Collections.Specialized;

namespace Chronos.Client.Win.ViewModels
{
    public interface IContainerViewModel : IViewModel, IEnumerable<IViewModel>, INotifyCollectionChanged
    {
        event EventHandler<ViewModelEventArgs> ViewModelAttached;

        event EventHandler<ViewModelEventArgs> ViewModelDeattached;

        void ActivateItem(IViewModel viewModel);

        void DeactivateItem(IViewModel viewModel, bool close);

        T FindFirstChild<T>() where T : IViewModel;
    }
}
