using System;
using Chronos.Model;
using Chronos.Storage;

namespace Chronos.Java.BasicProfiler
{
    [Serializable]
    [DataTable(TableName = "BasicProfiler_Modules")]
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
