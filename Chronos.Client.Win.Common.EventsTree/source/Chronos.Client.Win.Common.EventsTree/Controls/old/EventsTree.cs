using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using Chronos.Client.Common.EventsTree;

namespace Chronos.Client.Win.Common.EventsTree.Controls
{
    [TemplatePart(Name = ItemsControlPartName, Type = typeof(ItemsControl))]
    [TemplatePart(Name = ScrollViewerPartName, Type = typeof(ScrollViewer))]
    public class EventsTree : Control
    {
        private const string ItemsControlPartName = "ItemsControl";
        private const string ScrollViewerPartName = "ScrollViewer";
        private static readonly DependencyPropertyKey HoveredEventPropertyKey;
        public static readonly DependencyProperty ItemsProperty;
        public static readonly DependencyProperty SelectedEventProperty;
        public static readonly DependencyProperty HoveredEventProperty;
        public static readonly DependencyProperty EventFormatterProperty;

        private ItemsControl _itemsControl;
        private ScrollViewer _scrollViewer;
        private bool _navigateSelectedEvent;
        internal readonly IDictionary<IEvent, EventsTreeItem> EventsTable;

        static EventsTree()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(EventsTree), new FrameworkPropertyMetadata(typeof(EventsTree)));
            ItemsProperty = DependencyProperty.Register("Items", typeof(IEventsTreeCollection), typeof(EventsTree), new PropertyMetadata(null, OnTracesPropertyChanged));
            SelectedEventProperty = DependencyProperty.Register("SelectedEvent", typeof(IEvent), typeof(EventsTree), new PropertyMetadata(null, OnSelectedEventPropertyChanged));
            HoveredEventPropertyKey = DependencyProperty.RegisterReadOnly("HoveredEvent", typeof(IEvent), typeof(EventsTree), new PropertyMetadata(null, OnHoveredEventPropertyChanged));
            HoveredEventProperty = HoveredEventPropertyKey.DependencyProperty;
            EventFormatterProperty = DependencyProperty.Register("EventFormatter", typeof(IEventFormatter), typeof(EventsTree));
        }

        public EventsTree()
        {
            _navigateSelectedEvent = true;
            EventsTable = new Dictionary<IEvent, EventsTreeItem>();
        }

        public IEventFormatter EventFormatter
        {
            get { return (IEventFormatter)GetValue(EventFormatterProperty); }
            set { SetValue(EventFormatterProperty, value); }
        }

        public IEventsTreeCollection Items
        {
            get { return (IEventsTreeCollection)GetValue(ItemsProperty); }
            set { SetValue(ItemsProperty, value); }
        }

        public IEvent HoveredEvent
        {
            get { return (IEvent)GetValue(HoveredEventProperty); }
            internal set { SetValue(HoveredEventPropertyKey, value); }
        }

        public IEvent SelectedEvent
        {
            get { return (IEvent)GetValue(SelectedEventProperty); }
            set { SetValue(SelectedEventProperty, value); }
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            _itemsControl = GetTemplateChild(ItemsControlPartName) as ItemsControl;
            _scrollViewer = GetTemplateChild(ScrollViewerPartName) as ScrollViewer;
            InitializeTrees();
        }

        public void Navigate(IEvent targetEvent)
        {
            IList<IEvent> sequence = new List<IEvent>();
            while (targetEvent.Parent != null)
            {
                sequence.Insert(0, targetEvent);
                targetEvent = targetEvent.Parent;
            }
            EventsTreeItem item = FindItem(targetEvent);
            if (item == null)
            {
                System.Diagnostics.Debugger.Break();
                return;
            }
            item.Expand();
            for (int i = 0; i < sequence.Count; i++)
            {
                IEvent coreEvent = sequence[i];
                item = item.FindItem(coreEvent);
                if (item == null)
                {
                    break;
                }
                item.Expand();
            }
            if (item != null && _scrollViewer != null)
            {
                Point point = item.GetMiddlePosition(_scrollViewer);
                _scrollViewer.ScrollToVerticalOffset(point.Y);
            }
        }

        private EventsTreeItem FindItem(IEvent targetEvent)
        {
            foreach (EventsTreeItem item in _itemsControl.Items)
            {
                if (item.Event == targetEvent)
                {
                    return item;
                }
            }
            return null;
        }

        private void InitializeTrees()
        {
            EventsTable.Clear();
            if (_itemsControl != null && Items != null)
            {
                List<EventsTreeItem> items = new List<EventsTreeItem>();
                foreach (IEventsTree tree in Items)
                {
                    if (!tree.HasChildren)
                    {
                        continue;
                    }
                    EventsTreeItem item = new EventsTreeItem(tree, EventFormatter, this);
                    EventsTable.Add(tree, item);
                    items.Add(item);
                }
                _itemsControl.ItemsSource = items;
            }
        }

        internal void SetSelectedEventInternal(IEvent @event)
        {
            if (SelectedEvent == @event)
            {
                return;
            }
            _navigateSelectedEvent = false;
            SelectedEvent = @event;
            _navigateSelectedEvent = true;
        }

        private static void OnTracesPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            EventsTree traceTreeView = (EventsTree)sender;
            if (traceTreeView != null)
            {
                traceTreeView.InitializeTrees();
            }
        }

        private static void OnSelectedEventPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            EventsTree view = (EventsTree)sender;
            if (view == null)
            {
                return;
            }
            IEvent oldEvent = e.OldValue as IEvent;
            IEvent newEvent = e.NewValue as IEvent;
            if (oldEvent == newEvent)
            {
                return;
            }
            IDictionary<IEvent, EventsTreeItem> eventsTable = view.EventsTable;
            if (oldEvent != null)
            {
                EventsTreeItem oldItem;
                if (eventsTable.TryGetValue(oldEvent, out oldItem))
                {
                    oldItem.SetIsSelectedInternal(false);
                }
            }
            if (view._navigateSelectedEvent)
            {
                view.Navigate(newEvent);
            }
            if (newEvent != null)
            {
                EventsTreeItem newItem;
                if (eventsTable.TryGetValue(newEvent, out newItem))
                {
                    newItem.SetIsSelectedInternal(true);
                }
            }
        }

        private static void OnHoveredEventPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            EventsTree traceTreeView = (EventsTree)sender;
            if (traceTreeView == null)
            {
                return;
            }
            IDictionary<IEvent, EventsTreeItem> eventsTable = traceTreeView.EventsTable;
            IEvent oldEvent = e.OldValue as IEvent;
            IEvent newEvent = e.NewValue as IEvent;
            EventsTreeItem oldTraceTreeViewItem;
            EventsTreeItem newTraceTreeViewItem;
            if (oldEvent != null)
            {
                if (eventsTable.TryGetValue(oldEvent, out oldTraceTreeViewItem))
                {
                    oldTraceTreeViewItem.IsHovered = false;
                }
            }
            if (newEvent != null)
            {
                if (eventsTable.TryGetValue(newEvent, out newTraceTreeViewItem))
                {
                    newTraceTreeViewItem.IsHovered = true;
                }
            }
        }
    }
}
