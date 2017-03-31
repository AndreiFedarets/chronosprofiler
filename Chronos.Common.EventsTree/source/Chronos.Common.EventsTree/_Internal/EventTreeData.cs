using Chronos.Storage;
using System;

namespace Chronos.Common.EventsTree
{
    [Serializable]
    [DataTable(TableName = "EventsTree_EventsTreeData")]
    public class EventTreeData
    {
        public EventTreeData()
        {
            
        }

        internal EventTreeData(ulong eventsTreeId, byte[] data)
        {
            EventsTreeId = eventsTreeId;
            Data = data;
        }

        [DataTableColumn(PrimaryKey = true)]
        public ulong EventsTreeId { get; private set; }

        [DataTableColumn(DataType = "BLOB")]
        public byte[] Data { get; private set; }
    }
}
