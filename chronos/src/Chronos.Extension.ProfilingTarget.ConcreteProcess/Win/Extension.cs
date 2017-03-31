using Chronos.Client;
using Chronos.Extensibility;
using Rhiannon.Resources;
using Rhiannon.Windows.Presentation;

namespace Chronos.Extension.ProfilingTarget.ConcreteProcess.Win
{
	public class Extension : IExtension
	{
		private readonly IResourceProvider _resourceProvider;
		private readonly IViewsManager _viewsManager;
		private readonly IProfilingTargetCollection _targets;
		private readonly IClientApplication _application;

		public Extension(IResourceProvider resourceProvider, IViewsManager viewsManager, IClientApplication application, IProfilingTargetCollection targets)
		{
			_resourceProvider = resourceProvider;
			_viewsManager = viewsManager;
			_application = application;
			_targets = targets;
		}

		public void Initialize()
		{
			_viewsManager.Register<ViewActivator>();
			_resourceProvider.RegisterManager(Resources.Localization.ResourceManager, ResourceType.Text);
			_resourceProvider.RegisterManager(Resources.Images.ResourceManager, ResourceType.Image);
			_targets.Register(new ProfilingTarget(_viewsManager, _application, _resourceProvider));
		}
	}
}
