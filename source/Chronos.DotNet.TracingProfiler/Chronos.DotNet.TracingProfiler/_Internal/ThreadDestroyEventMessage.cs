using Chronos.Common.EventsTree;
using Chronos.DotNet.BasicProfiler;

namespace Chronos.DotNet.TracingProfiler
{
    internal sealed class ThreadDestroyEventMessage : RemoteBaseObject, IThreadDestroyEventMessage 
    {
        private readonly IThreadCollection _threads;

        public ThreadDestroyEventMessage(IThreadCollection threads)
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
                return "<UNKNOWN THREAD DESTROYED>";
            }
            return string.Format("Thread {0} destroyed", threadInfo.Id);
        }
    }
}
