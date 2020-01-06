using Chronos.Client.Win.Controls.Common.EventsTree;
using Layex.ViewModels;
using System.Collections.Generic;
using System.Windows.Input;

namespace Chronos.Client.Win.DotNet.FindReference.ViewModels
{
    [ViewModelAttribute(Constants.ViewModels.FindReferenceViewModel)]
    public sealed class FindReferenceViewModel : ViewModel
    {
        private EventsTreeView _eventsTreeView;
        private IEventSearch _currentEventSearch;
        private string _searchText;
        private bool _searchTextEditable;
        private bool _findPreviousAvailable;
        private bool _findNextAvailable;

        public override string DisplayName
        {
            get { return "Find"; }
        }

        public ICommand FindPreviousCommand { get; private set; }

        public ICommand FindNextCommand { get; private set; }

        public ICommand StopSearchCommand { get; private set; }

        public string SearchText
        {
            get { return _searchText; }
            set
            {
                _searchText = value;
                _currentEventSearch = null;
                UpdateBindings();
                NotifyOfPropertyChange(() => SearchText);
            }
        }

        public bool SearchTextEditable
        {
            get { return _searchTextEditable; }
            set
            {
                _searchTextEditable = value;
                NotifyOfPropertyChange(() => SearchTextEditable);
            }
        }

        public bool FindPreviousAvailable
        {
            get { return _findPreviousAvailable; }
            private set
            {
                _findPreviousAvailable = value;
                NotifyOfPropertyChange(() => FindPreviousAvailable);
            }
        }

        public bool FindNextAvailable
        {
            get { return _findNextAvailable; }
            private set
            {
                _findNextAvailable = value;
                NotifyOfPropertyChange(() => FindNextAvailable);
            }
        }

        public override void Dispose()
        {
            _eventsTreeView.ChildrenUpdated -= OnViewChildrenUpdated;
            base.Dispose();
        }

        internal void BeginSearch(IEventSearchAdapter adapter)
        {
            SearchText = adapter.SearchText;
            SearchTextEditable = false;
            IEnumerator<EventTreeItem> enumerator = _eventsTreeView.GetEnumerator();
            _currentEventSearch = new EventSearch(adapter, enumerator);
            //FindNext();
        }

        private void FindPrevious()
        {
            if (_currentEventSearch == null)
            {
                return;
            }
            _currentEventSearch.FindPrevious();
            UpdateBindings();
        }

        private void FindNext()
        {
            if (_currentEventSearch == null)
            {
                EventTextSearchAdapter searchAdapter = new EventTextSearchAdapter(_searchText);
                IEnumerator<EventTreeItem> enumerator = _eventsTreeView.GetEnumerator();
                _currentEventSearch = new EventSearch(searchAdapter, enumerator);
            }
            _currentEventSearch.FindNext();
            UpdateBindings();
        }

        private void StopSearch()
        {
            _currentEventSearch = null;
            SearchText = string.Empty;
            SearchTextEditable = true;
            UpdateBindings();
        }

        private void UpdateBindings()
        {
            if (_currentEventSearch == null)
            {
                FindNextAvailable = !string.IsNullOrWhiteSpace(_searchText);
                FindPreviousAvailable = false;
            }
            else
            {
                FindNextAvailable = _currentEventSearch.CanFindNext;
                FindPreviousAvailable = _currentEventSearch.CanFindPrevious;   
            }
        }

        private void OnViewChildrenUpdated(object sender, System.EventArgs e)
        {
            _currentEventSearch = null;
            UpdateBindings();
        }

        void IAttachmentViewModel.OnAttached(IViewModel targetViewModel)
        {
            dynamic temp = targetViewModel;
            _eventsTreeView = temp.View;
            _eventsTreeView.ChildrenUpdated += OnViewChildrenUpdated;
            FindPreviousCommand = new SyncCommand(FindPrevious);
            FindNextCommand = new SyncCommand(FindNext);
            StopSearchCommand = new SyncCommand(StopSearch);
            _searchTextEditable = true;
            UpdateBindings();
        }
    }
}
