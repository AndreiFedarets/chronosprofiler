using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using Chronos.Client.Common.EventsTree;

namespace Chronos.Client.Win.Common.EventsTree.Controls
{
    [TemplatePart(Name = ItemsControlPartName, Type = typeof(ItemsControl))]
    public abstract class EventsTreeBase : Control
    {
        private static readonly DependencyPropertyKey HoveredEventPropertyKey;
        public static readonly DependencyProperty SelectedEventProperty;
        public static readonly DependencyProperty HoveredEventProperty;

        private const string ItemsControlPartName = "ItemsControl";

        private ItemsControl _itemsControl;
        private ScrollViewer _scrollViewer;
        private bool _navigateSelectedEvent;
        private IDictionary<IEvent, Even>
        
        static EventsTreeBase()
        {
            SelectedEventProperty = DependencyProperty.Register("SelectedEvent", typeof(IEvent), typeof(EventsTree), new PropertyMetadata(null, OnSelectedEventPropertyChanged));
            HoveredEventPropertyKey = DependencyProperty.RegisterReadOnly("HoveredEvent", typeof(IEvent), typeof(EventsTree), new PropertyMetadata(null, OnHoveredEventPropertyChanged));
            HoveredEventProperty = HoveredEventPropertyKey.DependencyProperty;
        }

        protected EventsTreeBase()
        {
            _navigateSelectedEvent = true;
            //EventsTable = new Dictionary<IEvent, EventsTreeItem>();
        }

        public abstract IEnumerable<IEvent> Events { get; }

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
            _scrollViewer = this.FindParent<ScrollViewer>();
            InitializeItems();
        }

        protected abstract void InitializeItems();

        public virtual void Navigate(IEvent targetEvent)
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
            EventsTreeBase traceTreeView = (EventsTreeBase)sender;
            if (traceTreeView != null)
            {
                traceTreeView.InitializeItems();
            }
        }

        private static void OnSelectedEventPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            EventsTreeBase view = (EventsTreeBase)sender;
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
            IDictionary<IEvent, EventsTreeItem> eventsTable = view._eventsTable;
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
