using System;
using Chronos.Common;

namespace Chronos.DotNet.BasicProfiler
{
    [Serializable]
    public sealed class ThreadInfo : UnitBase
    {
        public ThreadInfo(ThreadNativeInfo threadInfo)
            : base(threadInfo)
        {
        }

        private ThreadNativeInfo ThreadNativeInfo
        {
            get { return (ThreadNativeInfo)NativeUnit; }
        }

        public uint OsThreadId
        {
            get { return ThreadNativeInfo.OsThreadId; }
        }

        internal void SetDependencies()
        {
        }
    }
}
