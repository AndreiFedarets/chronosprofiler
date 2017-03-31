using System.Collections.Generic;
using Chronos.Client;
using Chronos.Core;
using Rhiannon.Windows.Presentation;

namespace Chronos.Extension.ProfilingStrategy.DotNetPerformance.Win
{
	public class ViewModel : ViewModelBase, IViewModel
    {
        private readonly IProfilingFilterProvider _filtersProvider;
        private ProfilingFilter _selectedFilter;
		private bool _disableInlining;
		private bool _logAppDomainsLoading;
		private bool _logAssembliesLoading;
		private bool _logModulesLoading;
		private bool _logClassesLoading;
	    private bool _profileSqlQueries;

        public ViewModel(IProfilingFilterProvider filtersProvider)
	    {
            _filtersProvider = filtersProvider;
	    }

        public IEnumerable<ProfilingFilter> Filters { get; private set; }

        public ProfilingFilter SelectedFilter
        {
            get { return _selectedFilter; }
            set { SetPropertyAndNotifyChanged(() => SelectedFilter, ref _selectedFilter, value); }
        }

		public bool DisableInlining
		{
			get { return _disableInlining; }
			set { SetPropertyAndNotifyChanged(() => DisableInlining, ref _disableInlining, value); }
		}

		public bool LogAppDomainsLoading
		{
			get { return _logAppDomainsLoading; }
			set { SetPropertyAndNotifyChanged(() => LogAppDomainsLoading, ref _logAppDomainsLoading, value); }
		}

		public bool LogAssembliesLoading
		{
			get { return _logAssembliesLoading; }
			set { SetPropertyAndNotifyChanged(() => LogAssembliesLoading, ref _logAssembliesLoading, value); }
		}

		public bool LogModulesLoading
		{
			get { return _logModulesLoading; }
			set { SetPropertyAndNotifyChanged(() => LogModulesLoading, ref _logModulesLoading, value); }
		}

		public bool LogClassesLoading
		{
			get { return _logClassesLoading; }
			set { SetPropertyAndNotifyChanged(() => LogClassesLoading, ref _logClassesLoading, value); }
		}

        public bool ProfileSqlQueries
        {
            get { return _logClassesLoading; }
            set { SetPropertyAndNotifyChanged(() => ProfileSqlQueries, ref _profileSqlQueries, value); }
        }

		public ClrEventsMask EventsMask
		{
			get
			{
				ClrEventsMask eventsMask = ClrEventsMask.MonitorNone;
				if (DisableInlining)
				{
					eventsMask = eventsMask | ClrEventsMask.DisableInlining;
				}
				if (LogAppDomainsLoading)
				{
					eventsMask = eventsMask | ClrEventsMask.MonitorAppDomainLoads;
				}
				if (LogAssembliesLoading)
				{
					eventsMask = eventsMask | ClrEventsMask.MonitorAssemblyLoads;
				}
				if (LogModulesLoading)
				{
					eventsMask = eventsMask | ClrEventsMask.MonitorModuleLoads;
				}
				if (LogClassesLoading)
				{
					eventsMask = eventsMask | ClrEventsMask.MonitorClassLoads;
				}
				return eventsMask;
			}
		}

        protected override void InitializeInternal()
        {
            base.InitializeInternal();
            Filters = _filtersProvider.Load();
            SelectedFilter = _filtersProvider.GetDefault();
        }
	}
}
