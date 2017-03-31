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
            }
            Content = view;
            CloneViewProperties(view);
        }

        private void CloneViewProperties(View view)
        {
            MinHeight = view.MinHeight;
            Height = view.Height;
            MaxHeight = view.MaxHeight;

            MinWidth = view.MinWidth;
            Width = view.Width;
            MaxWidth = view.MaxWidth;
        }
    }
}
