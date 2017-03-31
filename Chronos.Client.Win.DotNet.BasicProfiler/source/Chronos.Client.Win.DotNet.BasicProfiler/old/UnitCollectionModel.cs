using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Data;
using Chronos.Client.Win.Presentation;
using System.Windows.Threading;
using Chronos.DotNet.BasicProfiler.Client.Win.UnitFiltering;

namespace Chronos.DotNet.BasicProfiler.Client.Win
{
    public sealed class UnitCollectionModel<T> : PropertyChangedNotifier, IDisposable where T : UnitBase
    {
        private readonly ICollectionView _collectionView;
        private readonly Dispatcher _dispatcher;
        private readonly IUnitCollection<T> _unitCollection;
        private readonly Dictionary<uint, T> _dictionary;
        private readonly ObservableCollection<T> _collection;
        private T _selectedUnit;
        private IUnitFilter _selectedUnitFilter;

        public UnitCollectionModel(IUnitCollection<T> collection)
        {
            _dispatcher = Dispatcher.CurrentDispatcher;
            _unitCollection = collection;
            _dictionary = new Dictionary<uint, T>();
            _collection = new ObservableCollection<T>(_unitCollection);
            _collectionView = CollectionViewSource.GetDefaultView(Units);
            UnitFilters = new List<IUnitFilter>();
            InitializeFilters();
            _unitCollection.UnitsUpdated += OnUnitsCollectionUpdated;
        }

        private void OnUnitsCollectionUpdated(object sender, UnitCollectionEventArgs<T> e)
        {
            Action action = () => Update(e.Units);
            if (_dispatcher.CheckAccess())
            {
                action();
            }
            else
            {
                _dispatcher.Invoke(action);
            }
        }

        private void Update(IEnumerable<T> units)
        {
            foreach (T unit in units)
            {
                T original;
                if (_dictionary.TryGetValue(unit.Uid, out original))
                {
                    _collection.Remove(original);
                    _dictionary.Remove(unit.Uid);
                }
                _collection.Add(unit);
                _dictionary.Add(unit.Uid, unit);
            }
        }

        public IEnumerable<T> Units
        {
            get { return _collection; }
        }

        public T SelectedUnit
        {
            get { return _selectedUnit; }
            set { SetPropertyAndNotifyChanged(() => SelectedUnit, out _selectedUnit, value); }
        }

        public List<IUnitFilter> UnitFilters { get; private set; }

        public IUnitFilter SelectedUnitFilter
        {
            get { return _selectedUnitFilter; }
            set
            {
                if (value != null && UnitFilters.Contains(value))
                {
                    SetPropertyAndNotifyChanged(() => SelectedUnitFilter, out _selectedUnitFilter, value);
                }
            }
        }

        public void Dispose()
        {
            _unitCollection.UnitsUpdated -= OnUnitsCollectionUpdated;
        }

        private void InitializeFilters()
        {
            UnitFilters.Add(new UnitNameFilter(_collectionView));
            SelectedUnitFilter = UnitFilters.FirstOrDefault();
        }
    }
}
