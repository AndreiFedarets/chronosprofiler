using System;
using System.Collections;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Data;
using Adenium;
using Chronos.Client.Win.Models;
using Chronos.Model;

namespace Chronos.Client.Win.ViewModels
{
    public class UnitsListViewModel : ViewModel
    {
        private GridViewDynamicColumn _selectedColumn;
        private readonly IUnitsListModel _model;

        public UnitsListViewModel(IUnitsListModel model)
        {
            _model = model;
            ICollectionView collectionView = CollectionViewSource.GetDefaultView(Units);
            Columns = new ObservableCollection<GridViewDynamicColumn>(_model.Columns);
            foreach (GridViewDynamicColumn column in Columns)
            {
                column.AttachCollectionView(collectionView);
            }
            SelectedColumn = Columns.FirstOrDefault();
        }

        public override string DisplayName
        {
            get { return _model.DisplayName; }
            set { }
        }

        public IEnumerable Units
        {
            get { return _model.Units; }
        }

        public Type UnitType
        {
            get { return _model.UnitType; }
        }

        public ObservableCollection<GridViewDynamicColumn> Columns { get; private set; }

        public GridViewDynamicColumn SelectedColumn
        {
            get { return _selectedColumn; }
            set
            {
                _selectedColumn = value;
                NotifyOfPropertyChange(() => SelectedColumn);
            }
        }

        public UnitBase SelectedUnit
        {
            get { return _model.SelectedUnit; }
            set
            {
                _model.SelectedUnit = value;
                NotifyOfPropertyChange(() => SelectedUnit);
            }
        }
    }
}
