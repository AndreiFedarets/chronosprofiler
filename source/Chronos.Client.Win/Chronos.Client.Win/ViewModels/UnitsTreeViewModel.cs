using Adenium;
using Chronos.Client.Win.Models;

namespace Chronos.Client.Win.ViewModels
{
    public class UnitsTreeViewModel : ViewModel
    {
        private readonly IUnitsTreeModel _model;

        public UnitsTreeViewModel(IUnitsTreeModel model)
        {
            _model = model;
            //ICollectionView collectionView = CollectionViewSource.GetDefaultView(Units);
            //Columns = new ObservableCollection<GridViewDynamicColumn>(_model.Columns);
            //foreach (GridViewDynamicColumn column in Columns)
            //{
            //    column.AttachCollectionView(collectionView);
            //}
            //SelectedColumn = Columns.FirstOrDefault();
            //Menus.Add(new CompositeMenu(Constants.Menus.ItemContextMenu));
        }

        public override string DisplayName
        {
            get { return _model.DisplayName; }
            set { }
        }


    }
}
