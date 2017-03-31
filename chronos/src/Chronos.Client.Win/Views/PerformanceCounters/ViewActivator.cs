using Rhiannon.Windows.Presentation;

namespace Chronos.Client.Win.Views.PerformanceCounters
{
	public class ViewActivator : ViewActivatorBase<IView, View, IViewModel, ViewModel>
	{
		public ViewActivator()
			: base(ViewNames.BaseViews.PerformanceCounters)
		{
		}
	}
}
