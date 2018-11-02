using Chronos.Common;
using Chronos.Common.Proxy;
using Chronos.Proxy;

namespace Chronos.DotNet.BasicProfiler.Proxy
{
    internal sealed class AssemblyCollection : UnitCollectionProxyBase<AssemblyInfo>, IAssemblyCollection
    {
        private IAppDomainCollection _appDomains;

        public AssemblyCollection(IUnitCollection<AssemblyInfo> remoteObject)
            : base(remoteObject)
        {
        }

        [ServiceProxyInitializationMethod]
        public void SetDependencies(IAppDomainCollection appDomains)
        {
            _appDomains = appDomains;
            foreach (AssemblyInfo unit in this)
            {
                unit.SetDependencies(_appDomains);
            }
        }

        protected override AssemblyInfo Convert(AssemblyInfo unit)
        {
            unit.SetDependencies(_appDomains);
            return unit;
        }
    }
}
