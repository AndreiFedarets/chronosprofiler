using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Data;
using System.Windows.Input;
using Chronos.Core;
using Rhiannon.Windows.Presentation;
using Rhiannon.Windows.Presentation.Commands;
using Rhiannon.Threading;
using System.IO;

namespace Chronos.Client.Win.Views.Options.ProfilingFilter
{
	public class ViewModel : ViewModelBase, IViewModel
	{
		private readonly IProfilingFilterProvider _filterProvider;
	    private readonly ITaskFactory _taskFactory;
		private readonly IViewsManager _viewsManager;
		private Client.ProfilingFilter _selectedFilter;
        private IEnumerable<Client.ProfilingFilter> _filters;
		private ProfilingFilterItemCollection _filterItems;
		private ICollectionView _view;
		private string _filterItemText;
	    private ProfilingFilterItem _selectedFilterItem;

        public ViewModel(IProfilingFilterProvider filterProvider, IViewsManager viewsManager, ITaskFactory taskFactory)
		{
            _filterProvider = filterProvider;
			_viewsManager = viewsManager;
            _taskFactory = taskFactory;
            Filters = filterProvider.Load();
            SelectedFilter = filterProvider.GetDefault();
			FilterTypes = new[] { FilterType.Include, FilterType.Exclude };
		}

        public ICommand DeleteSelectedFilterItemsCommand { get; private set; }
        public ICommand SelectAllFilterItemsCommand { get; private set; }
        public ICommand SetAsDefaultFilterCommand { get; private set; }
        public ICommand SaveFilterCommand { get; private set; }
        public ICommand RestoreFilterCommand { get; private set; }
        public ICommand DeleteFilterCommand { get; private set; }
        public ICommand CreateFilterCommand { get; private set; }
        public ICommand AddTextFilterItemCommand { get; private set; }
        public ICommand LoadFilterItemsFromGacCommand { get; private set; }
        public ICommand LoadFilterItemsFromFolderCommand { get; private set; }
        public ICommand LoadSpecificFilterItemsCommand { get; private set; }

	    public ProfilingFilterItem SelectedFilterItem
	    {
            get { return _selectedFilterItem; }
            set { SetPropertyAndNotifyChanged(() => SelectedFilterItem, ref _selectedFilterItem, value); }
	    }

		public ProfilingFilterItemCollection FilterItems
		{
			get { return _filterItems; }
			private set
			{
				_selectedFilter.Items = value.Select(x => x.AssemblyName).ToList();
				_filterItems = new ProfilingFilterItemCollection(_selectedFilter);
				_view = CollectionViewSource.GetDefaultView(_filterItems);
				NotifyPropertyChanged(() => FilterItems);
                SelectedFilterItem = FilterItems.FirstOrDefault();
			}
		}

		public IEnumerable<FilterType> FilterTypes { get; private set; }

		public IEnumerable<Client.ProfilingFilter> Filters
		{
			get { return _filters; }
			private set { SetPropertyAndNotifyChanged(() => Filters, ref _filters, value); }
		}

        public Client.ProfilingFilter SelectedFilter
		{
			get { return _selectedFilter; }
			set
			{
				_selectedFilter = value;
				_filterItems = new ProfilingFilterItemCollection(_selectedFilter);
				_view = CollectionViewSource.GetDefaultView(_filterItems);
				NotifyPropertyChanged(() => SelectedFilter);
				NotifyPropertyChanged(() => FilterItems);
                NotifyPropertyChanged(() => FilterItems);
			}
		}

		public string FilterItemText
		{
			get { return _filterItemText; }
			set { SetPropertyAndNotifyChanged(() => FilterItemText, ref _filterItemText, value); }
		}

		protected override void InitializeInternal()
		{
            base.InitializeInternal();
            SelectAllFilterItemsCommand = new SyncCommand<bool>(SelectAllFilterItems);
            DeleteSelectedFilterItemsCommand = new SyncCommand(DeleteSelectedFilterItems);
		    AddTextFilterItemCommand = new SyncCommand(AddTextFilterItem);
		    LoadFilterItemsFromGacCommand = new AsyncCommand(LoadFilterItemsFromGac, _taskFactory);
            LoadFilterItemsFromFolderCommand = new SyncCommand(LoadFilterItemsFromFolder);
            LoadSpecificFilterItemsCommand = new SyncCommand(LoadSpecificFilterItems);

            SetAsDefaultFilterCommand = new AsyncCommand<Client.ProfilingFilter>(SetAsDefaultFilter, CanSetAsDefaultFilter, _taskFactory);
            SaveFilterCommand = new AsyncCommand<Client.ProfilingFilter>(SaveFilter, _taskFactory);
            RestoreFilterCommand = new AsyncCommand<Client.ProfilingFilter>(RestoreConfiguration, CanRestoreConfiguration, _taskFactory);
            DeleteFilterCommand = new AsyncCommand<Client.ProfilingFilter>(DeleteFilter, CanDeleteFilter, _taskFactory);
            CreateFilterCommand = new SyncCommand(CreateFilter);
		}

