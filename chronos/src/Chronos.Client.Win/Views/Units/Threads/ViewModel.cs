using Chronos.Core;

namespace Chronos.Client.Win.Views.Units.Threads
{
	public class ViewModel : ViewModel<ThreadInfo>, IViewModel
	{
		private readonly IProcessShadow _processShadow;

		public ViewModel(IProcessShadow processShadow)
			: base(processShadow.Threads, processShadow)
		{
			_processShadow = processShadow;
			processShadow.Threads.UnitsUpdated += OnUnitsUpdated;
		}

		public override void Dispose()
		{
			base.Dispose();
			_processShadow.Threads.UnitsUpdated -= OnUnitsUpdated;
		}
	}
}
