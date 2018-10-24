using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;

namespace Adenium
{
    public static class GridViewSorting
    {
        public static readonly DependencyProperty CommandProperty;
        public static readonly DependencyProperty AutoSortProperty;
        public static readonly DependencyProperty PropertyNameProperty;

        static GridViewSorting()
        {
            CommandProperty = DependencyProperty.RegisterAttached("Command", typeof(ICommand), typeof(GridViewSorting), new UIPropertyMetadata(null, OnCommandPropertyChanged));
            AutoSortProperty = DependencyProperty.RegisterAttached("AutoSort", typeof(bool), typeof(GridViewSorting), new UIPropertyMetadata(false, OnAutoSortPropertyChanged));
            PropertyNameProperty = DependencyProperty.RegisterAttached("PropertyName", typeof(string), typeof(GridViewSorting), new UIPropertyMetadata(null));
        }

        public static ICommand GetCommand(DependencyObject obj)
        {
            return (ICommand) obj.GetValue(CommandProperty);
        }

        public static void SetCommand(DependencyObject obj, ICommand value)
        {
            obj.SetValue(CommandProperty, value);
        }

        public static bool GetAutoSort(DependencyObject obj)
        {
            return (bool) obj.GetValue(AutoSortProperty);
        }

        public static void SetAutoSort(DependencyObject obj, bool value)
        {
            obj.SetValue(AutoSortProperty, value);
        }

        public static string GetPropertyName(DependencyObject obj)
        {
            return (string) obj.GetValue(PropertyNameProperty);
        }

        public static void SetPropertyName(DependencyObject obj, string value)
        {
            obj.SetValue(PropertyNameProperty, value);
        }

        private static T GetAncestor<T>(DependencyObject reference) where T : DependencyObject
        {
            DependencyObject parent = VisualTreeHelper.GetParent(reference);
            while (!(parent is T))
            {
                parent = VisualTreeHelper.GetParent(parent);
            }
            return (T) parent;
        }

        private static void ApplySort(ICollectionView view, string propertyName)
        {
            ListSortDirection direction = ListSortDirection.Ascending;
            if (view.SortDescriptions.Count > 0)
            {
                SortDescription currentSort = view.SortDescriptions[0];
                if (currentSort.PropertyName == propertyName)
                {
                    direction = currentSort.Direction == ListSortDirection.Ascending
                        ? ListSortDirection.Descending
                        : ListSortDirection.Ascending;
                }
                view.SortDescriptions.Clear();
            }
            if (!string.IsNullOrEmpty(propertyName))
            {
                view.SortDescriptions.Add(new SortDescription(propertyName, direction));
            }
        }

        private static void OnCommandPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            ItemsControl listView = sender as ItemsControl;
            if (listView != null)
            {
                if (!GetAutoSort(listView)) // Don't change click handler if AutoSort enabled
                {
                    if (e.OldValue != null && e.NewValue == null)
                    {
                        listView.RemoveHandler(ButtonBase.ClickEvent, new RoutedEventHandler(OnColumnHeaderClick));
                    }
                    if (e.OldValue == null && e.NewValue != null)
                    {
                        listView.AddHandler(ButtonBase.ClickEvent, new RoutedEventHandler(OnColumnHeaderClick));
                    }
                }
            }
        }

        private static void OnColumnHeaderClick(object sender, RoutedEventArgs e)
        {
            GridViewColumnHeader headerClicked = e.OriginalSource as GridViewColumnHeader;
            if (headerClicked != null)
            {
                string propertyName = GetPropertyName(headerClicked.Column);
                if (!string.IsNullOrEmpty(propertyName))
                {
                    ListView listView = GetAncestor<ListView>(headerClicked);
                    if (listView != null)
                    {
                        ICommand command = GetCommand(listView);
                        if (command != null)
                        {
                            if (command.CanExecute(propertyName))
                            {
                                command.Execute(propertyName);
                            }
                        }
                        else if (GetAutoSort(listView))
                        {
                            ApplySort(listView.Items, propertyName);
                        }
                    }
                }
            }
        }

        private static void OnAutoSortPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            ListView listView = sender as ListView;
            if (listView != null)
            {
                if (GetCommand(listView) == null) // Don't change click handler if a command is set
                {
                    bool oldValue = (bool)e.OldValue;
                    bool newValue = (bool)e.NewValue;
                    if (oldValue && !newValue)
                    {
                        listView.RemoveHandler(ButtonBase.ClickEvent, new RoutedEventHandler(OnColumnHeaderClick));
                    }
                    if (!oldValue && newValue)
                    {
                        listView.AddHandler(ButtonBase.ClickEvent, new RoutedEventHandler(OnColumnHeaderClick));
                    }
                }
            }
        }

    }
}
