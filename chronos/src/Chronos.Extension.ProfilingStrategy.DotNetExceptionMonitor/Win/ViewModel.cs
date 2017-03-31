using Chronos.Core;
using Rhiannon.Windows.Presentation;

namespace Chronos.Extension.ProfilingStrategy.DotNetExceptionMonitor.Win
{
	public class ViewModel : ViewModelBase, IViewModel
	{
		private bool _disableInlining;
		private bool _logAppDomainsLoading;
		private bool _logAssembliesLoading;
		private bool _logModulesLoading;
		private bool _logClassesLoading;

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
	}
}
