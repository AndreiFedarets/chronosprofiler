using Chronos.Model;

namespace Chronos.DotNet.BasicProfiler
{
    internal sealed class AssemblyCollection : UnitCollectionBase<AssemblyInfo, AssemblyNativeInfo>, IAssemblyCollection
    {
        private IAppDomainCollection _appDomains;

        public void SetDependencies(IAppDomainCollection appDomains)
        {
            _appDomains = appDomains;
        }

        protected override AssemblyInfo Convert(AssemblyNativeInfo nativeUnit)
        {
            return new AssemblyInfo(nativeUnit, _appDomains);
        }
    }
}
