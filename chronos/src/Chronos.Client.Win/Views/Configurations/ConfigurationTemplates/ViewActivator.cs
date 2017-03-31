using Rhiannon.Unity;
using Rhiannon.Windows.Presentation;

namespace Chronos.Client.Win.Views.Configurations.ConfigurationTemplates
{
	public class ViewActivator : ViewActivatorBase<IView, View, IViewModel, ViewModel>
	{
		public ViewActivator()
			: base(ViewNames.Configurations.ConfigurationTemplates)
		{
		}

		public override IWindow ActivateAndWrap(IContainer container, params object[] args)
		{
			IWindow window = base.ActivateAndWrap(container, args);
			window.Height = 300;
			window.Width = 400;
			return window;
		}
	}
}
