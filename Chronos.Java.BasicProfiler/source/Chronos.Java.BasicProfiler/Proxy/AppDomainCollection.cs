using Chronos.Java.BasicProfiler;
using Chronos.Model;

namespace Chronos.Proxy.Model.Java.BasicProfiler
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
