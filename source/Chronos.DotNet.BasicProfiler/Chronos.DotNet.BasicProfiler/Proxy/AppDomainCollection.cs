using Chronos.Common;
using Chronos.Common.Proxy;
using Chronos.Proxy;

namespace Chronos.DotNet.BasicProfiler.Proxy
{
    internal sealed class AppDomainCollection : UnitCollectionProxyBase<AppDomainInfo>, IAppDomainCollection
    {
        public AppDomainCollection(IUnitCollection<AppDomainInfo> remoteObject)
            : base(remoteObject)
        {
        }

        [ServiceProxyInitializationMethod]
        public void SetDependencies()
        {
            foreach (AppDomainInfo unit in this)
            {
                unit.SetDependencies();
            }
        }

        protected override AppDomainInfo Convert(AppDomainInfo unit)
        {
            unit.SetDependencies();
            return unit;
        }
    }
}
