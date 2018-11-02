using Chronos.Common;

namespace Chronos.DotNet.SqlProfiler
{
    [PublicService(typeof(Proxy.SqlQueryCollection))]
    public interface ISqlQueryCollection : IUnitCollection<SqlQueryInfo>
    {
    }
}
