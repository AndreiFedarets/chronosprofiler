using Chronos.Model;
using Chronos.Proxy;
using Chronos.Proxy.Model;

namespace Chronos.DotNet.SqlProfiler.Proxy
{
    internal sealed class MsSqlQueryCollection : UnitCollectionProxyBase<MsSqlQueryInfo>, IMsSqlQueryCollection
    {
        public MsSqlQueryCollection(IUnitCollection<MsSqlQueryInfo> remoteObject, IApplicationSponsor sponsor)
            : base(remoteObject, sponsor)
        {
        }

        [ServiceProxyInitializationMethod]
        public void SetDependencies()
        {
        }

        protected override MsSqlQueryInfo Convert(MsSqlQueryInfo unit)
        {
            unit.SetDependencies();
            return unit;
        }
    }
}
