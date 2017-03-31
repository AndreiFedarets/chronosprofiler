using System;
using System.Collections.Generic;
using Chronos.Core;
using Chronos.Extensibility;
using Rhiannon.Extensions;
using Rhiannon.Resources;
using Rhiannon.Windows.Presentation;

namespace Chronos.Extension.ProfilingStrategy.DotNetExceptionMonitor.Win
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
            Filters = new List<string>();
            FilterType = FilterType.Exclude;
        }

        public ClrEventsMask EventsMask
        {
            get { return ClrEventsMask.MonitorExceptions; }
        }

        public FilterType FilterType { get; private set; }

        public bool ProfileSqlQueries
        {
            get { return false; }
        }

        public List<string> Filters { get; private set; }

        public string DisplayName
        {
            get { return (string) _resourceProvider[Constants.ResourcesKeys.DotNetExceptionMonitorKey]; }
        }

        public event Action Changed;

        public object GetView()
        {
            IViewBase view = _viewsManager.Resolve(Constants.ViewNames.Win.DotNetExceptionMonitor);
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
