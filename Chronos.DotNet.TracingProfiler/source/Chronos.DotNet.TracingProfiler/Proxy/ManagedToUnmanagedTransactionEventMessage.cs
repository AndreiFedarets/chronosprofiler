using Chronos.Common.EventsTree;
using Chronos.DotNet.TracingProfiler;

namespace Chronos.Proxy.DotNet.TracingProfiler
{
    internal sealed class ManagedToUnmanagedTransactionEventMessage : ProxyBaseObject<IManagedToUnmanagedTransactionEventMessage>, IManagedToUnmanagedTransactionEventMessage
    {
        public ManagedToUnmanagedTransactionEventMessage(IManagedToUnmanagedTransactionEventMessage remoteObject)
            : base(remoteObject)
        {
        }

        public string BuildMessage(IEvent @event)
        {
            return Chronos.DotNet.TracingProfiler.ManagedToUnmanagedTransactionEventMessage.BuildMessageInternal(@event);
        }
    }
}
