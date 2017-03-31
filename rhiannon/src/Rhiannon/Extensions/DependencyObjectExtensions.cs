using System.Windows;
using System.Windows.Controls;

namespace Rhiannon.Extensions
{
	public static class DependencyObjectExtensions
	{
		public static void SetGridRow(this DependencyObject dependencyObject, int row)
		{
			dependencyObject.SetValue(Grid.RowProperty, row);
		}

		public static void SetGridColumn(this DependencyObject dependencyObject, int column)
		{
			dependencyObject.SetValue(Grid.ColumnProperty, column);
		}

		public static void SetGridRowAndColumn(this DependencyObject dependencyObject, int row, int column)
		{
			dependencyObject.SetValue(Grid.RowProperty, row);
			dependencyObject.SetValue(Grid.ColumnProperty, column);
		}

		public static void SetGridColumnSpan(this DependencyObject dependencyObject, int columnSpan)
		{
			dependencyObject.SetValue(Grid.ColumnSpanProperty, columnSpan);
		}
		
	}
}
