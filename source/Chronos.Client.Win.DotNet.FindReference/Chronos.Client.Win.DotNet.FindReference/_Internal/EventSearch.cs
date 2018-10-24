using System.Collections.Generic;
using Chronos.Client.Win.Controls.Common.EventsTree;

namespace Chronos.Client.Win.DotNet.FindReference
{
    internal sealed class EventSearch : IEventSearch
    {
        private readonly IEnumerator<EventTreeItem> _enumerator;
        private readonly IEventSearchAdapter _adapter;
        private LinkedEventTreeItem _currentItem;
        private bool _searchFinished;

        public EventSearch(IEventSearchAdapter adapter, IEnumerator<EventTreeItem> enumerable)
        {
            _adapter = adapter;
            _enumerator = enumerable;
            _currentItem = null;
            _searchFinished = false;
        }

        public bool CanFindPrevious
        {
            get { return _currentItem != null && _currentItem.Previous != null; }
        }

        public bool CanFindNext
        {
            get
            {
                if (_searchFinished)
                {
                    return _currentItem != null && _currentItem.Next != null;
                }
                return true;
            }
        }

        public void FindNext()
        {
            if (_currentItem != null && _currentItem.Next != null)
            {
                _currentItem = _currentItem.Next;
                _currentItem.Item.Select(true);
                return;
            }
            while (_enumerator.MoveNext())
            {
                EventTreeItem item = _enumerator.Current;
                if (_adapter.Match(item))
                {
                    LinkedEventTreeItem currentItem = new LinkedEventTreeItem(item);
                    if (_currentItem != null)
                    {
                        _currentItem.Next = currentItem;
                        currentItem.Previous = _currentItem;
                    }
                    _currentItem = currentItem;
                    _currentItem.Item.Select(true);
                    break;
                }
            }
            _searchFinished = _enumerator.Current == null;
        }

        public void FindPrevious()
        {
            if (_currentItem != null && _currentItem.Previous != null)
            {
                _currentItem = _currentItem.Previous;
                _currentItem.Item.Select(true);
            }
        }

        private class LinkedEventTreeItem
        {
            public LinkedEventTreeItem(EventTreeItem item)
            {
                Item = item;
            }

            public EventTreeItem Item { get; private set; }

            public LinkedEventTreeItem Next { get; set; }

            public LinkedEventTreeItem Previous { get; set; }
        }
    }
}
