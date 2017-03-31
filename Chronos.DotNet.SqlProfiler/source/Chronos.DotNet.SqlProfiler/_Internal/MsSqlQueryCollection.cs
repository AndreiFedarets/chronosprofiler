using Chronos.Model;

namespace Chronos.DotNet.SqlProfiler
{
    internal sealed class MsSqlQueryCollection : UnitCollectionBase<MsSqlQueryInfo, MsSqlQueryNativeInfo>, IMsSqlQueryCollection
    {
        protected override MsSqlQueryInfo Convert(MsSqlQueryNativeInfo nativeUnit)
        {
            return new MsSqlQueryInfo(nativeUnit);
        }
    }
}
