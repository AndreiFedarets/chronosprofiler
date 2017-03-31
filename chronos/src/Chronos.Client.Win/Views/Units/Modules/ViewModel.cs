using Chronos.Core;

namespace Chronos.Client.Win.Views.Units.Modules
{
	public class ViewModel : ViewModel<ModuleInfo>, IViewModel
	{
		private readonly IProcessShadow _processShadow;

		public ViewModel(IProcessShadow processShadow)
			: base(processShadow.Modules, processShadow)
		{
			_processShadow = processShadow;
			processShadow.Modules.UnitsUpdated += OnUnitsUpdated;
		}

		public override void Dispose()
		{
			base.Dispose();
			_processShadow.Modules.UnitsUpdated -= OnUnitsUpdated;
		}
	}
}
