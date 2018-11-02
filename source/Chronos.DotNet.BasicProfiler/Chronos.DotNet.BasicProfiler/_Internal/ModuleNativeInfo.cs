using System;
using Chronos.Common;
using Chronos.Storage;

namespace Chronos.DotNet.BasicProfiler
{
    [Serializable]
    [DataTable(TableName = "DotNet_BasicProfiler_Modules")]
    public sealed class ModuleNativeInfo : NativeUnitBase
    {
        public ModuleNativeInfo()
        {
            AssemblyId = 0;
        }

        [DataTableColumn]
        public ulong AssemblyId { get; set; }
    }
}
