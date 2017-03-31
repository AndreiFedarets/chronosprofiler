using Rhiannon.Windows.Presentation;

namespace Chronos.Extension.ProfilingStrategy.DotNetPerformance.Win
{
	public class ViewActivator : ViewActivatorBase<IView, View, IViewModel, ViewModel>
	{
		public ViewActivator()
			: base(Constants.ViewNames.Win.DotNetPerformance, true)
		{
		}
	}
}
