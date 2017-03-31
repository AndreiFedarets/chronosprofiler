using Rhiannon.Windows.Presentation;

namespace Chronos.Client.Win.Views.Groups.Home
{
	public class ViewActivator : ViewActivatorBase<IView, View, IViewModel, ViewModel>
	{
		public ViewActivator()
			: base(ViewNames.Groups.Home)
		{
		}
	}
}
