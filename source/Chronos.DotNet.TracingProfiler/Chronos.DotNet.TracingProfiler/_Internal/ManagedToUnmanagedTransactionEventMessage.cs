using Chronos.Common.EventsTree;

namespace Chronos.DotNet.TracingProfiler
{
    internal sealed class ManagedToUnmanagedTransactionEventMessage : RemoteBaseObject, IManagedToUnmanagedTransactionEventMessage
    {
        public ManagedToUnmanagedTransactionEventMessage()
        {
        }

        public string BuildMessage(IEvent @event)
        {
            return BuildMessageInternal(@event);
        }

        internal static string BuildMessageInternal(IEvent @event)
        {
            return "Transaction to unmanaged code";
        }
    }
}
