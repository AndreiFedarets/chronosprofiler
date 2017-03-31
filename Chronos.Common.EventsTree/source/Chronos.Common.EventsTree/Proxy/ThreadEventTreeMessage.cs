using Chronos.Common.EventsTree;

namespace Chronos.Proxy.Common.EventsTree
{
    internal sealed class ThreadEventTreeMessage : ProxyBaseObject<IThreadEventTreeMessage>, IThreadEventTreeMessage
    {
        public const byte EventType = 0;

        public ThreadEventTreeMessage(IThreadEventTreeMessage remoteObject)
            : base(remoteObject)
        {
        }

        public string BuildMessage(IEvent @event)
        {
            return Chronos.Common.EventsTree.ThreadEventTreeMessage.BuildMessageInternal(@event);
        }
    }
}
