using Chronos.Model;

namespace Chronos.DotNet.BasicProfiler
{
    internal sealed class ThreadCollection : UnitCollectionBase<ThreadInfo, ThreadNativeInfo>, IThreadCollection
    {
        protected override ThreadInfo Convert(ThreadNativeInfo nativeUnit)
        {
            return new ThreadInfo(nativeUnit);
        }
    }
}
