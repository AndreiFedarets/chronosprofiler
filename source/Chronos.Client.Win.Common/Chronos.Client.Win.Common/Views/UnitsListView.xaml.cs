using Layex.Actions;
using System.Windows.Controls;

namespace Chronos.Client.Win.Common.Views
{
    public partial class UnitsListView
    {
        public UnitsListView()
        {
            InitializeComponent();
            UnitsList.SelectionChanged += OnUnitsListViewSelectionChanged;
        }

        private void OnUnitsListViewSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            dynamic viewModel = DataContext;
            ActionGroup group = viewModel.ItemContextGroup;
            if (group == null)
            {
                return;
            }
            if (e.RemovedItems != null && e.RemovedItems.Count > 0)
            {
                foreach (object item in e.RemovedItems)
                {
                    ListViewItem listViewItem = (ListViewItem)UnitsList.ItemContainerGenerator.ContainerFromItem(item);
                    if (listViewItem != null)
                    {
                        listViewItem.ContextMenu = null;
                    }
                }
            }
            if (e.AddedItems != null && e.AddedItems.Count > 0)
            {
                foreach (object item in e.AddedItems)
                {
                    ListViewItem listViewItem = (ListViewItem)UnitsList.ItemContainerGenerator.ContainerFromItem(item);
                    if (listViewItem != null)
                    {
                        listViewItem.ContextMenu = new ContextMenu { ItemsSource = group };
                    }
                }
            }
        }
    }
}
