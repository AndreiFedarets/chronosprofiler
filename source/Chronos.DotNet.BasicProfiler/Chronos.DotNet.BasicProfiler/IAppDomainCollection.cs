using Chronos.Common;

namespace Chronos.DotNet.BasicProfiler
{
    [PublicService(typeof(Proxy.AppDomainCollection))]
    public interface IAppDomainCollection : IUnitCollection<AppDomainInfo>
    {

    }
}
