using System;
using System.Collections.Generic;
using System.Linq;
using Chronos.Core;
using Chronos.Extensibility;
using Rhiannon.Extensions;
using Rhiannon.Resources;
using Rhiannon.Windows.Presentation;

namespace Chronos.Extension.ProfilingStrategy.DotNetPerformance.Win
{
	public class ProfilingStrategy : IProfilingStrategy
	{
		private readonly IViewsManager _viewsManager;
		private readonly IResourceProvider _resourceProvider;
		private IViewModel _currentViewModel;

		public ProfilingStrategy(IViewsManager viewsManager, IResourceProvider resourceProvider)
		{
			_viewsManager = viewsManager;
			_resourceProvider = resourceProvider;
		}

		public ClrEventsMask EventsMask
		{
			get { return ClrEventsMask.MonitorEnterLeave | _currentViewModel.EventsMask; }
        }

	    public bool ProfileSqlQueries
	    {
            get { return _currentViewModel.ProfileSqlQueries; }
	    }

        public FilterType FilterType
        {
            get { return _currentViewModel.SelectedFilter.Type; }
        }

        public List<string> Filters
        {
            get { return _currentViewModel.SelectedFilter.Items.Select(x => x.Name).ToList(); }
        }

		public string DisplayName
		{
			get { return (string)_resourceProvider[Constants.ResourcesKeys.DotNetPerformanceKey]; }
		}

		public event Action Changed;

		public object GetView()
		{
			IViewBase view = _viewsManager.Resolve(Constants.ViewNames.Win.DotNetPerformance);
			if (_currentViewModel != null)
			{
				_currentViewModel.PropertyChanged -= OnCurrentViewModelPropertyChanged;
			}
			_currentViewModel = (IViewModel) view.ViewModel;
			if (_currentViewModel != null)
			{
				_currentViewModel.PropertyChanged += OnCurrentViewModelPropertyChanged;
			}
			return view;
		}

		private void OnCurrentViewModelPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
		{
			Changed.SafeInvoke();
		}
    }
}
