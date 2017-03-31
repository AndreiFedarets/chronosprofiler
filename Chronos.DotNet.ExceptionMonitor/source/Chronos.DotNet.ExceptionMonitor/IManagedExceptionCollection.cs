using Chronos.Model;

namespace Chronos.DotNet.ExceptionMonitor
{
    [PublicService(typeof(Proxy.ManagedExceptionCollection))]
    public interface IManagedExceptionCollection : IUnitCollection<ManagedExceptionInfo>
    {

    }
}
