using System;
using Adenium.Layouting;

namespace Adenium
{
    public interface IViewModelManager
    {
        void RegisterLayoutProvider(ILayoutProvider layoutProvider);

        void ShowWindow(IViewModel viewModel);

        bool? ShowDialog(IViewModel viewModel);

        void ShowPopup(IViewModel viewModel);

        IViewModel CreateViewModel(Type viewModelType);

        IViewModel CreateViewModel(Type viewModelType, IContainerViewModel parentViewModel);

        IViewModel CreateViewModel<T1>(Type viewModelType, IContainerViewModel parentViewModel, T1 dependency1);

        IViewModel CreateViewModel<T1, T2>(Type viewModelType, IContainerViewModel parentViewModel, T1 dependency1, T2 dependency2);

        IViewModel CreateViewModel<T1, T2, T3>(Type viewModelType, IContainerViewModel parentViewModel, T1 dependency1, T2 dependency2, T3 dependency3);

        void BuildViewModelLayout(IViewModel viewModel);

        void ResetViewModelLayout(IViewModel viewModel);

        IViewModel ActivateItem(IContainerViewModel containerViewModel, string childViewModelId);

        IViewModel ActivateItem<T1>(IContainerViewModel containerViewModel, string childViewModelId, T1 dependency1);

        IViewModel ActivateItem<T1, T2>(IContainerViewModel containerViewModel, string childViewModelId, T1 dependency1, T2 dependency2);

        IViewModel ActivateItem<T1, T2, T3>(IContainerViewModel containerViewModel, string childViewModelId, T1 dependency1, T2 dependency2, T3 dependency3);
    }

    public static class ViewModelManagerExtensions
    {
        // CreateViewModel
        public static TViewModel CreateViewModel<TViewModel>(this IViewModelManager manager) where TViewModel : IViewModel
        {
            return (TViewModel)manager.CreateViewModel<object, object, object>(typeof(TViewModel), null, null, null, null);
        }

        public static TViewModel CreateViewModel<TViewModel>(this IViewModelManager manager, IContainerViewModel parentViewModel) where TViewModel : IViewModel
        {
            return (TViewModel)manager.CreateViewModel(typeof(TViewModel), parentViewModel);
        }

        public static TViewModel CreateViewModel<TViewModel, T1>(this IViewModelManager manager, IContainerViewModel parentViewModel, T1 dependency1) where TViewModel : IViewModel
        {
            return (TViewModel)manager.CreateViewModel(typeof(TViewModel), parentViewModel, dependency1);
        }

        public static TViewModel CreateViewModel<TViewModel, T1, T2>(this IViewModelManager manager, IContainerViewModel parentViewModel, T1 dependency1, T2 dependency2) where TViewModel : IViewModel
        {
            return (TViewModel)manager.CreateViewModel(typeof(TViewModel), parentViewModel, dependency1, dependency2);
        }

        public static TViewModel CreateViewModel<TViewModel, T1, T2, T3>(this IViewModelManager manager, IContainerViewModel parentViewModel, T1 dependency1, T2 dependency2, T3 dependency3) where TViewModel : IViewModel
        {
            return (TViewModel)manager.CreateViewModel(typeof(TViewModel), parentViewModel, dependency1, dependency2, dependency3);
        }

        //ShowDialog
        public static bool? ShowDialog<TViewModel>(this IViewModelManager manager) where TViewModel : IViewModel
        {
            TViewModel viewModel = manager.CreateViewModel<TViewModel>();
            return manager.ShowDialog(viewModel);
        }

        public static bool? ShowDialog<TViewModel>(this IViewModelManager manager, IContainerViewModel parentViewModel) where TViewModel : IViewModel
        {
            TViewModel viewModel = manager.CreateViewModel<TViewModel>(parentViewModel);
            return manager.ShowDialog(viewModel);
        }

        public static bool? ShowDialog<TViewModel, T1>(this IViewModelManager manager, IContainerViewModel parentViewModel,  T1 dependency1) where TViewModel : IViewModel
        {
            TViewModel viewModel = manager.CreateViewModel<TViewModel, T1>(parentViewModel, dependency1);
            return manager.ShowDialog(viewModel);
        }

        public static bool? ShowDialog<TViewModel, T1, T2>(this IViewModelManager manager, IContainerViewModel parentViewModel, T1 dependency1, T2 dependency2) where TViewModel : IViewModel
        {
            TViewModel viewModel = manager.CreateViewModel<TViewModel, T1, T2>(parentViewModel, dependency1, dependency2);
            return manager.ShowDialog(viewModel);
        }

        public static bool? ShowDialog<TViewModel, T1, T2, T3>(this IViewModelManager manager, IContainerViewModel parentViewModel, T1 dependency1, T2 dependency2, T3 dependency3) where TViewModel : IViewModel
        {
            TViewModel viewModel = manager.CreateViewModel<TViewModel, T1, T2, T3>(parentViewModel, dependency1, dependency2, dependency3);
            return manager.ShowDialog(viewModel);
        }
    }
}
