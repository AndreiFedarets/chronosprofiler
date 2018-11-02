using Chronos.Common;
using Chronos.Common.Proxy;
using Chronos.Proxy;

namespace Chronos.DotNet.BasicProfiler.Proxy
{
    internal sealed class ModuleCollection : UnitCollectionProxyBase<ModuleInfo>, IModuleCollection
    {
        private IAssemblyCollection _assemblies;

        public ModuleCollection(IUnitCollection<ModuleInfo> remoteObject)
            : base(remoteObject)
        {
        }

        [ServiceProxyInitializationMethod]
        public void SetDependencies(IAssemblyCollection assemblies)
        {
            _assemblies = assemblies;
            foreach (ModuleInfo unit in this)
            {
                unit.SetDependencies(_assemblies);
            }
        }

        protected override ModuleInfo Convert(ModuleInfo unit)
        {
            unit.SetDependencies(_assemblies);
            return unit;
        }
    }
}
