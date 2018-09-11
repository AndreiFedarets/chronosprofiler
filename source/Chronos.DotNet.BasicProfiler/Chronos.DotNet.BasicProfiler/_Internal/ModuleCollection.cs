using Chronos.Model;

namespace Chronos.DotNet.BasicProfiler
{
    internal sealed class ModuleCollection : UnitCollectionBase<ModuleInfo, ModuleNativeInfo>, IModuleCollection
    {
        private IAssemblyCollection _assemblies;

        public void SetDependencies(IAssemblyCollection assemblies)
        {
            _assemblies = assemblies;
        }

        protected override ModuleInfo Convert(ModuleNativeInfo nativeUnit)
        {
            return new ModuleInfo(nativeUnit, _assemblies);
        }
    }
}
