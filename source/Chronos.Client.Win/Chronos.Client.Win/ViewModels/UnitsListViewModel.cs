using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Data;
using Adenium;
using Adenium.Layouting;
using Chronos.Model;

namespace Chronos.Client.Win.ViewModels
{
    public abstract class UnitsListViewModel<T> : ViewModel where T : UnitBase
    {
        private GridViewDynamicColumn _selectedColumn;
        private T _selectedUnit;
        private readonly string _itemContextMenuId;

        protected UnitsListViewModel(IEnumerable<T> units, IEnumerable<GridViewDynamicColumn> columns, string itemContextMenuId)
        {
            Units = units;
            _itemContextMenuId = itemContextMenuId;
            Columns = new ObservableCollection<GridViewDynamicColumn>(columns);
            ICollectionView collectionView = CollectionViewSource.GetDefaultView(Units);
            foreach (GridViewDynamicColumn column in Columns)
            {
                column.AttachCollectionView(collectionView);
            }
            SelectedColumn = Columns.FirstOrDefault();
        }

        public IMenu ItemContextMenu
        {
            get { return Menus[_itemContextMenuId]; }
        }
        
        public ObservableCollection<GridViewDynamicColumn> Columns { get; private set; }

        public IEnumerable<T> Units { get; private set; }

        public GridViewDynamicColumn SelectedColumn
        {
            get { return _selectedColumn; }
            set
            {
                _selectedColumn = value;
                NotifyOfPropertyChange(() => SelectedColumn);
            }
        }

        public T SelectedUnit
        {
            get { return _selectedUnit; }
            set
            {
                _selectedUnit = value;
                NotifyOfPropertyChange(() => SelectedUnit);
            }
        }
    }
}
