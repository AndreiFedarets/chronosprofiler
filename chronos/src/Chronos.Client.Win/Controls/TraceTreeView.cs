using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using Chronos.Core;
using Rhiannon.Extensions;

namespace Chronos.Client.Win.Controls
{
    [TemplatePart(Name = ItemsControlPartName, Type = typeof(ItemsControl))]
    [TemplatePart(Name = ScrollViewerPartName, Type = typeof(ScrollViewer))]
    public class TraceTreeView : Control
    {
        private const string ItemsControlPartName = "ItemsControl";
        private const string ScrollViewerPartName = "ScrollViewer";
        private static readonly DependencyPropertyKey HoveredEventPropertyKey;
        public static readonly DependencyProperty TracesProperty;
        public static readonly DependencyProperty SelectedEventProperty;
        public static readonly DependencyProperty HoveredEventProperty;
        public static readonly DependencyProperty EventNameFormatterProperty;

        private ItemsControl _itemsControl;
        private ScrollViewer _scrollViewer;
        private bool _navigateSelectedEvent;
        internal readonly IDictionary<IEvent, TraceTreeViewItem> EventsTable;

        static TraceTreeView()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(TraceTreeView), new FrameworkPropertyMetadata(typeof(TraceTreeView)));
            TracesProperty = DependencyProperty.Register("Traces", typeof(IThreadTraceCollection), typeof(TraceTreeView), new PropertyMetadata(null, OnTracesPropertyChanged));
            SelectedEventProperty = DependencyProperty.Register("SelectedEvent", typeof(IEvent), typeof(TraceTreeView), new PropertyMetadata(null, OnSelectedEventPropertyChanged));
            HoveredEventPropertyKey = DependencyProperty.RegisterReadOnly("HoveredEvent", typeof(IEvent), typeof(TraceTreeView), new PropertyMetadata(null, OnHoveredEventPropertyChanged));
            HoveredEventProperty = HoveredEventPropertyKey.DependencyProperty;
            EventNameFormatterProperty = DependencyProperty.Register("EventNameFormatter", typeof(IEventNameFormatter), typeof(TraceTreeView));
        }

        public TraceTreeView()
        {
            _navigateSelectedEvent = true;
            EventsTable = new Dictionary<IEvent, TraceTreeViewItem>();
        }

        public IEventNameFormatter EventNameFormatter
        {
            get { return (IEventNameFormatter)GetValue(EventNameFormatterProperty); }
            set { SetValue(EventNameFormatterProperty, value); }
        }

        public IThreadTraceCollection Traces
        {
            get { return (IThreadTraceCollection)GetValue(TracesProperty); }
            set { SetValue(TracesProperty, value); }
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
            InitializeTraces();
        }

        public void Navigate(IEvent targetEvent)
        {
            IList<IEvent> sequence = new List<IEvent>();
            while (targetEvent.Parent != null)
            {
                sequence.Insert(0, targetEvent);
                targetEvent = targetEvent.Parent;
            }
            TraceTreeViewItem item = FindItem(targetEvent);
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

        private TraceTreeViewItem FindItem(IEvent targetEvent)
        {
            foreach (TraceTreeViewItem item in _itemsControl.Items)
            {
                if (item.Event == targetEvent)
                {
                    return item;
                }
            }
            return null;
        }

        private void InitializeTraces()
        {
            EventsTable.Clear();
            if (_itemsControl != null && Traces != null)
            {
                List<TraceTreeViewItem> items = new List<TraceTreeViewItem>();
                foreach (IThreadTrace threadTrace in Traces)
                {
                    if (!threadTrace.HasChildren)
                    {
                        continue;
                    }
                    TraceTreeViewItem item = new TraceTreeViewItem(threadTrace, EventNameFormatter, this);
                    EventsTable.Add(threadTrace, item);
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
            TraceTreeView traceTreeView = (TraceTreeView)sender;
            if (traceTreeView != null)
            {
                traceTreeView.InitializeTraces();
            }
        }

        private static void OnSelectedEventPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            TraceTreeView view = (TraceTreeView)sender;
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
            IDictionary<IEvent, TraceTreeViewItem> eventsTable = view.EventsTable;
            if (oldEvent != null)
            {
                TraceTreeViewItem oldItem;
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
                TraceTreeViewItem newItem;
                if (eventsTable.TryGetValue(newEvent, out newItem))
                {
                    newItem.SetIsSelectedInternal(true);
                }
            }
        }

        private static void OnHoveredEventPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            TraceTreeView traceTreeView = (TraceTreeView)sender;
            if (traceTreeView == null)
            {
                return;
            }
            IDictionary<IEvent, TraceTreeViewItem> eventsTable = traceTreeView.EventsTable;
            IEvent oldEvent = e.OldValue as IEvent;
            IEvent newEvent = e.NewValue as IEvent;
            TraceTreeViewItem oldTraceTreeViewItem;
            TraceTreeViewItem newTraceTreeViewItem;
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
