using Chronos.Client.Win.ViewModels;

namespace Chronos.Client.Win.DotNet.BasicProfiler
{
    internal class ProfilingTypeSession : IProfilingTypeSession
    {
        private readonly AppDomainCollection _appDomains;
        private readonly AssemblyCollection _assemblies;
        private readonly ModuleCollection _modules;
        private readonly ClassCollection _classes;
        private readonly FunctionCollection _functions;
        private readonly ThreadCollection _threads;

        public ProfilingTypeSession()
        {
            _appDomains = new AppDomainCollection();
            _assemblies = new AssemblyCollection(_appDomains);
            _modules = new ModuleCollection(_assemblies);
            _classes = new ClassCollection(_modules, _assemblies);
            _functions = new FunctionCollection(_classes);
            _threads = new ThreadCollection();
        }

        public void ExportServices(IServiceContainer container)
        {
            container.Register<IAppDomainCollection>(_appDomains);
            container.Register<IAssemblyCollection>(_assemblies);
            container.Register<IModuleCollection>(_modules);
            container.Register<IClassCollection>(_classes);
            container.Register<IFunctionCollection>(_functions);
            container.Register<IThreadCollection>(_threads);
        }

        public void ImportServices(IServiceContainer container)
        {
            Daemon.DotNet.BasicProfiler.IAppDomainCollection daemonAppDomains = container.Resolve<Daemon.DotNet.BasicProfiler.IAppDomainCollection>();
            _appDomains.Initialize(daemonAppDomains);

            Daemon.DotNet.BasicProfiler.IAssemblyCollection daemonAssemblies = container.Resolve<Daemon.DotNet.BasicProfiler.IAssemblyCollection>();
            _assemblies.Initialize(daemonAssemblies);

            Daemon.DotNet.BasicProfiler.IModuleCollection daemonModules = container.Resolve<Daemon.DotNet.BasicProfiler.IModuleCollection>();
            _modules.Initialize(daemonModules);

            Daemon.DotNet.BasicProfiler.IClassCollection daemonClasses = container.Resolve<Daemon.DotNet.BasicProfiler.IClassCollection>();
            _classes.Initialize(daemonClasses);

            Daemon.DotNet.BasicProfiler.IFunctionCollection daemonFunctions = container.Resolve<Daemon.DotNet.BasicProfiler.IFunctionCollection>();
            _functions.Initialize(daemonFunctions);

            Daemon.DotNet.BasicProfiler.IThreadCollection daemonThreads = container.Resolve<Daemon.DotNet.BasicProfiler.IThreadCollection>();
            _threads.Initialize(daemonThreads);
        }

        public void IntegrateViewModel(object profilingResultsViewModel)
        {
            ProfilingResultsViewModel viewModel = (ProfilingResultsViewModel) profilingResultsViewModel;
            IMenuItem basicInformationMenuItem = viewModel.Menu.AddMenuItem(new BasicInformationMenuItem());
            basicInformationMenuItem.AddMenuItem(new AppDomainsMenuItem(viewModel.Session));
            basicInformationMenuItem.AddMenuItem(new AssembliesMenuItem(viewModel.Session));
            basicInformationMenuItem.AddMenuItem(new ModulesMenuItem(viewModel.Session));
            basicInformationMenuItem.AddMenuItem(new ClassesMenuItem(viewModel.Session));
            basicInformationMenuItem.AddMenuItem(new FunctionsMenuItem(viewModel.Session));
            basicInformationMenuItem.AddMenuItem(new ThreadsMenuItem(viewModel.Session));
        }

        public void ReloadData()
        {
            
        }
    }
}
