using Rhiannon.Presentation;

namespace Chronos.Client.Win.Views.Groups.Advanced
{
	public class ViewActivator : ViewActivatorBase<IView, View, IViewModel, ViewModel>
	{
		public ViewActivator()
			: base(ViewNames.Groups.Advanced)
		{
		}
	}
}
