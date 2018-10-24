using System.Collections.Specialized;
using System.Windows;
using System.Windows.Controls;

namespace Adenium
{
    public sealed class GridViewBehaviorExtension : IViewBehaviorExtension
    {
        private readonly View _view;
        private readonly GridViewModel _viewModel;

        public GridViewBehaviorExtension(View view, GridViewModel viewModel)
        {
            _view = view;
            _viewModel = viewModel;
        }

        public void Initialize()
        {
            BuildLayout();
            _viewModel.Items.CollectionChanged += OnViewModelCollectionChanged;
        }

        public void Dispose()
        {
            _viewModel.Items.CollectionChanged -= OnViewModelCollectionChanged;
            ClearLayout();
        }

        private void ClearLayout()
        {
            ContentControl contentControl = ViewsManager.FindViewContent(_view);
            if (contentControl.Content == null)
            {
                return;
            }
            Grid control = (Grid)contentControl.Content;
            contentControl.Content = null;
            control.Children.Clear();
        }

        private void BuildLayout()
        {
            GridViewBuilder layout = new GridViewBuilder(_viewModel);
            layout.BuildGrid();
            Grid grid = new Grid();
            grid.HorizontalAlignment = HorizontalAlignment.Stretch;
            grid.VerticalAlignment = VerticalAlignment.Stretch;
            if (layout.Rows > 1)
            {
                for (int row = 0; row < layout.Rows; row++)
                {
                    RowDefinition rowDefinition = new RowDefinition();
                    bool isFixedHeight = layout.GetIsFixedHeight(row);
                    if (isFixedHeight)
                    {
                        rowDefinition.Height = GridLength.Auto;
                    }
                    else
                    {
                        rowDefinition.Height = new GridLength(1, GridUnitType.Star);
                    }
                    grid.RowDefinitions.Add(rowDefinition);
                }
            }
            if (layout.Columns > 1)
            {
                for (int column = 0; column < layout.Columns; column++)
                {
                    ColumnDefinition columnDefinition = new ColumnDefinition();
                    bool isFixedHeight = layout.GetIsFixedWidth(column);
                    if (isFixedHeight)
                    {
                        columnDefinition.Width = GridLength.Auto;
                    }
                    else
                    {
                        columnDefinition.Width = new GridLength(1, GridUnitType.Star);
                    }
                    grid.ColumnDefinitions.Add(columnDefinition);
                }
            }
            foreach (GridViewItem item in layout)
            {
                grid.Children.Add(item.View);
            }
            ContentControl contentControl = ViewsManager.FindViewContent(_view);
            contentControl.Content = grid;
        }

        private void OnViewModelCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            ClearLayout();
            BuildLayout();
        }
    }
}
