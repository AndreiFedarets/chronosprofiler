using Rhiannon.Unity;
using Rhiannon.Windows.Presentation;

namespace Rhiannon.Windows.Views.MessageBox
{
	public class ViewActivator : ViewActivatorBase<IView, View, IViewModel, ViewModel>
	{
		public ViewActivator()
			: base(ViewNames.BaseName.MessageBox)
		{
		}

		public override IWindow ActivateAndWrap(IContainer container, params object[] args)
		{
			IWindow window = base.ActivateAndWrap(container, args);
			window.MinHeight = 400;
			window.MinWidth = 500;
			window.Height = 600;
			window.Width = 800;
			return window;
		}
	}
}
