using System;
using System.Collections.Generic;
using System.Linq;

namespace Chronos.Common.EventsTree
{
    [Serializable]
    internal sealed class ThreadEventTree : Event, IThreadEventTree
    {
        private readonly byte[] _data;

        public ThreadEventTree(uint threadUid, uint threadOsId, uint beginLifetime, uint endLifetime, byte[] data)
            : base(null, data, 0, new EventTreeBuilder())
        {
            _data = data;
            ThreadUid = threadUid;
            ThreadOsId = threadOsId;
            BeginLifetime = beginLifetime;
            EndLifetime = endLifetime;
        }

        public uint ThreadUid { get; private set; }

        public uint ThreadOsId { get; private set; }

        public uint BeginLifetime { get; private set; }

        public uint EndLifetime { get; private set; }

        public byte[] GetBinaryData()
        {
            return _data;
        }
    }
}
