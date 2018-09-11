using System;
using Chronos.Model;
using Chronos.Storage;

namespace Chronos.Java.BasicProfiler
{
    [Serializable]
    [DataTable(TableName = "BasicProfiler_Threads")]
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
