using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using Caliburn.Micro;
using System.ComponentModel;

namespace Adenium
{
    public class GridViewDynamicColumn : PropertyChangedBase
    {
        private readonly Func<object, string, bool> _filterFunc;
        private readonly DelayedNotification<string> _filterValue;
        private readonly string _propertyName;
        private ICollectionView _collectionView;
        private GridViewColumn _column;
        private BindingBase _displayMemberBinding;
        private object _header;

        public GridViewDynamicColumn(object header, string propertyName)
            : this(header, propertyName, null)
        {
        }

        public GridViewDynamicColumn(object header, string propertyName, Func<object, string, bool> filterFunc)
        {
            _header = header;
            _displayMemberBinding = new Binding(propertyName);
            _propertyName = propertyName;
            _filterFunc = filterFunc;
            _filterValue = new DelayedNotification<string>(OnFilterValueChanged, true);
        }

        public object Header
        {
            get { return _header; }
            set
            {
                _header = value;
                NotifyOfPropertyChange(() => Header);
            }
        }

        public BindingBase DisplayMemberBinding
        {
            get { return _displayMemberBinding; }
            set
            {
                _displayMemberBinding = value;
                NotifyOfPropertyChange(() => DisplayMemberBinding);
            }
        }

        public string FilterValue
        {
            get { return _filterValue.GetValue(); }
            set { _filterValue.SetValue(value); }
        }

        public bool SupportsFiltering
        {
            get { return _collectionView != null && _filterFunc != null; }
        }

        private void OnFilterValueChanged(string value)
        {
            NotifyOfPropertyChange(() => FilterValue);
            _collectionView.Filter = FilterInternal;
        }

        private bool FilterInternal(object item)
        {
            return _filterFunc(item, _filterValue.GetValue());
        }

        internal void Bind(GridViewColumn column)
        {
            Unbind();
            _column = column;
            _column.DisplayMemberBinding = DisplayMemberBinding;
            //Header
            Binding binding = new Binding("Header") {Source = this};
            BindingOperations.SetBinding(_column, GridViewColumn.HeaderProperty, binding);
            //Sorting
            GridViewSorting.SetPropertyName(column, _propertyName);
        }

        internal void Unbind()
        {
            if (_column != null)
            {
                BindingOperations.ClearBinding(_column, GridViewColumn.HeaderProperty);
            }
        }

        public void AttachCollectionView(ICollectionView collectionView)
        {
            _collectionView = collectionView;
        }
    }

    public static class GridViewComposition
    {
        public static readonly DependencyProperty DynamicColumnsProperty;

        static GridViewComposition()
        {
            DynamicColumnsProperty = DependencyProperty.RegisterAttached("DynamicColumns", typeof(IEnumerable<GridViewDynamicColumn>), typeof(GridViewComposition), new UIPropertyMetadata(null, OnDynamicColumnsPropertyChanged));
        }

        public static IEnumerable<GridViewDynamicColumn> GetDynamicColumns(DependencyObject obj)
        {
            return (IEnumerable<GridViewDynamicColumn>)obj.GetValue(DynamicColumnsProperty);
        }

        public static void SetDynamicColumns(DependencyObject obj, IEnumerable<GridViewDynamicColumn> value)
        {
            obj.SetValue(DynamicColumnsProperty, value);
        }

        private static void OnDynamicColumnsPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            INotifyCollectionChanged collectionChanged = e.OldValue as INotifyCollectionChanged;
            if (collectionChanged != null)
            {
                collectionChanged.CollectionChanged -= OnColumnsCollectionChanged;
            }
            GridView gridView = (GridView)sender;
            gridView.Columns.Clear();
            IEnumerable<GridViewDynamicColumn> dynamicColumns = (IEnumerable<GridViewDynamicColumn>)e.NewValue;
            if (dynamicColumns == null)
            {
                return;
            }
            collectionChanged = e.NewValue as INotifyCollectionChanged;
            if (collectionChanged != null)
            {
                collectionChanged.CollectionChanged += OnColumnsCollectionChanged;
            }
            InitializeGridColumns(gridView, dynamicColumns);
        }

        private static void OnColumnsCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            //TODO: how to get GridView to re-initialize columns?
        }

        private static void InitializeGridColumns(GridView gridView, IEnumerable<GridViewDynamicColumn> dynamicColumns)
        {
            foreach (GridViewDynamicColumn dynamicColumn in dynamicColumns)
            {
                GridViewColumn column = new GridViewColumn();
                dynamicColumn.Bind(column);
                gridView.Columns.Add(column);
            }
        }
    }
}
