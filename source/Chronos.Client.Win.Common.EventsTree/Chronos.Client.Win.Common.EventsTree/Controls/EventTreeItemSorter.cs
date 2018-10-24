using System.Collections.Generic;

namespace Chronos.Client.Win.Controls.Common.EventsTree
{
    internal static class EventTreeItemSorter
    {
        public static void Sort(List<EventTreeItem> source, EventTreeSortType sortType)
        {
            switch (sortType)
            {
                case EventTreeSortType.Time:
                    source.Sort(CompareByTime);
                    break;
                case EventTreeSortType.Hits:
                    source.Sort(CompareByHits);
                    break;
            }
        }

        private static int CompareByTime(EventTreeItem x, EventTreeItem y)
        {
            return y.Event.Time.CompareTo(x.Event.Time);
        }

        private static int CompareByHits(EventTreeItem x, EventTreeItem y)
        {
            return y.Event.Hits.CompareTo(x.Event.Hits);
        }
    }
}
