using Chronos.Extensibility;
using Rhiannon.Resources;
using Rhiannon.Windows.Presentation;

namespace Chronos.Extension.ProfilingStrategy.DotNetExceptionMonitor.Win
{
	public class Extension : IExtension
	{
		private readonly IResourceProvider _resourceProvider;
		private readonly IViewsManager _viewsManager;
		private readonly IProfilingStrategyCollection _strategies;

		public Extension(IResourceProvider resourceProvider, IViewsManager viewsManager, IProfilingStrategyCollection strategies)
		{
			_resourceProvider = resourceProvider;
			_viewsManager = viewsManager;
			_strategies = strategies;
		}

		public void Initialize()
		{
			_viewsManager.Register<ViewActivator>();
			_resourceProvider.RegisterManager(Properties.Resources.ResourceManager, ResourceType.Text);
			_strategies.Register(new ProfilingStrategy(_viewsManager, _resourceProvider));
		}
	}
}
