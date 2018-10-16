using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Chronos.Client.Win.ViewModels;

namespace Chronos.Client.Win.Views
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
            GridViewLayout layout = new GridViewLayout(_viewModel);
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
            foreach (GridViewLayoutItem item in layout)
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

        private sealed class GridViewLayout : IEnumerable<GridViewLayoutItem>
        {
            private readonly Dictionary<Guid, GridViewLayoutItem> _items;
            private List<GridViewLayoutItem> _processedItems;
            private List<GridViewLayoutItem> _remainingItems;
            private GridViewLayoutItem _firstLayoutItem;

            public GridViewLayout(IEnumerable<IViewModel> viewModels)
            {
                IEnumerable<GridViewLayoutItem> cells = viewModels.Select(x => new GridViewLayoutItem(x));
                _items = cells.ToDictionary(x => x.TypeId, x => x);
            }

            public int Columns
            {
                get
                {
                    if (_processedItems == null)
                    {
                        return 0;
                    }
                    int columnEnd = _processedItems.Max(x => x.ColumnEnd);
                    return columnEnd + 1;
                }
            }

            public int Rows
            {
                get
                {
                    if (_processedItems == null)
                    {
                        return 0;
                    }
                    int rowEnd = _processedItems.Max(x => x.RowEnd);
                    return rowEnd + 1;
                }
            }

            public bool GetIsFixedHeight(int row)
            {
                for (int column = 0; column < Columns; column++)
                {
                    GridViewLayoutItem item = GetLayoutItem(row, column);
                    if (item != null && item.IsFixedHeight)
                    {
                        return true;
                    }
                }
                return false;
            }

            public bool GetIsFixedWidth(int column)
            {
                for (int row = 0; row < Rows; row++)
                {
                    GridViewLayoutItem item = GetLayoutItem(row, column);
                    if (item != null && item.IsFixedWidth)
                    {
                        return true;
                    }
                }
                return false;
            }

            public void BuildGrid()
            {
                if (_items.Count == 0)
                {
                    return;
                }
                _firstLayoutItem = null;
                _processedItems = new List<GridViewLayoutItem>();
                _remainingItems = new List<GridViewLayoutItem>(_items.Values);
                List<GridViewLayoutItem> rootItems = TakeRemainingItems(x => x.IsRoot);
                foreach (GridViewLayoutItem item in rootItems)
                {
                    InsertItem(item);
                }
                RecalculateIndexes();
                AdjustAlignment();
            }

            private void InsertItem(GridViewLayoutItem insertingItem)
            {
                if (insertingItem.IsRoot)
                {
                    InsertRootItem(insertingItem);
                }
                else
                {
                    InsertUsualItem(insertingItem);
                }
                _processedItems.Add(insertingItem);
                List<GridViewLayoutItem> items = TakeRemainingItems(x => insertingItem.TypeId == x.AttachTo);
                foreach (GridViewLayoutItem item in items)
                {
                    InsertItem(item);
                }
            }

            private void InsertRootItem(GridViewLayoutItem insertingItem)
            {
                //if this is the first root cell
                if (_processedItems.Count == 0)
                {
                    insertingItem.Row = 0;
                    insertingItem.Column = 0;
                    _firstLayoutItem = insertingItem;
                }
                else
                {
                    insertingItem.Column = _firstLayoutItem.Column;
                    int rowStart = _firstLayoutItem.RowEnd + 1;
                    while (!IsCellEmpty(rowStart, insertingItem.Column))
                    {
                        rowStart++;
                    }
                    insertingItem.Row = rowStart;
                }
            }

            private void InsertUsualItem(GridViewLayoutItem insertingItem)
            {
                GridViewLayoutItem targetItem;
                if (!_items.TryGetValue(insertingItem.AttachTo, out targetItem))
                {
                    //TODO: There is no target cell, warning!!!
                    return;
                }
                Position position = insertingItem.ViewPosition;
                switch (position)
                {
                    case Position.Left:
                        InsertLeftItem(targetItem, insertingItem);
                        break;
                    case Position.Top:
                        InsertTopItem(targetItem, insertingItem);
                        break;
                    case Position.Right:
                        InsertRightItem(targetItem, insertingItem);
                        break;
                    case Position.Bottom:
                        InsertBottomItem(targetItem, insertingItem);
                        break;
                    case Position.Default:
                        InsertBottomItem(targetItem, insertingItem);
                        break;
                }
            }

            private void InsertLeftItem(GridViewLayoutItem targetItem, GridViewLayoutItem insertingItem)
            {
                insertingItem.Column = targetItem.Column - 1;
                insertingItem.Row = targetItem.Row;
                bool isCellEmpty = IsCellEmpty(insertingItem.Row, insertingItem.Column);
                if (!isCellEmpty)
                {
                    //Move all cell that at the left
                    foreach (GridViewLayoutItem item in _processedItems)
                    {
                        if (item.Column <= insertingItem.Column)
                        {
                            item.Column--;
                        }
                    }
                }
            }

            private void InsertTopItem(GridViewLayoutItem targetItem, GridViewLayoutItem insertingItem)
            {
                insertingItem.Column = targetItem.Column;
                insertingItem.Row = targetItem.Row - 1;
                bool isCellEmpty = IsCellEmpty(insertingItem.Row, insertingItem.Column);
                if (!isCellEmpty)
                {
                    //Move all cell that at the top
                    foreach (GridViewLayoutItem item in _processedItems)
                    {
                        if (item.Row <= insertingItem.Row)
                        {
                            item.Row--;
                        }
                    }
                }
            }

            private void InsertRightItem(GridViewLayoutItem targetItem, GridViewLayoutItem insertingItem)
            {
                insertingItem.Column = targetItem.Column + 1;
                insertingItem.Row = targetItem.Row;
                bool isCellEmpty = IsCellEmpty(insertingItem.Row, insertingItem.Column);
                if (!isCellEmpty)
                {
                    //Move all cell that at the left
                    foreach (GridViewLayoutItem item in _processedItems)
                    {
                        if (item.Column >= insertingItem.Column)
                        {
                            item.Column++;
                        }
                    }
                }
            }

            private void InsertBottomItem(GridViewLayoutItem targetItem, GridViewLayoutItem insertingItem)
            {
                insertingItem.Column = targetItem.Column;
                insertingItem.Row = targetItem.Row + 1;
                bool isCellEmpty = IsCellEmpty(insertingItem.Row, insertingItem.Column);
                if (!isCellEmpty)
                {
                    //Move all cell that at the top
                    foreach (GridViewLayoutItem item in _processedItems)
                    {
                        if (item.Row >= insertingItem.Row)
                        {
                            item.Row++;
                        }
                    }
                }
            }

            private bool IsCellEmpty(int row, int column)
            {
                GridViewLayoutItem item = GetLayoutItem(row, column);
                return item == null;
            }

            private GridViewLayoutItem GetLayoutItem(int row, int column)
            {
                foreach (GridViewLayoutItem item in _processedItems)
                {
                    for (int rowSpan = 0; rowSpan < item.RowSpan; rowSpan++)
                    {
                        for (int columnSpan = 0; columnSpan < item.ColumnSpan; columnSpan++)
                        {
                            if (item.Row + rowSpan == row && item.Column + columnSpan == column)
                            {
                                return item;
                            }
                        }
                    }
                }
                return null;
            }

            private List<GridViewLayoutItem> TakeRemainingItems(Func<GridViewLayoutItem, bool> filter)
            {
                List<GridViewLayoutItem> targetItems = _remainingItems.Where(filter).ToList();
                foreach (GridViewLayoutItem targetItem in targetItems)
                {
                    _remainingItems.Remove(targetItem);
                }
                return targetItems;
            }

            private void AdjustAlignment()
            {
                //Adjust horizontal alignment
                foreach (GridViewLayoutItem item in _processedItems)
                {
                    if (!item.IsFixedWidth)
                    {
                        AdjustHorizontalAlignment(item);
                    }
                }
                //Adjust vertical alignment
                foreach (GridViewLayoutItem item in _processedItems)
                {
                    if (!item.IsFixedHeight)
                    {
                        AdjustVerticalAlignment(item);
                    }
                }
                ////Adjust auto alignment
                //foreach (GridViewLayoutItem item in _processedItems)
                //{
                //    if (item.ViewAlignment == Alignment.Auto)
                //    {
                //        AdjustHorizontalAlignment(item);
                //    }
                //}
            }

            private void AdjustHorizontalAlignment(GridViewLayoutItem item)
            {
                int columnsEnd = Columns - 1;
                //adjust left
                while (item.Column > 0 && IsCellEmpty(item.Row, item.Column - 1))
                {
                    item.Column--;
                    item.ColumnSpan++;
                }
                //adjust right
                while (item.ColumnEnd < columnsEnd && IsCellEmpty(item.Row, item.Column + item.ColumnSpan))
                {
                    item.ColumnSpan++;
                }
            }

            private void AdjustVerticalAlignment(GridViewLayoutItem item)
            {
                int rowsEnd = Rows - 1;
                //adjust top
                while (item.Row > 0 && IsCellEmpty(item.Row - 1, item.Column))
                {
                    item.Row--;
                    item.RowSpan++;
                }
                //adjust bottom
                while (item.RowEnd < rowsEnd && IsCellEmpty(item.Row + item.RowSpan, item.Column))
                {
                    item.RowSpan++;
                }
            }

            private void RecalculateIndexes()
            {
                int minRow = _processedItems.Min(x => x.Row);
                int minColumn = _processedItems.Min(x => x.Column);
                int rowOffset = Math.Abs(minRow);
                int columnOffset = Math.Abs(minColumn);
                foreach (GridViewLayoutItem cell in _processedItems)
                {
                    cell.Row += rowOffset;
                    cell.Column += columnOffset;
                }
            }

            public IEnumerator<GridViewLayoutItem> GetEnumerator()
            {
                return _items.Values.GetEnumerator();
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }
        }

        private sealed class GridViewLayoutItem
        {
            private readonly IViewModel _viewModel;
            private View _view;

            public GridViewLayoutItem(IViewModel viewModel)
            {
                _viewModel = viewModel;
                RowSpan = 1;
                ColumnSpan = 1;
            }

            public View View
            {
                get
                {
                    if (_view == null)
                    {
                        _view = ViewsManager.LocateViewForModel(_viewModel);
                    }
                    return _view;
                }
            }

            public Position ViewPosition
            {
                get { return View.ViewPosition; }
            }

            public Guid AttachTo
            {
                get { return View.AttachTo; }
            }

            public Guid TypeId
            {
                get { return _viewModel.TypeId; }
            }

            public bool IsRoot
            {
                get { return AttachTo == Guid.Empty; }
            }

            public int Row
            {
                get { return (int)View.GetValue(Grid.RowProperty); }
                set { View.SetValue(Grid.RowProperty, value); }
            }

            public int RowEnd
            {
                get { return Row + RowSpan - 1; }
            }

            public int RowSpan
            {
                get { return (int)View.GetValue(Grid.RowSpanProperty); }
                set { View.SetValue(Grid.RowSpanProperty, value); }
            }

            public int Column
            {
                get { return (int)View.GetValue(Grid.ColumnProperty); }
                set { View.SetValue(Grid.ColumnProperty, value); }
            }

            public int ColumnEnd
            {
                get { return Column + ColumnSpan - 1; }
            }

            public int ColumnSpan
            {
                get { return (int)View.GetValue(Grid.ColumnSpanProperty); }
                set { View.SetValue(Grid.ColumnSpanProperty, value); }
            }

            public bool IsFixedHeight
            {
                get { return !View.Height.IsNaNOrZero() || !View.MinHeight.IsNaNOrZero() || !View.ContentHeight.IsNaNOrZero() || !View.ContentMinHeight.IsNaNOrZero(); }
            }

            public bool IsFixedWidth
            {
                get { return !View.Width.IsNaNOrZero() || !View.MinWidth.IsNaNOrZero() || !View.ContentWidth.IsNaNOrZero() || !View.ContentMinWidth.IsNaNOrZero(); }
            }
        }
    }
}
