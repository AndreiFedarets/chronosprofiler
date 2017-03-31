using System;

namespace Chronos.Core
{
    [Serializable]
    public sealed class CallstackInfo : UnitBase
    {
        public CallstackInfo(uint id, uint threadId, byte[] rootEvent, uint beginLifetime, uint endLifetime)
            : base(id, id, beginLifetime, endLifetime, string.Empty)
        {
            ThreadId = threadId;
            Revision = 0;
            RootEvent = rootEvent;
        }

        public CallstackInfo()
        {
            ThreadId = 0;
            Revision = 0;
            RootEvent = null;
        }

        public uint ThreadId { get; set; }

        public byte[] RootEvent { get; set; }
    }
}
