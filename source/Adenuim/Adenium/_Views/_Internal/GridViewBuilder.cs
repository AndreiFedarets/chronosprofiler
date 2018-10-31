using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Adenium
{
    internal sealed class GridViewBuilder : IEnumerable<GridViewItem>
    {
        private readonly Dictionary<string, GridViewItem> _items;
        private List<GridViewItem> _processedItems;
        private List<GridViewItem> _remainingItems;
        private GridViewItem _firstLayoutItem;

        public GridViewBuilder(IEnumerable<IViewModel> viewModels)
        {
            IEnumerable<GridViewItem> cells = viewModels.Select(x => new GridViewItem(x));
            _items = cells.ToDictionary(x => x.ViewModelUid, x => x);
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
                GridViewItem item = GetLayoutItem(row, column);
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
                GridViewItem item = GetLayoutItem(row, column);
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
            _processedItems = new List<GridViewItem>();
            _remainingItems = new List<GridViewItem>(_items.Values);
            List<GridViewItem> rootItems = TakeRemainingItems(x => x.IsRoot);
            foreach (GridViewItem item in rootItems)
            {
                InsertItem(item);
            }
            RecalculateIndexes();
            AdjustAlignment();
        }

        private void InsertItem(GridViewItem insertingItem)
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
            List<GridViewItem> items = TakeRemainingItems(x => string.Equals(insertingItem.ViewModelUid, x.AttachTo, StringComparison.OrdinalIgnoreCase));
            foreach (GridViewItem item in items)
            {
                InsertItem(item);
            }
        }

        private void InsertRootItem(GridViewItem insertingItem)
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


        private void InsertUsualItem(GridViewItem insertingItem)
        {
            GridViewItem targetItem;
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

        private void InsertLeftItem(GridViewItem targetItem, GridViewItem insertingItem)
        {
            insertingItem.Column = targetItem.Column - 1;
            insertingItem.Row = targetItem.Row;
            bool isCellEmpty = IsCellEmpty(insertingItem.Row, insertingItem.Column);
            if (!isCellEmpty)
            {
                //Move all cell that at the left
                foreach (GridViewItem item in _processedItems)
                {
                    if (item.Column <= insertingItem.Column)
                    {
                        item.Column--;
                    }
                }
            }
        }

        private void InsertTopItem(GridViewItem targetItem, GridViewItem insertingItem)
        {
            insertingItem.Column = targetItem.Column;
            insertingItem.Row = targetItem.Row - 1;
            bool isCellEmpty = IsCellEmpty(insertingItem.Row, insertingItem.Column);
            if (!isCellEmpty)
            {
                //Move all cell that at the top
                foreach (GridViewItem item in _processedItems)
                {
                    if (item.Row <= insertingItem.Row)
                    {
                        item.Row--;
                    }
                }
            }
        }

        private void InsertRightItem(GridViewItem targetItem, GridViewItem insertingItem)
        {
            insertingItem.Column = targetItem.Column + 1;
            insertingItem.Row = targetItem.Row;
            bool isCellEmpty = IsCellEmpty(insertingItem.Row, insertingItem.Column);
            if (!isCellEmpty)
            {
                //Move all cell that at the left
                foreach (GridViewItem item in _processedItems)
                {
                    if (item.Column >= insertingItem.Column)
                    {
                        item.Column++;
                    }
                }
            }
        }

        private void InsertBottomItem(GridViewItem targetItem, GridViewItem insertingItem)
        {
            insertingItem.Column = targetItem.Column;
            insertingItem.Row = targetItem.Row + 1;
            bool isCellEmpty = IsCellEmpty(insertingItem.Row, insertingItem.Column);
            if (!isCellEmpty)
            {
                //Move all cell that at the top
                foreach (GridViewItem item in _processedItems)
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
            GridViewItem item = GetLayoutItem(row, column);
            return item == null;
        }

        private GridViewItem GetLayoutItem(int row, int column)
        {
            foreach (GridViewItem item in _processedItems)
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

        private List<GridViewItem> TakeRemainingItems(Func<GridViewItem, bool> filter)
        {
            List<GridViewItem> targetItems = _remainingItems.Where(filter).ToList();
            foreach (GridViewItem targetItem in targetItems)
            {
                _remainingItems.Remove(targetItem);
            }
            return targetItems;
        }

        private void AdjustAlignment()
        {
            //Adjust horizontal alignment
            foreach (GridViewItem item in _processedItems)
            {
                if (!item.IsFixedWidth)
                {
                    AdjustHorizontalAlignment(item);
                }
            }
            //Adjust vertical alignment
            foreach (GridViewItem item in _processedItems)
            {
                if (!item.IsFixedHeight)
                {
                    AdjustVerticalAlignment(item);
                }
            }
            ////Adjust auto alignment
            //foreach (GridViewItem item in _processedItems)
            //{
            //    if (item.ViewAlignment == Alignment.Auto)
            //    {
            //        AdjustHorizontalAlignment(item);
            //    }
            //}
        }

        private void AdjustHorizontalAlignment(GridViewItem item)
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

        private void AdjustVerticalAlignment(GridViewItem item)
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
            foreach (GridViewItem cell in _processedItems)
            {
                cell.Row += rowOffset;
                cell.Column += columnOffset;
            }
        }

        public IEnumerator<GridViewItem> GetEnumerator()
        {
            return _items.Values.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
