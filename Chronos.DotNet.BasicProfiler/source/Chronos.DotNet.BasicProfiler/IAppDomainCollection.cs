using Chronos.Model;

namespace Chronos.DotNet.BasicProfiler
{
    [PublicService(typeof(Proxy.Model.DotNet.BasicProfiler.AppDomainCollection))]
    public interface IAppDomainCollection : IUnitCollection<AppDomainInfo>
    {

    }
}
