using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Chronos.Client.Win
{
    public static class DependencyObjectExtensions
    {
        public static void ScrollTo(this ItemsControl itemsControl, object item)
        {
            ScrollViewer scrollViewer = null;
            DependencyObject parent = itemsControl;
            while (true)
            {
                parent = VisualTreeHelper.GetChild(parent, 0);
                if (parent == null)
                {
                    return;
                }
                scrollViewer = parent as ScrollViewer;
                if (scrollViewer != null)
                {
                    break;
                }
            }
            int index = itemsControl.Items.IndexOf(item);
            if (index != -1)
            {
                scrollViewer.ScrollToVerticalOffset(index);
            }
        }

        public static void FindVisualChildren<T>(this DependencyObject dependencyObject, List<T> results) where T : DependencyObject
        {
            int count = VisualTreeHelper.GetChildrenCount(dependencyObject);
            for (int i = 0; i < count; i++)
            {
                DependencyObject currentObject = VisualTreeHelper.GetChild(dependencyObject, i);
                T current = currentObject as T;
                if (current != null)
                {
                    results.Add(current);
                }
                FindVisualChildren<T>(currentObject, results);
            }
        }

        public static T FindFirstVisualChild<T>(this DependencyObject dependencyObject) where T : DependencyObject
        {
            int count = VisualTreeHelper.GetChildrenCount(dependencyObject);
            for (int i = 0; i < count; i++)
            {
                DependencyObject currentObject = VisualTreeHelper.GetChild(dependencyObject, i);
                T current = currentObject as T;
                if (current != null)
                {
                    return current;
                }
                current = FindFirstVisualChild<T>(currentObject);
                if (current != null)
                {
                    return current;
                }
            }
            return null;
        }
    }
}
