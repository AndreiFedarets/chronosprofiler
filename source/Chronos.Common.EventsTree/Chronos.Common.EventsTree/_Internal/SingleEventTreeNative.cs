using System;
using Chronos.Storage;

namespace Chronos.Common.EventsTree
{
    [Serializable]
    [DataTable(TableName = "Common_EventsTree_SingleEventTrees")]
    internal sealed class SingleEventTreeNative
    {
        [DataTableColumn(PrimaryKey = true)]
        public ulong Uid { get; set; }

        [DataTableColumn]
        public uint ThreadUid { get; set; }
        
        [DataTableColumn]
        public uint ThreadOsId { get; set; }

        [DataTableColumn]
        public uint BeginLifetime { get; set; }

        [DataTableColumn]
        public uint EndLifetime { get; set; }

        [DataTableColumn(DataType = "BLOB")]
        public byte[] Data { get; set; }
    }
}
