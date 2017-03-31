using System.Windows;
using System.Windows.Threading;
using Chronos.Client.Win.Views;
using Chronos.Communication.Remoting;
using Chronos.Installation;
using Microsoft.Practices.Composite.Modularity;
using Microsoft.Practices.Composite.UnityExtensions;
using Microsoft.Practices.Unity;
using Rhiannon.Logging;
using Rhiannon.Resources;
using Rhiannon.Serialization;
using Rhiannon.Threading;
using Rhiannon.Unity;
using Rhiannon.Windows.Presentation;
using Rhiannon.Windows.Presentation.Documents;

namespace Chronos.Client.Win
{
    public class Bootstrapper : UnityBootstrapper
	{
		private IContainer _container;

		protected override DependencyObject CreateShell()
		{
			IViewsManager viewsManager = _container.Resolve<IViewsManager>();
			IWindow window = viewsManager.ResolveAndWrap(ViewNames.Main);
			//window.Closing += OnShellClosing;
			window.Open();
			return window as DependencyObject;
		}

		////TODO: Remove this hard code! We should ask this only when we(!) launched Host.
		//private void OnShellClosing(object sender, System.ComponentModel.CancelEventArgs e)
		//{
		//    if (ClientApplication.Current.Host.Sessions.Any(x => x.IsActive))
		//    {
		//        e.Cancel = MessageBox.Show("All session will be aborted. Continue?", "Chronos", MessageBoxButton.YesNo) ==
		//                   MessageBoxResult.No;
		//    }
		//}

		protected override IUnityContainer CreateContainer()
		{
			IUnityContainer unityContainer = base.CreateContainer();
			_container = new Container(unityContainer);
			unityContainer.RegisterInstance(_container);
			return unityContainer;
		}

		protected override void ConfigureContainer()
		{
			base.ConfigureContainer();
			IResourceProvider resourceProvider = new ResourceProvider(Resources.Localization.ResourceManager, Resources.Images.ResourceManager);
			_container.RegisterInstance(resourceProvider);
			_container.RegisterType<ILogger, DefaultLogger>(true);

			_container.RegisterType<IProcessShadowRenderer, ProcessShadowRenderer>(true);
			_container.RegisterType<IDocumentCollection, DocumentCollection>(true);
			_container.RegisterInstance(Dispatcher.CurrentDispatcher);
			_container.RegisterType<IThreadFactory, ThreadFactory>(true);
			_container.RegisterType<ITaskFactory, TaskFactory>(true);
			_container.RegisterType<IViewsManager, ViewsManager>(true);
			_container.RegisterType<INotificationService, SimpleNotificationService>(true);
			_container.RegisterType<ISerializerFactory, SerializerFactory>(true);
			_container.RegisterType<IRemotingExecutor, RemotingExecutor>(true);


			_container.RegisterType<IProfilerInstaller, ProfilerInstaller>(true);
			_container.RegisterType<IProgramFilesSettings, ProgramFilesSettings>(true);
			_container.RegisterType<IProfilingFilterProvider, ProfilingFilterProvider>(true);

			ConfigureViews();

			ClientApplication.Run(_container);
		}

		protected override IModuleCatalog GetModuleCatalog()
		{
			return new ModuleCatalog();
		}

		private void ConfigureViews()
		{

			IViewsManager viewsManager = _container.Resolve<IViewsManager>();
			viewsManager.Register<Rhiannon.Windows.Views.MessageBox.ViewActivator>();
			viewsManager.Register<Views.Main.ViewActivator>();

			viewsManager.Register<Views.Pages.ConfigurationsPage.ViewActivator>();
			viewsManager.Register<Views.Pages.SessionsPage.ViewActivator>();

			viewsManager.Register<Views.Groups.Home.ViewActivator>();


			viewsManager.Register<Views.Configurations.ConfigurationTemplates.ViewActivator>();
			viewsManager.Register<Views.Configurations.RecentConfigurations.ViewActivator>();
			viewsManager.Register<Views.Configurations.CreateConfiguration.ViewActivator>();

			viewsManager.Register<Views.Sessions.ActiveSessions.ViewActivator>();
			viewsManager.Register<Views.Sessions.SavedSessions.ViewActivator>();


			viewsManager.Register<Views.ProcessShadow.WinApplication.ViewActivator>();
			viewsManager.Register<Views.Units.Threads.ViewActivator>();
			viewsManager.Register<Views.Units.AppDomains.ViewActivator>();
			viewsManager.Register<Views.Units.Assemblies.ViewActivator>();
			viewsManager.Register<Views.Units.Modules.ViewActivator>();
			viewsManager.Register<Views.Units.Classes.ViewActivator>();
			viewsManager.Register<Views.Units.Functions.ViewActivator>();
            viewsManager.Register<Views.Units.Exceptions.ViewActivator>();
            viewsManager.Register<Views.Units.SqlRequests.ViewActivator>();
            viewsManager.Register<Views.Units.Callstacks.ViewActivator>();

            viewsManager.Register<Views.References.Assembly.ViewActivator>();
            viewsManager.Register<Views.References.Class.ViewActivator>();
            viewsManager.Register<Views.References.Function.ViewActivator>();

			viewsManager.Register<Views.ThreadTrace.ViewActivator>();
			viewsManager.Register<Views.PerformanceCounters.ViewActivator>();

			viewsManager.Register<Views.Options.ViewActivator>();
			viewsManager.Register<Views.Options.AddAssembly.ViewActivator>();
			viewsManager.Register<Views.Options.Shell.ViewActivator>();
			viewsManager.Register<Views.Options.Installation.ViewActivator>();
            viewsManager.Register<Views.Options.ProfilingFilter.ViewActivator>();
			viewsManager.Register<Views.Options.PerformanceCounters.ViewActivator>();

		}

		public void Shutdown()
		{
			IThreadFactory threadFactory = Container.Resolve<IThreadFactory>();
			threadFactory.CloseAll();
			ClientApplication.Shutdown();
		}
	}
}
