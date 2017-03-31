using Rhiannon.Windows.Presentation;

namespace Chronos.Client.Win.Views.Sessions.SavedSessions
{
	public class ViewActivator : ViewActivatorBase<IView, View, IViewModel, ViewModel>
	{
		public ViewActivator()
			: base(ViewNames.Sessions.SavedSessions)
		{
		}
	}
}
