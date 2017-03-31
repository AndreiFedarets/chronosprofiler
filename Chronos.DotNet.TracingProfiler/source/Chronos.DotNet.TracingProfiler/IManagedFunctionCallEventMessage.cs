using Chronos.Common.EventsTree;

namespace Chronos.DotNet.TracingProfiler
{
    [PublicService(typeof(Proxy.DotNet.TracingProfiler.ManagedFunctionCallEventMessage))]
    public interface IManagedFunctionCallEventMessage : IEventMessage
    {
    }
}
