using Rhiannon.Windows.Presentation;

namespace Chronos.Extension.ProfilingTarget.InternetInformationService.Win
{
	public class ViewActivator : ViewActivatorBase<IView, View, IViewModel, ViewModel>
	{
		public ViewActivator()
			: base(Constants.ViewNames.Win.InternetInformationService, true)
		{
		}
	}
}
