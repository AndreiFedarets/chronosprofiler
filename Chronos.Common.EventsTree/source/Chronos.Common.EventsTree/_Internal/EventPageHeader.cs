using System.Runtime.InteropServices;

namespace Chronos.Common.EventsTree
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    internal struct EventPageHeader
    {
        public byte PageType;
        public EventPageState Flag;
        public uint ThreadUid;
        public uint ThreadOsId;
        public ulong EventsTreeGlobalId;
        public ushort EventsTreeLocalId;
        public uint BeginLifetime;
        public uint EndLifetime;
        public uint PageIndex;
        public uint EventsDataSize;
    }
}
