using Chronos.Common.EventsTree;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Chronos.Client.Win.Controls.Common.EventsTree
{
    [TemplatePart(Name = ItemsControlPartName, Type = typeof(ItemsControl))]
    [TemplatePart(Name = ContainerGridPartName, Type = typeof(Grid))]
    [TemplatePart(Name = ScrollViewerPartName, Type = typeof(ScrollViewer))]
    [TemplatePart(Name = ContentBorderPartName, Type = typeof(Border))]
    public class Timeline : Control
    {
        private const string ItemsControlPartName = "ItemsControl";
        private const string ContainerGridPartName = "ContainerGrid";
        private const string ScrollViewerPartName = "ScrollViewer";
        private const string ContentBorderPartName = "ContentBorder";
        private const int ZoomStep = 3;

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
        private static readonly DependencyProperty EventMessageBuilderProperty;
        private static readonly DependencyProperty OpenCommandProperty;
        //private static readonly DependencyProperty ThreadsProperty;

        private readonly ObservableCollection<ThreadTimeline> _collection;
        private ItemsControl _itemsControl;
        private Grid _containerGrid;
        private ScrollViewer _scrollViewer;
        private Border _contentBorder;

        static Timeline()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(Timeline), new FrameworkPropertyMetadata(typeof(Timeline)));
            // Events
            EventsProperty = DependencyProperty.Register("Events", typeof(IEventTreeCollection), typeof(Timeline), new PropertyMetadata(OnEventsPropertyChanged));
            // ProfilingTimer
            ProfilingTimerProperty = DependencyProperty.Register("ProfilingTimer", typeof(IProfilingTimer), typeof(Timeline), new PropertyMetadata(OnProfilingTimerPropertyChanged));
            // OpenCommand
            OpenCommandProperty = DependencyProperty.Register("OpenCommand", typeof(ICommand), typeof(Timeline), new PropertyMetadata());
            // EventMessageBuilder
            EventMessageBuilderProperty = DependencyProperty.Register("EventMessageBuilder", typeof(IEventMessageBuilder), typeof(Timeline), new PropertyMetadata(OnEventFormatterPropertyChanged));
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
            PreviewMouseWheel += OnMouseWheel;
            SizeChanged += OnSizeChanged;
        }

        public ICommand OpenCommand
        {
            get { return (ICommand)GetValue(OpenCommandProperty); }
            set { SetValue(OpenCommandProperty, value); }
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

        public IEventMessageBuilder EventMessageBuilder
        {
            get { return (IEventMessageBuilder)GetValue(EventMessageBuilderProperty); }
            set { SetValue(EventMessageBuilderProperty, value); }
        }

        private void OnSizeChanged(object sender, SizeChangedEventArgs e)
        {
            UpdateContentBorder();
        }

        private void OnMouseWheel(object sender, MouseWheelEventArgs e)
        {
            if (_contentBorder == null)
            {
                return;
            }
            int change = e.Delta * ZoomStep;
            double newWidth = _contentBorder.ActualWidth + change;
            double horizontalOffset = _scrollViewer.HorizontalOffset + change / 2;
            _scrollViewer.ScrollToHorizontalOffset(horizontalOffset);
            ResizeContentBorder(newWidth);
        }

        private void UpdateContentBorder()
        {
            if (_contentBorder == null)
            {
                return;
            }
            ResizeContentBorder(_contentBorder.ActualWidth);
        }

        private void ResizeContentBorder(double newWidth)
        {
            if (_contentBorder == null || _containerGrid == null)
            {
                return;
            }
            if (newWidth < _containerGrid.ActualWidth)
            {
                newWidth = _containerGrid.ActualWidth;
            }
            _contentBorder.Width = newWidth;
            Size containerSize = new Size(newWidth, _contentBorder.ActualHeight);
            foreach (ThreadTimeline item in _collection)
            {
                item.UpdateLocationAndSize(containerSize);
            }
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            //------------------
            _itemsControl = GetTemplateChild(ItemsControlPartName) as ItemsControl;
            if (_itemsControl != null)
            {
                _itemsControl.ItemsSource = _collection;
            }
            //------------------
            _containerGrid = GetTemplateChild(ContainerGridPartName) as Grid;
            //------------------
            _scrollViewer = GetTemplateChild(ScrollViewerPartName) as ScrollViewer;
            //------------------
            _contentBorder = GetTemplateChild(ContentBorderPartName) as Border;
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
                ThreadTimeline item = new ThreadTimeline(this, EventMessageBuilder, group.Key, group.ToList(), endTime, Events.MinTime, Events.MaxTime);
                _collection.Add(item);
            }
            //DispatcherExtensions.DoEvents();
            UpdateContentBorder();
            InvalidateVisual();
        }

        private static void OnEventsPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            Timeline view = (Timeline)sender;
            view.InitializeChildren();
        }

        private static void OnEventFormatterPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
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

        internal void OnOpenRequest(ISingleEventTree eventTree)
        {
            ICommand command = OpenCommand;
            if (command != null)
            {
                command.Execute(eventTree);
            }
        }
    }
}
