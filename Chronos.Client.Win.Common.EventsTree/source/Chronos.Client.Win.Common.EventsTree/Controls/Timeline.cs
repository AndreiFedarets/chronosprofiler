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
        //private static readonly DependencyProperty ThreadsProperty;

        private readonly ObservableCollection<ThreadTimeline> _board;
        private ItemsControl _itemsControl;

        static Timeline()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(Timeline), new FrameworkPropertyMetadata(typeof(Timeline)));
            // Events
            EventsProperty = DependencyProperty.Register("Events", typeof(IEventTreeCollection), typeof(Timeline), new PropertyMetadata(OnEventsPropertyChanged));
            ProfilingTimerProperty = DependencyProperty.Register("ProfilingTimer", typeof(IProfilingTimer), typeof(Timeline), new PropertyMetadata(OnProfilingTimerPropertyChanged));
            //ThreadsProperty = DependencyProperty.Register("Threads", typeof(ISingleEventTree), typeof(Timeline), new PropertyMetadata(OnEventsPropertyChanged));
        }

        public Timeline()
        {
            _board = new ObservableCollection<ThreadTimeline>();
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

        private bool AreChildrenInitialized { get; set; }

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

        private void InitializeChildren()
        {
            _board.Clear();
            if (Events == null || ProfilingTimer == null)
            {
                return;
            }
            IEnumerable<IGrouping<uint, ISingleEventTree>> groups = Events.GroupBy(x => x.ThreadUid);
            foreach (IGrouping<uint, ISingleEventTree> group in groups)
            {
                ThreadTimeline item = new ThreadTimeline(group.Key, group.ToList(), ProfilingTimer);
                _board.Add(item);
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
    }
}
