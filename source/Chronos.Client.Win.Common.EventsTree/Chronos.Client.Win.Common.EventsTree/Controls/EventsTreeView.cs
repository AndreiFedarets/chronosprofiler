using Chronos.Common.EventsTree;
using Layex.Extensions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;

namespace Chronos.Client.Win.Controls.Common.EventsTree
{
    [TemplatePart(Name = ItemsControlPartName, Type = typeof(ItemsControl))]
    [TemplatePart(Name = ScrollViewerPartName, Type = typeof(ScrollViewer))]
    public class EventsTreeView : Control, IEnumerable<EventTreeItem>
    {
        private const string ItemsControlPartName = "ItemsControl";
        private const string ScrollViewerPartName = "ScrollViewer";

        private static readonly DependencyProperty EventsProperty;
        private static readonly DependencyProperty EventsSortTypeProperty;
        private static readonly DependencyProperty EventMessageBuilderProperty;
        private static readonly DependencyPropertyKey HoveredItemPropertyKey;
        private static readonly DependencyPropertyKey SelectedItemPropertyKey;
        private static readonly DependencyPropertyKey HoveredEventPropertyKey;
        private static readonly DependencyPropertyKey SelectedEventPropertyKey;
        private static readonly DependencyProperty HoveredItemProperty;
        private static readonly DependencyProperty SelectedItemProperty;
        private static readonly DependencyProperty HoveredEventProperty;
        private static readonly DependencyProperty SelectedEventProperty;

        private readonly ObservableCollection<EventTreeItem> _board;
        private ItemsControl _itemsControl;
        private EventTreeItemCollection _children;

