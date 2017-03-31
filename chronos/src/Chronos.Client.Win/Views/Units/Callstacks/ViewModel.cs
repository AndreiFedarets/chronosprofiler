using Chronos.Core;

namespace Chronos.Client.Win.Views.Units.Callstacks
{
	public class ViewModel : ViewModel<CallstackInfo>, IViewModel
	{
		private readonly IProcessShadow _processShadow;

		public ViewModel(IProcessShadow processShadow, IEventNameFormatter eventNameFormatter)
			: base(processShadow.Callstacks, processShadow)
		{
			_processShadow = processShadow;
		    EventNameFormatter = eventNameFormatter;
			processShadow.Callstacks.UnitsUpdated += OnUnitsUpdated;
		}

		public override void Dispose()
		{
			base.Dispose();
            _processShadow.Callstacks.UnitsUpdated -= OnUnitsUpdated;
		}

        public IEventNameFormatter EventNameFormatter { get; private set; }
	}
}
