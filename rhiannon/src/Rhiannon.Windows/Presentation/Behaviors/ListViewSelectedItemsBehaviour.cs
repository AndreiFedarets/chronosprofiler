using System.Collections;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interactivity;

namespace Rhiannon.Windows.Presentation.Behaviors
{
	public class ListViewSelectedItemsBehaviour : Behavior<ListView>
	{
		public static readonly DependencyProperty SelectedItemsProperty;

		static ListViewSelectedItemsBehaviour()
		{
			SelectedItemsProperty = DependencyProperty.Register("SelectedItems", typeof(IEnumerable), typeof(ListViewSelectedItemsBehaviour), new PropertyMetadata(null, OnSelectedItemsPropertyChanged));
		}

		public IEnumerable SelectedItems
		{
			get { return (IEnumerable)GetValue(SelectedItemsProperty); }
			set { SetValue(SelectedItemsProperty, value); }
		}

		private static void OnSelectedItemsPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
		{
			ListViewSelectedItemsBehaviour behaviour = (ListViewSelectedItemsBehaviour)sender;
			ListView listView = behaviour.AssociatedObject;
			IEnumerable enumerable = (IEnumerable)e.NewValue;
			//listView.SelectedItem = enumerable
			//treeView.SelectItem(e.NewValue);
		}

		protected override void OnAttached()
		{
			base.OnAttached();
			AssociatedObject.SelectionChanged += OnListViewSelectionChanged;
		}

		protected override void OnDetaching()
		{
			base.OnDetaching();
			if (AssociatedObject != null)
			{
				AssociatedObject.SelectionChanged -= OnListViewSelectionChanged;
			}
		}

		private void OnListViewSelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			ListView listView = (ListView)sender;
			SelectedItems = listView.SelectedItems.Cast<object>();
		}
	}
}
