using Rhiannon.Windows.Presentation;

namespace Chronos.Extension.ProfilingStrategy.DotNetExceptionMonitor.Win
{
	public class ViewActivator : ViewActivatorBase<IView, View, IViewModel, ViewModel>
	{
		public ViewActivator()
			: base(Constants.ViewNames.Win.DotNetExceptionMonitor, true)
		{
		}
	}
}
