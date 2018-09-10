using Chronos.Model;
using Chronos.Proxy;
using Chronos.Proxy.Model;

namespace Chronos.DotNet.SqlProfiler.Proxy
{
    internal sealed class SqlQueryCollection : UnitCollectionProxyBase<SqlQueryInfo>, ISqlQueryCollection
    {
        public SqlQueryCollection(IUnitCollection<SqlQueryInfo> remoteObject)
            : base(remoteObject)
        {
        }

        [ServiceProxyInitializationMethod]
        public void SetDependencies()
        {
        }

        protected override SqlQueryInfo Convert(SqlQueryInfo unit)
        {
            unit.SetDependencies();
            return unit;
        }
    }
}
