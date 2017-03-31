using System;
namespace Chronos.Client.Win.ViewModels
{
    public interface IViewModelManager
    {
        event EventHandler<ViewModelEventArgs> ViewAttached;

        event EventHandler<ViewModelEventArgs> ViewDeattached;

        void ShowWindow(ViewModel viewModel);

        bool? ShowDialog(ViewModel viewModel);

        void ShowPopup(ViewModel viewModel);
    }

    public static class ViewModelManagerExtensions
    {
        //public static ViewModel ShowWindow(this IViewModelManager manager, string id, ILayoutScope scope, ResolutionDependencies dependencies)
        //{
        //    ViewModel viewModel = manager.BuildViewModel(id, scope, dependencies);
        //    manager.ShowWindow(viewModel);
        //    return viewModel;
        //}

        //public static ViewModel ShowWindow(this IViewModelManager manager, TextReader reader, ILayoutScope scope, ResolutionDependencies dependencies)
        //{
        //    ViewModel viewModel = manager.BuildViewModel(reader, scope, dependencies);
        //    manager.ShowWindow(viewModel);
        //    return viewModel;
        //}

        //public static bool? ShowDialog(this IViewModelManager manager, string id, ILayoutScope scope, ResolutionDependencies dependencies)
        //{
        //    ViewModel viewModel = manager.BuildViewModel(id, scope, dependencies);
        //    return manager.ShowDialog(viewModel);
        //}

        //public static bool? ShowDialog(this IViewModelManager manager, TextReader reader, ILayoutScope scope, ResolutionDependencies dependencies)
        //{
        //    ViewModel viewModel = manager.BuildViewModel(reader, scope, dependencies);
        //    return manager.ShowDialog(viewModel);
        //}

        //public static void ShowPopup(this IViewModelManager manager, string id, ILayoutScope scope, ResolutionDependencies dependencies)
        //{
        //    ViewModel viewModel = manager.BuildViewModel(id, scope, dependencies);
        //    manager.ShowPopup(viewModel);
        //}

        //public static void ShowPopup(this IViewModelManager manager, TextReader reader, ILayoutScope scope, ResolutionDependencies dependencies)
        //{
        //    ViewModel viewModel = manager.BuildViewModel(reader, scope, dependencies);
        //    manager.ShowPopup(viewModel);
        //}
    }

}
