using Chronos.Model;

namespace Chronos.Java.BasicProfiler
{
    internal sealed class ThreadCollection : UnitCollectionBase<ThreadInfo, ThreadNativeInfo>, IThreadCollection
    {
        protected override ThreadInfo Convert(ThreadNativeInfo nativeUnit)
        {
            return new ThreadInfo(nativeUnit);
        }
    }
}
