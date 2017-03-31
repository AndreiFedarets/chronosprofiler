using Chronos.Core;
using Rhiannon.Windows.Presentation;
using Rhiannon.Windows.Presentation.Documents;

namespace Chronos.Client.Win.Views.Options
{
	public class ViewModel : ViewModelBase, IViewModel
	{
		private readonly IViewsManager _viewsManager;

		public ViewModel(IViewsManager viewsManager)
		{
			_viewsManager = viewsManager;
			Documents = new DocumentCollection();
		}

		public IDocumentCollection Documents { get; private set; }

		protected override void InitializeInternal()
		{
			base.InitializeInternal();

			//IViewBase shellView = _viewsManager.Resolve(ViewNames.OptionViews.Shell);
			//Documents.Add(shellView, true, false);
            if (DiagnosticModeDetector.IsDiagnosticMode())
            {
                IViewBase profilerObjectView = _viewsManager.Resolve(ViewNames.OptionViews.Installation);
                Documents.Add(profilerObjectView, true, false);
            }

			IViewBase profilingFilterView = _viewsManager.Resolve(ViewNames.OptionViews.ProfilingFilter);
            Documents.Add(profilingFilterView, true, false);

            //IViewBase performanceCountersView = _viewsManager.Resolve(ViewNames.OptionViews.PerformanceCounters);
            //Documents.Add(performanceCountersView, true, false);
		}
	}
}
