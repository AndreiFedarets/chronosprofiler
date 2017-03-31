using Chronos.Client.Win.Model;

namespace Chronos.Client.Win.DotNet.BasicProfiler
{
    internal class ThreadCollection : UnitCollectionBase<ThreadInfo, Daemon.DotNet.BasicProfiler.ThreadInfo>, IThreadCollection
    {
        protected override ThreadInfo CreateClientUnit(Daemon.DotNet.BasicProfiler.ThreadInfo daemonUnit)
        {
            return new ThreadInfo(daemonUnit);
        }
    }
}
