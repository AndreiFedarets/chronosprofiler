using System;
using Chronos.Model;
using Chronos.Storage;

namespace Chronos.DotNet.ExceptionMonitor
{
    [Serializable]
    [DataTable(TableName = "DotNet_ExceptionMonitor_Exceptions")]
    public sealed class ExceptionNativeInfo : NativeUnitBase
    {
        public ExceptionNativeInfo()
        {
            ClassId = 0;
            Message = string.Empty;
            Stack = null;
        }

        [DataTableColumn]
        public ulong ClassId { get; set; }

        [DataTableColumn]
        public string Message { get; set; }

        [DataTableColumn(DataType = "BLOB")]
        public ulong[] Stack { get; set; }
    }
}
