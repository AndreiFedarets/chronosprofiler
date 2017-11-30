using Chronos.Java.BasicProfiler;
using Chronos.Model;

namespace Chronos.Proxy.Model.Java.BasicProfiler
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
