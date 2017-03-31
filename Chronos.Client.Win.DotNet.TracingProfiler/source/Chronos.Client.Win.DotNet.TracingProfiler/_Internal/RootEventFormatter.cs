using System.Linq;
using Chronos.Client.Win.Common.EventsTree;
using Chronos.Client.Win.DotNet.BasicProfiler;

namespace Chronos.Client.Win.DotNet.TracingProfiler
{
    internal class RootEventFormatter : IEventFormatter
    {
        private readonly IThreadCollection _threads;

        public RootEventFormatter(IThreadCollection threads)
        {
            _threads = threads;
        }

        public byte EventType
        {
            get { return 254; }
        }

        public string FormatName(IEvent @event)
        {
            ThreadInfo threadInfo = _threads.FirstOrDefault(x => x.OsThreadId == @event.ThreadUid);
            if (threadInfo == null)
            {
                return "<UNKNOWN FUNCTION>";
            }
            return string.Format("{0} #{1}", threadInfo.Name, threadInfo.Uid);
        }
    }
}
