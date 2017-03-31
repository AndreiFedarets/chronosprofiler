using Rhiannon.Unity;
using Rhiannon.Windows.Presentation;

namespace Chronos.Client.Win.Views.Pages.SessionsPage
{
	public class ViewActivator : ViewActivatorBase<IView, View, IViewModel, ViewModel>
	{
		public ViewActivator()
			: base(ViewNames.Pages.SessionsPage)
		{
		}

		public override IWindow ActivateAndWrap(IContainer container, params object[] args)
		{
			IWindow window = base.ActivateAndWrap(container, args);
			return window;
		}
	}
}
