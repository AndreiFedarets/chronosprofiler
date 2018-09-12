using Chronos.Client.Win.Model;

namespace Chronos.Client.Win.DotNet.BasicProfiler
{
    internal class ModuleCollection : UnitCollectionBase<ModuleInfo, Daemon.DotNet.BasicProfiler.ModuleInfo>, IModuleCollection
    {
        private readonly IAssemblyCollection _assemblies;

        public ModuleCollection(IAssemblyCollection assemblies)
        {
            _assemblies = assemblies;
        }

        protected override ModuleInfo CreateClientUnit(Daemon.DotNet.BasicProfiler.ModuleInfo daemonUnit)
        {
            return new ModuleInfo(daemonUnit, _assemblies);
        }
    }
}