        static EventsTreeView()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(EventsTreeView), new FrameworkPropertyMetadata(typeof(EventsTreeView)));
            // Events
            EventsProperty = DependencyProperty.Register("Events", typeof(IEnumerable<IEventTree>), typeof(EventsTreeView), new PropertyMetadata(OnEventsPropertyChanged));
            // EventsSortType
            EventsSortTypeProperty = DependencyProperty.Register("EventsSortType", typeof(EventTreeSortType), typeof(EventsTreeView), new PropertyMetadata(OnEventsSortTypePropertyChanged));
            // EventMessageBuilder
            EventMessageBuilderProperty = DependencyProperty.Register("EventMessageBuilder", typeof(IEventMessageBuilder), typeof(EventsTreeView), new PropertyMetadata(OnEventFormatterPropertyChanged));
            // HoveredItem
            HoveredItemPropertyKey = DependencyProperty.RegisterReadOnly("HoveredItem", typeof(EventTreeItem), typeof(EventsTreeView), new PropertyMetadata(OnHoveredItemPropertyChanged));
            HoveredItemProperty = HoveredItemPropertyKey.DependencyProperty;
            // SelectedItem
            SelectedItemPropertyKey = DependencyProperty.RegisterReadOnly("SelectedItem", typeof(EventTreeItem), typeof(EventsTreeView), new PropertyMetadata(OnSelectedItemPropertyChanged));
            SelectedItemProperty = SelectedItemPropertyKey.DependencyProperty;
            // HoveredEvent
            HoveredEventPropertyKey = DependencyProperty.RegisterReadOnly("HoveredEvent", typeof(IEvent), typeof(EventsTreeView), new PropertyMetadata());
            HoveredEventProperty = HoveredEventPropertyKey.DependencyProperty;
            // SelectedEvent
            SelectedEventPropertyKey = DependencyProperty.RegisterReadOnly("SelectedEvent", typeof(IEvent), typeof(EventsTreeView), new PropertyMetadata());
            SelectedEventProperty = SelectedEventPropertyKey.DependencyProperty;
        }

        public EventsTreeView()
        {
            _board = new ObservableCollection<EventTreeItem>();
        }

        public EventTreeSortType EventsSortType
        {
            get { return (EventTreeSortType)GetValue(EventsSortTypeProperty); }
            set { SetValue(EventsSortTypeProperty, value); }
        }

        public EventTreeItem HoveredItem
        {
            get { return (EventTreeItem)GetValue(HoveredItemProperty); }
            internal set { SetValue(HoveredItemPropertyKey, value); }
        }

        public EventTreeItem SelectedItem
        {
            get { return (EventTreeItem)GetValue(SelectedItemProperty); }
            internal set { SetValue(SelectedItemPropertyKey, value); }
        }

        public IEvent HoveredEvent
        {
            get { return (IEvent)GetValue(HoveredEventProperty); }
            private set { SetValue(HoveredEventPropertyKey, value); }
        }

        public IEvent SelectedEvent
        {
            get { return (IEvent)GetValue(SelectedEventProperty); }
            private set { SetValue(SelectedEventPropertyKey, value); }
        }

        public IEnumerable<IEventTree> Events
        {
            get { return (IEnumerable<IEventTree>)GetValue(EventsProperty); }
            set { SetValue(EventsProperty, value); }
        }

        public IEventMessageBuilder EventMessageBuilder
        {
            get { return (IEventMessageBuilder)GetValue(EventMessageBuilderProperty); }
            set { SetValue(EventMessageBuilderProperty, value); }
        }

        public event EventHandler ChildrenUpdated;

        public ReadOnlyCollection<EventTreeItem> Children
        {
            get
            {
                InitializeChildren();
                return _children;
            }
        }

        private bool AreChildrenInitialized
        {
            get { return _children != null; }
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            //------------------
            _itemsControl = GetTemplateChild(ItemsControlPartName) as ItemsControl;
            if (_itemsControl != null)
            {
                _itemsControl.ItemsSource = _board;
            }
            InitializeChildren();
        }

        public IEnumerator<EventTreeItem> GetEnumerator()
        {
            return new EventsTreeViewEnumerator(this);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        protected void InitializeChildren()
        {
            if (AreChildrenInitialized)
            {
                return;
            }
            if (EventMessageBuilder == null || Events == null)
            {
                return;
            }
            List<EventTreeItem> children = new List<EventTreeItem>();
            _board.Clear();
            foreach (IEventTree @event in Events)
            {
                EventTreeItem item = new EventTreeItem(@event, EventMessageBuilder, null, this, 0, EventsSortType);
                children.Add(item);
            }
            _children = new EventTreeItemCollection(children);
            _children.Sort(EventsSortType);
            foreach (EventTreeItem item in _children)
            {
                _board.Add(item);
            }
            ChildrenUpdated.SafeInvoke(this, EventArgs.Empty);
        }

        internal void InsertRange(int index, IEnumerable<EventTreeItem> items)
        {
            foreach (EventTreeItem item in items)
            {
                _board.Insert(index, item);
                index++;
            }
        }

        internal void ScrollTo(EventTreeItem item)
        {
            if (_itemsControl != null)
            {
                ScrollViewer scrollViewer = _itemsControl.FindFirstVisualChild<ScrollViewer>();
                if (scrollViewer != null)
                {
                    int index = _itemsControl.Items.IndexOf(item);
                    if (index != -1)
                    {
                        scrollViewer.ScrollToVerticalOffset(index);
                    }   
                }
            }
        }

        internal void RemoveRange(int index, int count)
        {
            for (int i = 0; i < count && _board.Count > index; i++)
            {
                _board.RemoveAt(index);
            }
        }

        internal int IndexOf(EventTreeItem item)
        {
            return _board.IndexOf(item);
        }

        private void ResetChildren()
        {
            _children = null;
            InitializeChildren();
        }

        private static void OnEventFormatterPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            EventsTreeView view = (EventsTreeView)sender;
            view.InitializeChildren();
        }

        private static void OnHoveredItemPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            EventsTreeView view = (EventsTreeView)sender;
            if (view.HoveredItem != null)
            {
                view.HoveredEvent = view.HoveredItem.Event;
            }
        }

        private static void OnSelectedItemPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            EventsTreeView view = (EventsTreeView)sender;
            EventTreeItem oldSelectedItem = (EventTreeItem)e.OldValue;
            EventTreeItem newSelectedItem = (EventTreeItem)e.NewValue;
            if (oldSelectedItem != null)
            {
                oldSelectedItem.IsSelected = false;
            }
            if (newSelectedItem != null)
            {
                newSelectedItem.IsSelected = true;
            }
            if (view.SelectedItem != null)
            {
                view.SelectedEvent = view.SelectedItem.Event;
            }
        }

        private static void OnEventsPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            EventsTreeView view = (EventsTreeView)sender;
            view.ResetChildren();
        }

        private static void OnEventsSortTypePropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            EventsTreeView view = (EventsTreeView)sender;
            view.ResetChildren();
        }

        

        //private static void OnEventsPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        //{
        //    EventsTreeView view = (EventsTreeView)sender;
        //    view.InitializeChildren();
        //    IEnumerable<IEventTree> oldValue = (IEnumerable<IEventTree>)e.OldValue;
        //    IEnumerable<IEventTree> newValue = (IEnumerable<IEventTree>)e.NewValue;
        //    view.SubscribeEventsCollectionChanged(oldValue, newValue);
        //}

        //private void SubscribeEventsCollectionChanged(IEventTreeCollection oldValue, IEventTreeCollection newValue)
        //{
        //    INotifyCollectionChanged collectionChanged = oldValue as INotifyCollectionChanged;
        //    if (collectionChanged != null)
        //    {
        //        collectionChanged.CollectionChanged -= OnEventsCollectionChanged;
        //    }
        //    collectionChanged = newValue as INotifyCollectionChanged;
        //    if (collectionChanged != null)
        //    {
        //        collectionChanged.CollectionChanged += OnEventsCollectionChanged;
        //    }
        //}
    }
}
