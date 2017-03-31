using Rhiannon.Windows.Presentation;

namespace Chronos.Client.Win.Views.Sessions.ActiveSessions
{
	public class ViewActivator : ViewActivatorBase<IView, View, IViewModel, ViewModel>
	{
		public ViewActivator()
			: base(ViewNames.Sessions.ActiveSessions)
		{
		}
	}
}
