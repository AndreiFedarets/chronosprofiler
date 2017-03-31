namespace Chronos.Client.Win.DotNet.SqlProfiler
{
    public sealed class MsSqlQueryInfo : Model.UnitBase
    {
        public MsSqlQueryInfo(Daemon.DotNet.SqlProfiler.MsSqlQueryInfo sqlQueryInfo)
            : base(sqlQueryInfo)
        {
        }

        private Daemon.DotNet.SqlProfiler.MsSqlQueryInfo DaemonSqlQueryInfo
        {
            get { return ((Daemon.DotNet.SqlProfiler.MsSqlQueryInfo)DaemonUnit); }
        }

    }
}
