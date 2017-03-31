using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using Chronos.Client.Common.EventsTree;

namespace Chronos.Client.Win.Common.EventsTree.Controls
{
    public class EventsTreeCollection : EventsTreeBase
    {
        public static readonly DependencyProperty SourceProperty;
        public static readonly DependencyProperty EventFormatterProperty;

        internal readonly IDictionary<IEvent, EventsTreeItem> EventsTable;

        static EventsTreeCollection()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(EventsTreeCollection), new FrameworkPropertyMetadata(typeof(EventsTreeCollection)));
            SourceProperty = DependencyProperty.Register("Source", typeof(IEventsTreeCollection), typeof(EventsTree), new PropertyMetadata(null, OnTracesPropertyChanged));
            EventFormatterProperty = DependencyProperty.Register("EventFormatter", typeof(IEventFormatter), typeof(EventsTree));
        }

        public EventsTreeCollection()
        {
            EventsTable = new Dictionary<IEvent, EventsTreeItem>();
        }

        public IEventFormatter EventFormatter
        {
            get { return (IEventFormatter)GetValue(EventFormatterProperty); }
            set { SetValue(EventFormatterProperty, value); }
        }

        public IEventsTreeCollection Source
        {
            get { return (IEventsTreeCollection)GetValue(SourceProperty); }
            set { SetValue(SourceProperty, value); }
        }

        protected override void InitializeItems()
        {
            EventsTable.Clear();
            if (_itemsControl != null && Source != null)
            {
                List<EventsTreeItem> items = new List<EventsTreeItem>();
                foreach (IEventsTree tree in Source)
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

    }
}
