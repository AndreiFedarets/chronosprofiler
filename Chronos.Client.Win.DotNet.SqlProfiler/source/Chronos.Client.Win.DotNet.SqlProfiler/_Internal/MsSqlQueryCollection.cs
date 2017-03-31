using Chronos.Client.Win.Model;

namespace Chronos.Client.Win.DotNet.SqlProfiler
{
    internal sealed class MsSqlQueryCollection : UnitCollectionBase<MsSqlQueryInfo, Daemon.DotNet.SqlProfiler.MsSqlQueryInfo>, IMsSqlQueryCollection
    {
        protected override MsSqlQueryInfo CreateClientUnit(Daemon.DotNet.SqlProfiler.MsSqlQueryInfo daemonUnit)
        {
            return new MsSqlQueryInfo(daemonUnit);
        }
    }
}
