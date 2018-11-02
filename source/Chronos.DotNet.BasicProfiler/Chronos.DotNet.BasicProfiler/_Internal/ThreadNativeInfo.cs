using System;
using Chronos.Common;
using Chronos.Storage;

namespace Chronos.DotNet.BasicProfiler
{
    [Serializable]
    [DataTable(TableName = "DotNet_BasicProfiler_Threads")]
    public sealed class ThreadNativeInfo : NativeUnitBase
    {
        public ThreadNativeInfo()
        {
            OsThreadId = 0;
        }

        [DataTableColumn]
        public uint OsThreadId { get; set; }
    }
}
