using Rhiannon.Windows.Presentation;

namespace Chronos.Extension.ProfilingTarget.ConcreteProcess.Win
{
	public class ViewActivator : ViewActivatorBase<IView, View, IViewModel, ViewModel>
	{
		public ViewActivator()
			: base(Constants.ViewNames.Win.ConcreteProcess, true)
		{
		}
	}
}
