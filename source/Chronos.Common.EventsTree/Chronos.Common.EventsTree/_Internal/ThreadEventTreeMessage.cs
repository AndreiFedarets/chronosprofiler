namespace Chronos.Common.EventsTree
{
    internal sealed class ThreadEventTreeMessage : RemoteBaseObject, IThreadEventTreeMessage
    {
        public const byte EventType = 0;

        public string BuildMessage(IEvent @event)
        {
            return string.Format("Thread #{0}", @event.Unit);
        }

        public static string BuildMessageInternal(IEvent @event)
        {
            return string.Format("Thread #{0}", @event.Unit);
        }
    }
}
