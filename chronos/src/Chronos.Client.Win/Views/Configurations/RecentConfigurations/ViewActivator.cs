using Rhiannon.Unity;
using Rhiannon.Windows.Presentation;

namespace Chronos.Client.Win.Views.Configurations.RecentConfigurations
{
	public class ViewActivator : ViewActivatorBase<IView, View, IViewModel, ViewModel>
	{
		public ViewActivator()
			: base(ViewNames.Configurations.RecentConfigurations)
		{
		}

		public override IWindow ActivateAndWrap(IContainer container, params object[] args)
		{
			IWindow window = base.ActivateAndWrap(container, args);
			//window.WindowStyle = WindowStyle.ToolWindow;
			window.Height = 300;
			window.Width = 400;
			return window;
		}
	}
}
