using Chronos.Client.Win.ViewModels;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;

namespace Chronos.Client.Win.Views
{
    public class GridView : PageView
    {
        public new GridViewModel ViewModel
        {
            get { return (GridViewModel)DataContext; }
        }

        protected override void OnViewModelChanged(ViewModel oldValue, ViewModel newValue)
        {
            ViewModel.CollectionChanged += OnViewModelCollectionChanged;
            ApplyLayout();
        }

        private void ApplyLayout()
        {
            ClearLayout();
            GridViewLayout layout = new GridViewLayout(ViewModel);
            layout.BuildGrid();
            System.Windows.Controls.Grid grid = new System.Windows.Controls.Grid();
            grid.HorizontalAlignment = System.Windows.HorizontalAlignment.Stretch;
            grid.VerticalAlignment = System.Windows.VerticalAlignment.Stretch;
            for (int row = 0; row < layout.Rows; row++)
            {
                System.Windows.Controls.RowDefinition rowDefinition = new System.Windows.Controls.RowDefinition();
                bool isFixedHeight = layout.GetIsFixedHeight(row);
                if (isFixedHeight)
                {
                    rowDefinition.Height = System.Windows.GridLength.Auto;
                }
                else
                {
                    rowDefinition.Height = new System.Windows.GridLength(1, System.Windows.GridUnitType.Star);
                }
                grid.RowDefinitions.Add(rowDefinition);
            }
            for (int column = 0; column < layout.Columns; column++)
            {
                System.Windows.Controls.ColumnDefinition columnDefinition = new System.Windows.Controls.ColumnDefinition();
                bool isFixedHeight = layout.GetIsFixedWidth(column);
                if (isFixedHeight)
                {
                    columnDefinition.Width = System.Windows.GridLength.Auto;
                }
                else
                {
                    columnDefinition.Width = new System.Windows.GridLength(1, System.Windows.GridUnitType.Star);
                }
                grid.ColumnDefinitions.Add(columnDefinition);
            }
            foreach (GridViewLayoutItem item in layout)
            {
                grid.Children.Add(item.View);
            }
            System.Windows.Controls.ContentControl contentControl = ViewsManager.FindViewContent(this);
            contentControl.Content = grid;
        }

        private void ClearLayout()
        {
            System.Windows.Controls.ContentControl contentControl = ViewsManager.FindViewContent(this);
            if (contentControl.Content == null)
            {
                return;
            }
            System.Windows.Controls.Grid grid = (System.Windows.Controls.Grid) contentControl.Content;
            contentControl.Content = null;
            grid.Children.Clear();
        }

        private void OnViewModelCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            ApplyLayout();
        }

        private sealed class GridViewLayout : IEnumerable<GridViewLayoutItem>
        {
            private readonly Dictionary<Guid, GridViewLayoutItem> _items;
            private List<GridViewLayoutItem> _processedItems;
            private List<GridViewLayoutItem> _residualItems;
            private GridViewLayoutItem _firstLayoutItem;

            public GridViewLayout(IEnumerable<ViewModel> viewModels)
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
                _residualItems = new List<GridViewLayoutItem>(_items.Values);
                List<GridViewLayoutItem> rootItems = TakeResidualItems(x => x.IsRoot);
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
                List<GridViewLayoutItem> items = TakeResidualItems(x => insertingItem.TypeId == x.AttachTo);
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

            private List<GridViewLayoutItem> TakeResidualItems(Func<GridViewLayoutItem, bool> filter)
            {
                List<GridViewLayoutItem> targetItems = _residualItems.Where(filter).ToList();
                foreach (GridViewLayoutItem targetItem in targetItems)
                {
                    _residualItems.Remove(targetItem);
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
            private View _view;

            public GridViewLayoutItem(ViewModel viewModel)
            {
                ViewModel = viewModel;
                RowSpan = 1;
                ColumnSpan = 1;
            }

            public ViewModel ViewModel { get; private set; }

            public View View
            {
                get
                {
                    if (_view == null)
                    {
                        _view = ViewsManager.LocateViewForModel(ViewModel);
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
                get { return ViewModel.TypeId; }
            }

            public bool IsRoot
            {
                get { return AttachTo == Guid.Empty; }
            }

            public int Row
            {
                get { return (int)View.GetValue(System.Windows.Controls.Grid.RowProperty); }
                set { View.SetValue(System.Windows.Controls.Grid.RowProperty, value); }
            }

            public int RowEnd
            {
                get { return Row + RowSpan - 1; }
            }

            public int RowSpan
            {
                get { return (int)View.GetValue(System.Windows.Controls.Grid.RowSpanProperty); }
                set { View.SetValue(System.Windows.Controls.Grid.RowSpanProperty, value); }
            }

            public int Column
            {
                get { return (int)View.GetValue(System.Windows.Controls.Grid.ColumnProperty); }
                set { View.SetValue(System.Windows.Controls.Grid.ColumnProperty, value); }
            }

            public int ColumnEnd
            {
                get { return Column + ColumnSpan - 1; }
            }

            public int ColumnSpan
            {
                get { return (int)View.GetValue(System.Windows.Controls.Grid.ColumnSpanProperty); }
                set { View.SetValue(System.Windows.Controls.Grid.ColumnSpanProperty, value); }
            }

            public bool IsFixedHeight
            {
                get { return !View.Height.IsNaNOrZero() || !View.MinHeight.IsNaNOrZero(); }
            }

            public bool IsFixedWidth
            {
                get { return !View.Width.IsNaNOrZero() || !View.MinWidth.IsNaNOrZero(); }
            }
        }
    }
}
