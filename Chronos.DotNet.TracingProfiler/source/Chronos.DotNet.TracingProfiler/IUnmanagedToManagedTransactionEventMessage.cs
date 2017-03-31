using Chronos.Common.EventsTree;

namespace Chronos.DotNet.TracingProfiler
{
    [PublicService(typeof(Proxy.DotNet.TracingProfiler.UnmanagedToManagedTransactionEventMessage))]
    public interface IUnmanagedToManagedTransactionEventMessage : IEventMessage
    {
    }
}
