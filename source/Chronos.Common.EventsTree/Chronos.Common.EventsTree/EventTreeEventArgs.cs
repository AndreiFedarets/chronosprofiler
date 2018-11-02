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

        internal static void RaiseEvent(EventHandler<EventTreeEventArgs> handler, object sender, List<ISingleEventTree> collection)
        {
            if (handler != null)
            {
                handler(sender, new EventTreeEventArgs(collection));
            }
        }
    }
}
