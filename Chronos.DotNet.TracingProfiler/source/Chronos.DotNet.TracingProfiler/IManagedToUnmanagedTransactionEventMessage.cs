using Chronos.Common.EventsTree;

namespace Chronos.DotNet.TracingProfiler
{
    [PublicService(typeof(Proxy.DotNet.TracingProfiler.ManagedToUnmanagedTransactionEventMessage))]
    public interface IManagedToUnmanagedTransactionEventMessage : IEventMessage
    {
    }
}
