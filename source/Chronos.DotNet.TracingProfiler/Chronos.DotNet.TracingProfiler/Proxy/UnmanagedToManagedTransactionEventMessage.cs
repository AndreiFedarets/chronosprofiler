using Chronos.Common.EventsTree;
using Chronos.DotNet.TracingProfiler;

namespace Chronos.Proxy.DotNet.TracingProfiler
{
    internal sealed class UnmanagedToManagedTransactionEventMessage : ProxyBaseObject<IUnmanagedToManagedTransactionEventMessage>, IUnmanagedToManagedTransactionEventMessage
    {
        public UnmanagedToManagedTransactionEventMessage(IUnmanagedToManagedTransactionEventMessage remoteObject)
            : base(remoteObject)
        {
        }

        public string BuildMessage(IEvent @event)
        {
            return Chronos.DotNet.TracingProfiler.UnmanagedToManagedTransactionEventMessage.BuildMessageInternal(@event);
        }
    }
}
