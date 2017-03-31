using Rhiannon.Windows.Presentation;

namespace Chronos.Extension.ProfilingTarget.ProcessByName.Win
{
	public class ViewActivator : ViewActivatorBase<IView, View, IViewModel, ViewModel>
	{
		public ViewActivator()
			: base(Constants.ViewNames.Win.ProcessByName, true)
		{
		}
	}
}
