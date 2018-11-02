using System;
using System.Collections.Generic;
using System.Collections.Specialized;

namespace Adenium
{
    public interface IContainerViewModel : IViewModel, IEnumerable<IViewModel>, INotifyCollectionChanged
    {
        event EventHandler<ViewModelEventArgs> ItemAttached;

        event EventHandler<ViewModelEventArgs> ItemDeattached;

        void ActivateItem(IViewModel viewModel);

        void DeactivateItem(IViewModel viewModel, bool close);

        void RemoveItems();

        T FindFirstChild<T>(Func<T, bool> condition = null) where T : IViewModel;
    }

    public static class ContainerViewModelExtensions
    {
        public static IViewModel ActivateItem(this IContainerViewModel containerViewModel, string childViewModelId)
        {
            return ViewModelManager.Instance.ActivateItem(containerViewModel, childViewModelId);
        }

        public static IViewModel ActivateItem<T1>(this IContainerViewModel containerViewModel, string childViewModelId, T1 dependency1)
        {
            return ViewModelManager.Instance.ActivateItem(containerViewModel, childViewModelId, dependency1);
        }

        public static IViewModel ActivateItem<T1, T2>(this IContainerViewModel containerViewModel, string childViewModelId, T1 dependency1, T2 dependency2)
        {
            return ViewModelManager.Instance.ActivateItem(containerViewModel, childViewModelId, dependency1, dependency2);
        }

        public static IViewModel ActivateItem<T1, T2, T3>(this IContainerViewModel containerViewModel, string childViewModelId, T1 dependency1, T2 dependency2, T3 dependency3)
        {
            return ViewModelManager.Instance.ActivateItem(containerViewModel, childViewModelId, dependency1, dependency2, dependency3);
        }
    }
}
