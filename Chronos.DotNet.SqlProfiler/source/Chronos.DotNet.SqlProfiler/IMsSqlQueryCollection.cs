using Chronos.Model;

namespace Chronos.DotNet.SqlProfiler
{
    [PublicService(typeof(Proxy.MsSqlQueryCollection))]
    public interface IMsSqlQueryCollection : IUnitCollection<MsSqlQueryInfo>
    {
    }
}
