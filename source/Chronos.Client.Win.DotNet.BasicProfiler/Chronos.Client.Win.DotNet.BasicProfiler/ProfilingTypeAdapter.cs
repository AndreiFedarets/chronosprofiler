using Adenium;
using Adenium.Layouting;
using Chronos.DotNet.BasicProfiler;

namespace Chronos.Client.Win.DotNet.BasicProfiler
{
    public class ProfilingTypeAdapter : IProfilingTypeAdapter, ILayoutProvider, IServiceConsumer
    {
        private IAppDomainCollection _appDomains;
        private IAssemblyCollection _assemblies;
        private IModuleCollection _modules;
        private IClassCollection _classes;
        private IFunctionCollection _functions;
        private IThreadCollection _threads;

        void ILayoutProvider.ConfigureContainer(IViewModel targetViewModel, IContainer container)
        {
            container.RegisterInstance(_appDomains);
            container.RegisterInstance(_assemblies);
            container.RegisterInstance(_modules);
            container.RegisterInstance(_classes);
            container.RegisterInstance(_functions);
            container.RegisterInstance(_threads);
        }

        string ILayoutProvider.GetLayout(IViewModel targetViewModel)
        {
            return LayoutFileReader.ReadViewModelLayout(targetViewModel);
        }

        void IServiceConsumer.ExportServices(IServiceContainer container)
        {
        }

        void IServiceConsumer.ImportServices(IServiceContainer container)
        {
            _appDomains = container.Resolve<IAppDomainCollection>();
            _assemblies = container.Resolve<IAssemblyCollection>();
            _modules = container.Resolve<IModuleCollection>();
            _classes = container.Resolve<IClassCollection>();
            _functions = container.Resolve<IFunctionCollection>();
            _threads = container.Resolve<IThreadCollection>();
        }
    }
}
