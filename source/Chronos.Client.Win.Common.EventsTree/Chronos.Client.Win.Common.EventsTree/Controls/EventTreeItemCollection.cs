using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Chronos.Client.Win.Controls.Common.EventsTree
{
    internal sealed class EventTreeItemCollection : ReadOnlyCollection<EventTreeItem>
    {
        public EventTreeItemCollection(List<EventTreeItem> collection)
            : base(collection)
        {
        }

        public EventTreeItemCollection()
            : base(new List<EventTreeItem>())
        {
            
        }

        internal void Sort(EventTreeSortType eventsSortType)
        {
            EventTreeItemSorter.Sort((List<EventTreeItem>)Items, eventsSortType);
        }
    }
}
