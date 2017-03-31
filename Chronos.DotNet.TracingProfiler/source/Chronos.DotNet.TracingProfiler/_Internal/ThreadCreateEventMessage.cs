using Chronos.Common.EventsTree;
using Chronos.DotNet.BasicProfiler;

namespace Chronos.DotNet.TracingProfiler
{
    internal sealed class ThreadCreateEventMessage : RemoteBaseObject, IThreadCreateEventMessage 
    {
        public const byte EventType = 0x09;
        private readonly IThreadCollection _threads;

        public ThreadCreateEventMessage(IThreadCollection threads)
        {
            _threads = threads;
        }

        public string BuildMessage(IEvent @event)
        {
            return BuildMessageInternal(@event, _threads);
        }

        internal static string BuildMessageInternal(IEvent @event, IThreadCollection threads)
        {
            ThreadInfo threadInfo = threads[@event.Unit];
            if (threadInfo == null)
            {
                return "<UNKNOWN THREAD CREATED>";
            }
            return string.Format("Thread {0} created", threadInfo.Id);
        }
    }
}
