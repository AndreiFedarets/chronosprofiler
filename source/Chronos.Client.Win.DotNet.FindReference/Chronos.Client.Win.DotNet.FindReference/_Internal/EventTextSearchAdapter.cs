using System;
using Chronos.Client.Win.Controls.Common.EventsTree;

namespace Chronos.Client.Win.DotNet.FindReference
{
    internal class EventTextSearchAdapter : IEventSearchAdapter
    {
        private readonly string _searchText;

        public EventTextSearchAdapter(string searchText)
        {
            _searchText = searchText;
        }

        public bool Match(EventTreeItem item)
        {
            return item.EventText.IndexOf(_searchText, StringComparison.OrdinalIgnoreCase) >= 0;
        }

        public string SearchText
        {
            get { return _searchText; }
        }
    }
}
