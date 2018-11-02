using System;
using Chronos.Common;
using Chronos.Storage;

namespace Chronos.DotNet.BasicProfiler
{
    [Serializable]
    [DataTable(TableName = "DotNet_BasicProfiler_Functions")]
    public sealed class FunctionNativeInfo : NativeUnitBase
    {
        public FunctionNativeInfo()
        {
            ClassId = 0;
            AssemblyId = 0;
            ModuleId = 0;
            TypeToken = 0;
        }

        [DataTableColumn]
        public uint TypeToken { get; set; }

        [DataTableColumn]
        public ulong ClassId { get; set; }

        [DataTableColumn]
        public ulong ModuleId { get; set; }

        [DataTableColumn]
        public ulong AssemblyId { get; set; }
    }
}
