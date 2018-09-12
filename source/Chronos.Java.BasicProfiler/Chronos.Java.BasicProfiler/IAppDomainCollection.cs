using Chronos.Model;

namespace Chronos.Java.BasicProfiler
{
    [PublicService(typeof(Proxy.Model.Java.BasicProfiler.AppDomainCollection))]
    public interface IAppDomainCollection : IUnitCollection<AppDomainInfo>
    {

    }
}
