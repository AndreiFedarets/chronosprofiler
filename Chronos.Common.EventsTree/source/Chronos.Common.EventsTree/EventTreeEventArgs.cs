using System;
using System.Collections.Generic;

namespace Chronos.Common.EventsTree
{
    [Serializable]
    public sealed class EventTreeEventArgs : EventArgs
    {
        public EventTreeEventArgs(List<ISingleEventTree> collection)
        {
            Collection = collection;
        }

        public List<ISingleEventTree> Collection { get; private set; }
    }
}
