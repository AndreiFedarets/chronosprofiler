using System.Windows.Controls;
using Adenium.Layouting;

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
                        dynamic viewModel = ViewModel;
                        IMenu menu = viewModel.ItemContextMenu;
                        if (menu != null)
                        {
                            listViewItem.ContextMenu = new Adenium.Controls.ContextMenu { Source = menu };      
                        }
                    }
                }
            }
        }
    }
}