        private void LoadFilterItemsFromFolder()
        {
            System.Windows.Forms.FolderBrowserDialog dialog = new System.Windows.Forms.FolderBrowserDialog();
            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                string path = dialog.SelectedPath;
                DirectoryInfo directoryInfo = new DirectoryInfo(path);
                FileInfo[] files = directoryInfo.GetFiles("*.dll");
                List<ProfilingFilterItem> items = files.Select(fileInfo => new ProfilingFilterItem(Path.GetFileNameWithoutExtension(fileInfo.Name))).ToList();
                AddFilterItems(items);
            }
        }

        private void LoadSpecificFilterItems()
        {
            System.Windows.Forms.OpenFileDialog dialog = new System.Windows.Forms.OpenFileDialog();
            dialog.Multiselect = true;
            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                string[] fileNames = dialog.FileNames;
                List<ProfilingFilterItem> items = fileNames.Select(x => new ProfilingFilterItem(Path.GetFileNameWithoutExtension(x))).ToList();
                AddFilterItems(items);
            }
        }

        private void LoadFilterItemsFromGac()
        {
            GlobalAssemblyCache cache = new GlobalAssemblyCache();
            FilterItems = new ProfilingFilterItemCollection(cache.Select(x => new ProfilingFilterItem(x)));
        }

        private void AddTextFilterItem()
        {
            string filterItemText = FilterItemText;
            if (string.IsNullOrEmpty(filterItemText))
            {
                return;
            }
            filterItemText = filterItemText.Trim();
            if (string.IsNullOrEmpty(filterItemText))
            {
                return;
            }
            if (FilterItems.Any(x => string.Equals(x.Name, filterItemText, StringComparison.InvariantCultureIgnoreCase)))
            {
                return;
            }
            List<ProfilingFilterItem> filterItems = new List<ProfilingFilterItem>(FilterItems);
            ProfilingFilterItem filterItem = new ProfilingFilterItem(filterItemText);
            filterItems.Add(filterItem);
            FilterItems = new ProfilingFilterItemCollection(filterItems);
            SelectedFilterItem = filterItem;
            FilterItemText = string.Empty;
        }

        private bool CanRestoreConfiguration(Client.ProfilingFilter filter)
        {
            return filter != null && filter.IsEmbedded;
        }

        private void RestoreConfiguration(Client.ProfilingFilter filter)
        {
            _filterProvider.Restore(filter);
            Reload();
        }

        private bool CanSetAsDefaultFilter(Client.ProfilingFilter filter)
		{
            return filter != null && !filter.IsDefault;
		}

        private void SetAsDefaultFilter(Client.ProfilingFilter filter)
		{
            _filterProvider.SetAsDefault(filter);
            Reload();
		}

        private bool CanDeleteFilter(Client.ProfilingFilter filter)
		{
            return filter != null && !filter.IsEmbedded;
		}

        private void DeleteFilter(Client.ProfilingFilter filter)
		{
            bool isDefault = filter.IsDefault;
            _filterProvider.Delete(filter);
            Reload();
            if (isDefault)
            {
                filter = Filters.First();
                SetAsDefaultFilter(filter);
            }
		}

        private void SaveFilter(Client.ProfilingFilter filter)
		{
            _filterProvider.Save(filter);
            Reload();
		}

        private void SelectAllFilterItems(bool isChecked)
        {
            foreach (ProfilingFilterItem item in FilterItems)
            {
                item.IsChecked = isChecked;
            }
        }

        private void DeleteSelectedFilterItems()
		{
			FilterItems = new ProfilingFilterItemCollection(FilterItems.Where(x => !x.IsChecked));
		}

        private void CreateFilter()
		{
			IWindow window = _viewsManager.ResolveAndWrap<Rhiannon.Windows.Views.EnterName.IView>();
			window.OpenDialog();
			string name = ((Rhiannon.Windows.Views.EnterName.IView)window.View).Value;
            if (string.IsNullOrEmpty(name))
			{
			    return;
			}
            name = name.Trim();
            if (string.IsNullOrEmpty(name))
			{
			    return;
			}
            Client.ProfilingFilter filter = _filterProvider.Create(name);
			Reload();
            SelectedFilter = filter;
		}

        private void AddFilterItems(IEnumerable<ProfilingFilterItem> filterItems)
        {
            List<ProfilingFilterItem> newFilterItems = new List<ProfilingFilterItem>(FilterItems);
            foreach (ProfilingFilterItem filterItem in filterItems)
            {
                if (!newFilterItems.Any(x => string.Equals(x.Name, filterItem.Name, StringComparison.InvariantCultureIgnoreCase)))
                {
                    newFilterItems.Add(filterItem);
                }

            }
            FilterItems = new ProfilingFilterItemCollection(newFilterItems);
        }

        private void Reload()
        {
            string selectedFilterName = SelectedFilter.Name;
            Filters = _filterProvider.Load();
            Client.ProfilingFilter selectedFilter = Filters.FirstOrDefault(x => string.Equals(x.Name, selectedFilterName, StringComparison.InvariantCulture));
            if (selectedFilter == null)
            {
                selectedFilter = _filterProvider.GetDefault();
            }
            SelectedFilter = selectedFilter;
        }
	}
}
