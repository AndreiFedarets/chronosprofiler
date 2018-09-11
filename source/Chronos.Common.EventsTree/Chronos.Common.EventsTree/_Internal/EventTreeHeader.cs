using System;
using Chronos.Storage;

namespace Chronos.Common.EventsTree
{
    [Serializable]
    [DataTable(TableName = "EventsTree_EventsTreeHeader")]
    public class EventTreeHeader
    {
        public EventTreeHeader()
        {
            
        }

        internal EventTreeHeader(uint threadUid, uint threadOsId, ulong eventsTreeId, uint beginLifetime, uint endLifetime, uint eventsDataSize)
        {
            ThreadUid = threadUid;
            ThreadOsId = threadOsId;
            EventsTreeId = eventsTreeId;
            BeginLifetime = beginLifetime;
            EndLifetime = endLifetime;
            EventsDataSize = eventsDataSize;
        }

        [DataTableColumn]
        public uint ThreadUid { get; private set; }

        [DataTableColumn]
        public uint ThreadOsId { get; private set; }

        [DataTableColumn(PrimaryKey = true)]
        public ulong EventsTreeId { get; private set; }

        [DataTableColumn]
        public uint BeginLifetime { get; private set; }

        [DataTableColumn]
        public uint EndLifetime { get; private set; }

        public uint EventsCount
        {
            get { return EventsDataSize / NativeEventHelper.EventSize; }
        }

        [DataTableColumn]
        public uint EventsDataSize { get; private set; }
    }
}
