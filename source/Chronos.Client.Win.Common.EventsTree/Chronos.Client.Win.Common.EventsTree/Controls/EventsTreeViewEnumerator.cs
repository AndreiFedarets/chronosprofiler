using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Chronos.Client.Win.Controls.Common.EventsTree
{
    internal sealed class EventsTreeViewEnumerator : IEnumerator<EventTreeItem>
    {
        private readonly EventsTreeView _view;
        private EventTreeItem _currentItem;
        private bool _enumerationStarted;

        public EventsTreeViewEnumerator(EventsTreeView view)
        {
            _view = view;
            _enumerationStarted = false;
        }

        public EventTreeItem Current
        {
            get { return _currentItem; }
        }

        public void Dispose()
        {
            
        }

        object IEnumerator.Current
        {
            get { return _currentItem; }
        }

        public bool MoveNext()
        {
            if (_currentItem == null)
            {
                if (_enumerationStarted)
                {
                    return false;   
                }
                _enumerationStarted = true;
                _currentItem = _view.Children.FirstOrDefault();
                return _currentItem != null;
            }
            //if _currentItem has children - move to first child (dive into tree)
            if (_currentItem.Children.Count > 0)
            {
                _currentItem = _currentItem.Children[0];
                return true;
            }
            //_currentItem has no children so we go to item on the same level
            EventTreeItem parentItem = _currentItem.ParentItem;
            //if _currentItem has no parent, then it means we deal with Root item
            if (parentItem == null)
            {
                //get index of next item on View level
                int nextItemIndex = _view.Children.IndexOf(_currentItem) + 1;
                //if _currentItem is last item on View level...
                if (nextItemIndex >= _view.Children.Count)
                {
                    //...stop enumration - we reached end of View
                    _currentItem = null;
                    return false;
                }
                //swith to next item on View level
                _currentItem = _view.Children[nextItemIndex];
                return true;
            }
            //_current item has parent, so let's float up
            while (true)
            {
                //get index of next item on Parent level
                int nextItemIndex = parentItem.Children.IndexOf(_currentItem) + 1;
                //if _currentItem is not last item on Parent level
                if (nextItemIndex < parentItem.Children.Count)
                {
                    //swith to next item on Parent level
                    _currentItem = parentItem.Children[nextItemIndex];
                    return true;
                }
                //_currentItem is last item on Parent level, so let's go to parent of parentItem
                EventTreeItem nextParentItem = parentItem.ParentItem;
                //if parentItem has no Parent, then it means we deal with Root item
                if (nextParentItem == null)
                {
                    //get index of next item on View level
                    nextItemIndex = _view.Children.IndexOf(parentItem) + 1;
                    //if _currentItem is last item on View level...
                    if (nextItemIndex >= _view.Children.Count)
                    {
                        //...stop enumration - we reached end of View
                        _currentItem = null;
                        return false;
                    }
                    _currentItem = _view.Children[nextItemIndex];
                    return true;
                }
                //float up
                _currentItem = parentItem;
                parentItem = nextParentItem;
            }
        }

        public void Reset()
        {
            _currentItem = null;
            _enumerationStarted = false;
        }
    }
}
