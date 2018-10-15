using Chronos.Client.Win.ViewModels;
using System.Windows;

namespace Chronos.Client.Win.Views
{
    public class PlaceholderView : View
    {
        public PlaceholderView()
        {
            DataContextChanged += OnDataContextChanged;
            DisplayPanel = true;
        }

        private void OnDataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            PlaceholderViewModel viewModel = ViewModel as PlaceholderViewModel;
            if (viewModel != null)
            {
                viewModel.UnderlyingViewModelChanged -= OnUnderlyingViewModelChanged;
            }
            viewModel = e.NewValue as PlaceholderViewModel;
            if (viewModel != null)
            {
                viewModel.UnderlyingViewModelChanged += OnUnderlyingViewModelChanged;
            }
            RenderUnderlyingViewModel();
        }

        private void OnUnderlyingViewModelChanged(object sender, System.EventArgs e)
        {
            RenderUnderlyingViewModel();
        }

        private void RenderUnderlyingViewModel()
        {
            Content = null;
            PlaceholderViewModel viewModel = ViewModel as PlaceholderViewModel;
            if (viewModel == null)
            {
                return;
            }
            ViewModel underlyingViewModel = viewModel.UnderlyingViewModel;
            if (underlyingViewModel == null)
            {
                return;
            }
            View view = ViewsManager.LocateViewForModel(underlyingViewModel);
            if (view != null)
            {
                view.DisplayPanel = false;
                view.Margin = new Thickness();
                view.Padding = new Thickness();
                view.Style = null;
                Content = view;
                CloneViewProperties(view);
            }
        }

        private void CloneViewProperties(View view)
        {
            if (!view.ContentMinHeight.IsNaNOrZero())
            {
                ContentMinHeight = view.ContentMinHeight;   
            }
            if (!view.ContentHeight.IsNaNOrZero())
            {
                ContentHeight = view.ContentHeight;
            }
            if (!view.ContentMaxHeight.IsNaNOrZero())
            {
                ContentMaxHeight = view.ContentMaxHeight;
            }

            if (!view.ContentMinWidth.IsNaNOrZero())
            {
                ContentMinWidth = view.ContentMinWidth;
            }
            if (!view.ContentWidth.IsNaNOrZero())
            {
                ContentWidth = view.ContentWidth;
            }
            if (!view.ContentMaxWidth.IsNaNOrZero())
            {
                ContentMaxWidth = view.ContentMaxWidth;
            }
        }
    }
}
