using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using Rhiannon.Windows.Presentation;
using Rhiannon.Windows.Presentation.Commands;

namespace Chronos.Client.Win.Views.Options.AddAssembly
{
	public class ViewModel : ViewModelBase, IViewModel
	{
		private ProfilingFilterItemCollection _filterItems;
		private bool _areAllAssembliesSelected;

		public ICommand LoadFromGacCommand { get; private set; }
		public ICommand BrowseCommand { get; private set; }

        public ProfilingFilterItemCollection FilterItems
		{
			get { return _filterItems; }
            private set { SetPropertyAndNotifyChanged(() => FilterItems, ref _filterItems, value); }
		}

		public IEnumerable<AssemblyName> Assemblies { get; private set; }

		public bool AreAllAssembliesSelected
		{
			get { return _areAllAssembliesSelected; }
			set
			{
				_areAllAssembliesSelected = value;
                foreach (ProfilingFilterItem item in FilterItems)
				{
                    item.IsChecked = value;
				}
			}
		}

		protected override void InitializeInternal()
		{
			base.InitializeInternal();
			LoadFromGacCommand = new SyncCommand(LoadFromGac);
			BrowseCommand = new SyncCommand(Browse);
		}

		private void LoadFromGac()
		{
			GlobalAssemblyCache gac = new GlobalAssemblyCache();
			IEnumerable<ProfilingFilterItem> items = gac.Select(x => new ProfilingFilterItem(x));
            FilterItems = new ProfilingFilterItemCollection(items);
		}

		private void Browse()
		{
			System.Windows.Forms.FolderBrowserDialog dialog = new System.Windows.Forms.FolderBrowserDialog();
			if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
			{
				string path = dialog.SelectedPath;
				GlobalAssemblyCache gac = new GlobalAssemblyCache(path);
                IEnumerable<ProfilingFilterItem> items = gac.Select(x => new ProfilingFilterItem(x));
                FilterItems = new ProfilingFilterItemCollection(items);
			}
		}
	}
}
