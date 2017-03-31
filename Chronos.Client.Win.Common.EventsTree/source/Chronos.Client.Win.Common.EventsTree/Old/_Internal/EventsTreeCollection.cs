using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Chronos.Common.EventsTree;

namespace Chronos.Client.Win.Common.EventsTree
{
    public class EventsTreeCollection : ObservableCollection<IEventsTree>, IEventsTreeCollection
    {
        private readonly Dictionary<ulong, EventsTreeHeader> _headers;
        private Daemon.Common.EventsTree.IEventsTreeCollection _eventsTrees;

        public EventsTreeCollection()
        {
            _headers = new Dictionary<ulong, EventsTreeHeader>();
        }

        internal void Initialize(Daemon.Common.EventsTree.IEventsTreeCollection eventsTrees)
        {
            _eventsTrees = eventsTrees;
        }

        public void Refresh()
        {
            Clear();
            Dictionary<ulong, List<EventsTreeHeader>> threadsTrees = new Dictionary<ulong, List<EventsTreeHeader>>();
            lock (_headers)
            {
                _headers.Clear();
                foreach (EventsTreeHeader header in _eventsTrees)
                {
                    _headers.Add(header.EventsTreeId, header);
                    List<EventsTreeHeader> threadTrees;
                    if (!threadsTrees.TryGetValue(header.ThreadId, out threadTrees))
                    {
                        threadTrees = new List<EventsTreeHeader>();
                        threadsTrees.Add(header.ThreadId, threadTrees);
                    }
                    threadTrees.Add(header);
                }
            }
            foreach (KeyValuePair<ulong, List<EventsTreeHeader>> pair in threadsTrees)
            {
                List<EventsTreeHeader> headers = pair.Value;
                headers.Sort((x, y) => (int)(x.EventsTreeId - y.EventsTreeId));
                ulong[] treesIds = headers.Select(x => x.EventsTreeId).ToArray();
                byte[] data = _eventsTrees.GetMergedTreesData(treesIds);
                //TODO: temp solution, remove it after implementing exceptions tracker
                if (data.Length == 0)
                {
                    continue;
                }
                EventsTree eventsTree = new EventsTree((uint)pair.Key, data);
                Add(eventsTree);
            }
        }
    }
}
