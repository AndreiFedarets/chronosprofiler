using System;
using System.Collections.Generic;

namespace Chronos.Common.EventsTree
{
    [Serializable]
    internal sealed class MergedEventTree : Event, IMergedEventTree
    {
        private readonly byte[] _data;

        public MergedEventTree(uint beginLifetime, uint endLifetime, byte[] data)
            : base(null, data, 0, new EventTreeBuilder())
        {
            _data = data;
            BeginLifetime = beginLifetime;
            EndLifetime = endLifetime;
        }

        public uint BeginLifetime { get; private set; }

        public uint EndLifetime { get; private set; }

        public byte[] GetBinaryData()
        {
            return _data;
        }
    }
}
