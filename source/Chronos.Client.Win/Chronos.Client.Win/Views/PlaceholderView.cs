using Chronos.Client.Win.ViewModels;
using System.Windows;

namespace Chronos.Client.Win.Views
{
    public class PlaceholderView : View
    {
        public PlaceholderView()
        {
            DisplayPanel = true;
            DataContextChanged += OnDataContextChanged;
        }

        private void OnDataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            PlaceholderViewModel viewModel = e.NewValue as PlaceholderViewModel;
            View view = null;
            if (viewModel != null)
            {
                view = ViewsManager.LocateViewForModel(viewModel.UnderlyingViewModel);
            }
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
