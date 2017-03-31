using System.Windows.Input;
using Chronos.Core;
using Rhiannon.Windows.Presentation;
using Rhiannon.Windows.Presentation.Commands;

namespace Chronos.Client.Win.Views.PerformanceCounters
{
	public class ViewModel : ViewModelBase, IViewModel
	{
		private IPerformanceCounterCollection _performanceCounters;

		public ViewModel(IProcessShadow processShadow)
		{
			_performanceCounters = processShadow.PerformanceCounters;
			ProcessInfo = processShadow.ProcessInfo;
		}

		public ICommand RefreshCountersCommand { get; private set; }

		public ProcessInfo ProcessInfo { get; private set; }

		public IPerformanceCounterCollection PerformanceCounters
		{
			get { return _performanceCounters; }
			private set { SetPropertyAndNotifyChanged(() => PerformanceCounters, ref _performanceCounters, value); }
		}

		protected override void InitializeInternal()
		{
			base.InitializeInternal();
			RefreshCountersCommand = new SyncCommand(RefreshCounters);
		}

		public void RefreshCounters()
		{
			IPerformanceCounterCollection performanceCounters = PerformanceCounters;
			PerformanceCounters = null;
			performanceCounters.UpdateValues();
			PerformanceCounters = performanceCounters;
		}
	}
}
