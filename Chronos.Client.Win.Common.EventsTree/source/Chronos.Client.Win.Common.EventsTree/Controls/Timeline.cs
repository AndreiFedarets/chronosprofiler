using Chronos.Common.EventsTree;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Chronos.Client.Win.Controls.Common.EventsTree
{
    [TemplatePart(Name = ItemsControlPartName, Type = typeof(ItemsControl))]
    public class Timeline : Control
    {
        private const string ItemsControlPartName = "ItemsControl";

        private static readonly DependencyProperty EventsProperty;
        private static readonly DependencyProperty ProfilingTimerProperty;
        private static readonly DependencyPropertyKey HoveredItemPropertyKey;
        private static readonly DependencyPropertyKey SelectedItemPropertyKey;
        private static readonly DependencyPropertyKey HoveredEventTreePropertyKey;
        private static readonly DependencyPropertyKey SelectedEventTreePropertyKey;
        private static readonly DependencyProperty HoveredItemProperty;
        private static readonly DependencyProperty SelectedItemProperty;
        private static readonly DependencyProperty HoveredEventTreeProperty;
        private static readonly DependencyProperty SelectedEventTreeProperty;
        //private static readonly DependencyProperty ThreadsProperty;

        private readonly ObservableCollection<ThreadTimeline> _collection;
        private ItemsControl _itemsControl;

        static Timeline()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(Timeline), new FrameworkPropertyMetadata(typeof(Timeline)));
            // Events
            EventsProperty = DependencyProperty.Register("Events", typeof(IEventTreeCollection), typeof(Timeline), new PropertyMetadata(OnEventsPropertyChanged));
            ProfilingTimerProperty = DependencyProperty.Register("ProfilingTimer", typeof(IProfilingTimer), typeof(Timeline), new PropertyMetadata(OnProfilingTimerPropertyChanged));
            // HoveredItem
            HoveredItemPropertyKey = DependencyProperty.RegisterReadOnly("HoveredItem", typeof(ThreadTimelineItem), typeof(Timeline), new PropertyMetadata(OnHoveredItemPropertyChanged));
            HoveredItemProperty = HoveredItemPropertyKey.DependencyProperty;
            // SelectedItem
            SelectedItemPropertyKey = DependencyProperty.RegisterReadOnly("SelectedItem", typeof(ThreadTimelineItem), typeof(Timeline), new PropertyMetadata(OnSelectedItemPropertyChanged));
            SelectedItemProperty = SelectedItemPropertyKey.DependencyProperty;
            // HoveredEvent
            HoveredEventTreePropertyKey = DependencyProperty.RegisterReadOnly("HoveredEventTree", typeof(ISingleEventTree), typeof(Timeline), new PropertyMetadata());
            HoveredEventTreeProperty = HoveredEventTreePropertyKey.DependencyProperty;
            // SelectedEvent
            SelectedEventTreePropertyKey = DependencyProperty.RegisterReadOnly("SelectedEventTree", typeof(ISingleEventTree), typeof(Timeline), new PropertyMetadata());
            SelectedEventTreeProperty = SelectedEventTreePropertyKey.DependencyProperty;
        }

        public Timeline()
        {
            _collection = new ObservableCollection<ThreadTimeline>();
        }

        public IEventTreeCollection Events
        {
            get { return (IEventTreeCollection)GetValue(EventsProperty); }
            set { SetValue(EventsProperty, value); }
        }

        public IProfilingTimer ProfilingTimer
        {
            get { return (IProfilingTimer)GetValue(ProfilingTimerProperty); }
            set { SetValue(ProfilingTimerProperty, value); }
        }

        public ThreadTimelineItem HoveredItem
        {
            get { return (ThreadTimelineItem)GetValue(HoveredItemProperty); }
            internal set { SetValue(HoveredItemPropertyKey, value); }
        }

        public ThreadTimelineItem SelectedItem
        {
            get { return (ThreadTimelineItem)GetValue(SelectedItemProperty); }
            internal set { SetValue(SelectedItemPropertyKey, value); }
        }

        public ISingleEventTree HoveredEventTree
        {
            get { return (ISingleEventTree)GetValue(HoveredEventTreeProperty); }
            private set { SetValue(HoveredEventTreePropertyKey, value); }
        }

        public ISingleEventTree SelectedEventTree
        {
            get { return (ISingleEventTree)GetValue(SelectedEventTreeProperty); }
            private set { SetValue(SelectedEventTreePropertyKey, value); }
        }


        private bool AreChildrenInitialized { get; set; }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            //------------------
            _itemsControl = GetTemplateChild(ItemsControlPartName) as ItemsControl;
            if (_itemsControl != null)
            {
                _itemsControl.ItemsSource = _collection;
            }
            InitializeChildren();
        }

        private void InitializeChildren()
        {
            _collection.Clear();
            if (Events == null || ProfilingTimer == null)
            {
                return;
            }
            IEnumerable<IGrouping<uint, ISingleEventTree>> groups = Events.GroupBy(x => x.ThreadUid);
            uint endTime = ProfilingTimer.CurrentTime;
            foreach (IGrouping<uint, ISingleEventTree> group in groups)
            {
                ThreadTimeline item = new ThreadTimeline(this, group.Key, group.ToList(), endTime);
                _collection.Add(item);
            }
        }

        private static void OnEventsPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            Timeline view = (Timeline)sender;
            view.InitializeChildren();
        }

        private static void OnProfilingTimerPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            Timeline view = (Timeline)sender;
            view.InitializeChildren();
        }

        private static void OnHoveredItemPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            Timeline view = (Timeline)sender;
            if (view.HoveredItem != null)
            {
                view.HoveredEventTree = view.HoveredItem.EventTree;
            }
        }

        private static void OnSelectedItemPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            Timeline view = (Timeline)sender;
            ThreadTimelineItem oldSelectedItem = (ThreadTimelineItem)e.OldValue;
            ThreadTimelineItem newSelectedItem = (ThreadTimelineItem)e.NewValue;
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
                view.SelectedEventTree = view.SelectedItem.EventTree;
            }
        }

    }
}
