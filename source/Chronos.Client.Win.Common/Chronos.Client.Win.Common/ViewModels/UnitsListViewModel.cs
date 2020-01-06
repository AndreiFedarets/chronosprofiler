using Chronos.Common;
using Layex.Actions;
using Layex.Extensions;
using Layex.ViewModels;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Data;

namespace Chronos.Client.Win.Common.ViewModels
{
    public abstract class UnitsListViewModel<T> : ViewModel where T : UnitBase
    {
        private readonly SyncronizedUnitsCollection<T> _units;
        private GridViewDynamicColumn _selectedColumn;
        private T _selectedUnit;
        private readonly string _itemContextGroupName;

        protected UnitsListViewModel(IEnumerable<T> units, IEnumerable<GridViewDynamicColumn> columns, string itemContextGroupName)
        {
            //Units = units;
            _units = new SyncronizedUnitsCollection<T>(units);
            _itemContextGroupName = itemContextGroupName;
            Columns = new ObservableCollection<GridViewDynamicColumn>(columns);
            ICollectionView collectionView = CollectionViewSource.GetDefaultView(Units);
            foreach (GridViewDynamicColumn column in Columns)
            {
                column.AttachCollectionView(collectionView);
            }
            SelectedColumn = Columns.FirstOrDefault();
        }

        public ActionGroup ItemContextGroup
        {
            get { return (ActionGroup)Actions[_itemContextGroupName]; }
        }
        
        public ObservableCollection<GridViewDynamicColumn> Columns { get; private set; }

        public IEnumerable<T> Units
        {
            get { return _units; }
        }

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

        public override void Dispose()
        {
            base.Dispose();
            _units.Dispose();
        }
    }
}
