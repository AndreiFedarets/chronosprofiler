using System;
using Chronos.Model;
using Chronos.Storage;

namespace Chronos.DotNet.BasicProfiler
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
