using Chronos.Proxy;

namespace Chronos.Common.EventsTree.Proxy
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
