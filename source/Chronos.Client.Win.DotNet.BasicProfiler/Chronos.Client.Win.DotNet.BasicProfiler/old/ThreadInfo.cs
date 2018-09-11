namespace Chronos.Client.Win.DotNet.BasicProfiler
{
    public sealed class ThreadInfo : Model.UnitBase
    {
        public ThreadInfo(Daemon.DotNet.BasicProfiler.ThreadInfo threadInfo)
            : base(threadInfo)
        {
        }

        private Daemon.DotNet.BasicProfiler.ThreadInfo DaemonThreadInfo
        {
            get { return ((Daemon.DotNet.BasicProfiler.ThreadInfo)DaemonUnit); }
        }

        public uint OsThreadId
        {
            get { return DaemonThreadInfo.OsThreadId; }
        }
    }
}
