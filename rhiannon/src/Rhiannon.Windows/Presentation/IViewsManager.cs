using Rhiannon.Unity;

namespace Rhiannon.Windows.Presentation
{
	public interface IViewsManager
	{
		IWindow ResolveAndWrap<T>(params object[] args) where T : IViewBase;

		IWindow ResolveAndWrap(string viewName, params object[] args);

		T Resolve<T>(params object[] args) where T : IViewBase;

		IViewBase Resolve(string viewName, params object[] args);

		void Register<T>() where T : IViewActivatorBase;

		IViewsManager CreateChildViewsManager(IContainer container);
	}
}
