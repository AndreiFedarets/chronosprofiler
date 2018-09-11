using System;
using Chronos.Model;
using Chronos.Storage;

namespace Chronos.DotNet.BasicProfiler
{
    [Serializable]
    [DataTable(TableName = "BasicProfiler_Classes")]
    public sealed class ClassNativeInfo : NativeUnitBase
    {
        public ClassNativeInfo()
        {
            ModuleId = 0;
            TypeToken = 0;
            Namespace = string.Empty;
        }

        [DataTableColumn]
        public ulong ModuleId { get; set; }

        [DataTableColumn]
        public uint TypeToken { get; set; }

        [DataTableColumn]
        public string Namespace { get; set; }
    }
}
