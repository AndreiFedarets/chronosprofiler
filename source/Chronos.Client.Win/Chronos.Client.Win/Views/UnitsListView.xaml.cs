using System.Windows.Controls;

namespace Chronos.Client.Win.Views
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
                        listViewItem.ContextMenu = new Adenium.Controls.ContextMenu { Source = ViewModel.Menus[Constants.Menus.ItemContextMenu] };   
                    }
                }
            }
        }
    }
}
