using Chronos.Model;

namespace Chronos.DotNet.SqlProfiler
{
    internal sealed class SqlQueryCollection : UnitCollectionBase<SqlQueryInfo, SqlQueryNativeInfo>, ISqlQueryCollection
    {
        protected override SqlQueryInfo Convert(SqlQueryNativeInfo nativeUnit)
        {
            return new SqlQueryInfo(nativeUnit);
        }
    }
}
