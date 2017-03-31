using Chronos.Core;

namespace Chronos.Client.Win.Views.Units.AppDomains
{
	public class ViewModel : ViewModel<AppDomainInfo>, IViewModel
	{
		private readonly IProcessShadow _processShadow;

		public ViewModel(IProcessShadow processShadow)
			: base(processShadow.AppDomains, processShadow)
		{
			_processShadow = processShadow;
			processShadow.AppDomains.UnitsUpdated += OnUnitsUpdated;
		}

		public override void Dispose()
		{
			base.Dispose();
			_processShadow.AppDomains.UnitsUpdated -= OnUnitsUpdated;
		}
	}
}
