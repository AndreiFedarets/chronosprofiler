using Chronos.Extensibility;
using Rhiannon.Unity;
using Rhiannon.Windows.Presentation;

namespace Chronos.Client.Win.Views.Configurations.CreateConfiguration
{
	public class ViewActivator : ViewActivatorBase<IView, View, IViewModel, ViewModel>
	{
		public ViewActivator()
			: base(ViewNames.Configurations.CreateConfiguration)
		{
		}

		public override IWindow ActivateAndWrap(IContainer container, params object[] args)
		{
			IWindow window = ActivateAndWrapInternal(container, true, args);
			window.Height = 400;
			window.Width = 500;
			window.CanMaximize = false;
			window.CanMinimize = false;
			return window;
		}

		public override IViewBase Activate(IContainer container, params object[] args)
		{
			IProfilingTarget profilingTarget = (IProfilingTarget)args[0];
			IContainer childContainer = container.CreateChildContainer();
			childContainer.RegisterInstance(profilingTarget);
			IViewBase view = base.Activate(childContainer, args);
			return view;
		}
	}
}
