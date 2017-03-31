using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Windows.Data;
using System.Windows.Input;
using Rhiannon.Threading;
using Rhiannon.Windows.Presentation;
using Rhiannon.Windows.Presentation.Commands;

namespace Chronos.Client.Win.Views.Options.PerformanceCounters
{
	public class ViewModel : ViewModelBase, IViewModel
	{
		private readonly ITaskFactory _taskFactory;
		private PerformanceCounterCategory _selectedCategory;
		private PerformanceCounter _selectedCounter;
		private PerformanceCounter _selectedTargetCounter;
		private IEnumerable<PerformanceCounter> _counters;
		private IEnumerable<PerformanceCounterCategory> _categories;
		private readonly ObservableCollection<PerformanceCounter> _targetCounters;

		public ViewModel(ITaskFactory taskFactory)
		{
			_taskFactory = taskFactory;
			_targetCounters = new ObservableCollection<PerformanceCounter>();

		}

		public ICommand LoadCategoriesCommand { get; private set; }

		public ICommand AddCounterCommand { get; private set; }

		public ICommand RemoveCounterCommand { get; private set; }

		public IEnumerable<PerformanceCounterCategory> Categories
		{
			get { return _categories; }
			private set
			{
				SetPropertyAndNotifyChanged(() => Categories, ref _categories, value);
				if (_categories != null)
				{
					ICollectionView collectionView = CollectionViewSource.GetDefaultView(_categories);
					collectionView.SortDescriptions.Add(new SortDescription("CategoryName", ListSortDirection.Ascending));
					SelectedCategory = _categories.FirstOrDefault();
				}
			}
		}

		public PerformanceCounterCategory SelectedCategory
		{
			get { return _selectedCategory; }
			set
			{
				SetPropertyAndNotifyChanged(() => SelectedCategory, ref _selectedCategory, value);
				if (_selectedCategory != null)
				{
					Counters = _selectedCategory.GetCounters(string.Empty);
				}
			}
		}

		public IEnumerable<PerformanceCounter> Counters
		{
			get { return _counters; }
			set
			{
				SetPropertyAndNotifyChanged(() => Counters, ref _counters, value);
				if (_counters != null)
				{
					ICollectionView collectionView = CollectionViewSource.GetDefaultView(_counters);
					collectionView.SortDescriptions.Add(new SortDescription("CounterName", ListSortDirection.Ascending));
				}
			}
		}

		public IEnumerable<PerformanceCounter> TargetCounters
		{
			get { return _targetCounters; }
		}

		public PerformanceCounter SelectedCounter
		{
			get { return _selectedCounter; }
			set { SetPropertyAndNotifyChanged(() => SelectedCounter, ref _selectedCounter, value); }
		}

		public PerformanceCounter SelectedTargetCounter
		{
			get { return _selectedTargetCounter; }
			set { SetPropertyAndNotifyChanged(() => SelectedTargetCounter, ref _selectedTargetCounter, value); }
		}

		protected override void InitializeInternal()
		{
			base.InitializeInternal();
			LoadCategoriesCommand = new AsyncCommand(LoadCategories, _taskFactory);
			AddCounterCommand = new SyncCommand<PerformanceCounter>(AddCounter);
			RemoveCounterCommand = new SyncCommand<PerformanceCounter>(RemoveCounter);
		}

		private void LoadCategories()
		{
			PerformanceCounterCategory[] categories = PerformanceCounterCategory.GetCategories();
			_taskFactory.ThreadFactory.Invoke(() => Categories = categories);
		}

		private void AddCounter(PerformanceCounter counter)
		{
			if (!CanAddCounter(counter))
			{
				return;
			}
			_targetCounters.Add(counter);
		}

		private bool CanAddCounter(PerformanceCounter counter)
		{
			return counter != null && !_targetCounters.Contains(counter);
		}

		private void RemoveCounter(PerformanceCounter counter)
		{
			if (!CanRemoveCounter(counter))
			{
				return;
			}
			_targetCounters.Remove(counter);
		}

		private bool CanRemoveCounter(PerformanceCounter counter)
		{
			return counter != null && _targetCounters.Contains(counter);
		}
	}
}
