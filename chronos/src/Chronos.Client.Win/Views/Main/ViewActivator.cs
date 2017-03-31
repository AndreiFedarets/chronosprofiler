using Rhiannon.Unity;
using Rhiannon.Windows.Presentation;

namespace Chronos.Client.Win.Views.Main
{
	public class ViewActivator : ViewActivatorBase<IView, View, IViewModel, ViewModel>
	{
		public ViewActivator()
			: base(ViewNames.Main)
		{
		}

		public override IWindow ActivateAndWrap(IContainer container, params object[] args)
		{
			IWindow window = base.ActivateAndWrap(container, args);
			window.MinHeight = 400;
			window.MinWidth = 500;
			window.Height = 600;
			window.Width = 900;
			return window;
		}
	}
}
