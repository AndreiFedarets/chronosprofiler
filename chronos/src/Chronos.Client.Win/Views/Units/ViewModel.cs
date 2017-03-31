using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Data;
using Chronos.Client.Win.UnitFiltering;
using Chronos.Core;
using Rhiannon.Windows.Presentation;

namespace Chronos.Client.Win.Views.Units
{
	public abstract class ViewModel<T> : ViewModelBase, IViewModel where T : UnitBase
	{
		protected readonly ICollectionView CollectionView;
		private readonly UnitsObservableCollection<T> _units;
		private UnitBase _selectedUnitBase;
		private IUnitFilter _selectedUnitFilter;

		protected ViewModel(IEnumerable<T> units, IProcessShadow processShadow)
		{
			_units = new UnitsObservableCollection<T>(units);
			ProcessShadow = processShadow;
			CollectionView = CollectionViewSource.GetDefaultView(Units);
		}

		protected void OnUnitsUpdated(T[] units)
		{
			View.Invoke(() => _units.Update(units));
		}

        public IProcessShadow ProcessShadow { get; private set; }

		public ProcessInfo ProcessInfo
		{
			get { return ProcessShadow.ProcessInfo; }
		}

		public IEnumerable<IUnitFilter> UnitFilters { get; private set; }

		public IUnitFilter SelectedUnitFilter
		{
			get { return _selectedUnitFilter; }
			set { SetPropertyAndNotifyChanged(() => SelectedUnitFilter, ref _selectedUnitFilter, value); }
		}

		public IEnumerable Units
		{
			get { return _units; }
		}

		public UnitBase SelectedUnit
		{
			get { return _selectedUnitBase; }
			set
			{
				SetPropertyAndNotifyChanged(() => SelectedUnit, ref _selectedUnitBase, value);
				OnSelectionChanged(new UnitEventArgs(value));
			}
		}

		public event EventHandler<UnitEventArgs> SelectionChanged;

		protected virtual void OnSelectionChanged(UnitEventArgs e)
		{
			EventHandler<UnitEventArgs> handler = SelectionChanged;
			if (handler != null)
			{
				handler(this, e);
			}
		}

		protected override void InitializeInternal()
		{
			base.InitializeInternal();
			SelectedUnit = _units.Cast<UnitBase>().FirstOrDefault();
			IList<IUnitFilter> filters = new List<IUnitFilter>();
			InitializeFilters(filters);
			UnitFilters = filters;
			SelectedUnitFilter = filters.First();
		}

		protected virtual void InitializeFilters(IList<IUnitFilter> filters)
		{
			filters.Add(new UnitNameFilter(CollectionView));
		}
	}
}
