using System;
using System.Windows.Controls;

namespace Adenium
{
    internal sealed class GridViewItem
    {
            private readonly IViewModel _viewModel;
            private View _view;

            public GridViewItem(IViewModel viewModel)
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

            public string AttachTo
            {
                get { return View.AttachTo; }
            }

            public string ViewModelUid
            {
                get { return _viewModel.ViewModelUid; }
            }

            public bool IsRoot
            {
                get { return string.IsNullOrEmpty(AttachTo); }
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
