using Chronos.Common;

namespace Chronos.DotNet.ExceptionMonitor
{
    [PublicService(typeof(Proxy.ExceptionCollection))]
    public interface IExceptionCollection : IUnitCollection<ExceptionInfo>
    {

    }
}
