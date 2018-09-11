using Chronos.Common.EventsTree;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Chronos.Client.Win.Controls.Common.EventsTree
{
    [TemplatePart(Name = EventNameTextBlockPartName, Type = typeof(TextBlock))]
    [TemplatePart(Name = EventTimeTextBlockPartName, Type = typeof(TextBlock))]
    public sealed class ThreadTimelineItem : Control
    {
        private const string EventNameTextBlockPartName = "EventNameTextBlock";
        private const string EventTimeTextBlockPartName = "EventTimeTextBlock";

        private static readonly DependencyPropertyKey IsHoveredPropertyKey;
        public static readonly DependencyProperty IsHoveredProperty;
        private static readonly DependencyPropertyKey IsSelectedPropertyKey;
        public static readonly DependencyProperty IsSelectedProperty;
        private static readonly DependencyPropertyKey EventNamePropertyKey;
        public static readonly DependencyProperty EventNameProperty;
        private static readonly DependencyPropertyKey EventTimePropertyKey;
        public static readonly DependencyProperty EventTimeProperty;

        private readonly IEventMessageBuilder _eventMessageBuilder;
        private readonly Timeline _timeline;
        private readonly uint _endTime;

        static ThreadTimelineItem()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ThreadTimelineItem), new FrameworkPropertyMetadata(typeof(ThreadTimelineItem)));
            IsHoveredPropertyKey = DependencyProperty.RegisterReadOnly("IsHovered", typeof(bool), typeof(ThreadTimelineItem), new PropertyMetadata(OnIsHoveredPropertyChanged));
            IsHoveredProperty = IsHoveredPropertyKey.DependencyProperty;
            IsSelectedPropertyKey = DependencyProperty.RegisterReadOnly("IsSelected", typeof(bool), typeof(ThreadTimelineItem), new PropertyMetadata(OnIsSelectedPropertyChanged));
            IsSelectedProperty = IsSelectedPropertyKey.DependencyProperty;
            EventNamePropertyKey = DependencyProperty.RegisterReadOnly("EventName", typeof(string), typeof(ThreadTimelineItem), new PropertyMetadata());
            EventNameProperty = IsSelectedPropertyKey.DependencyProperty;
            EventTimePropertyKey = DependencyProperty.RegisterReadOnly("EventTime", typeof(uint), typeof(ThreadTimelineItem), new PropertyMetadata());
            EventTimeProperty = IsSelectedPropertyKey.DependencyProperty;
        }

        public ThreadTimelineItem(Timeline timeline, IEventMessageBuilder eventMessageBuilder, ISingleEventTree eventTree, uint endTime)
        {
            _timeline = timeline;
            _eventMessageBuilder = eventMessageBuilder;
            EventTree = eventTree;
            _endTime = endTime;
        }

        public ISingleEventTree EventTree { get; private set; }

        public string EventName
        {
            get { return (string)GetValue(EventNameProperty); }
            private set { SetValue(EventNamePropertyKey, value); }
        }

        public uint EventTime
        {
            get { return (uint)GetValue(EventTimeProperty); }
            private set { SetValue(EventTimePropertyKey, value); }
        }

        public bool IsHovered
        {
            get { return (bool)GetValue(IsHoveredProperty); }
            private set { SetValue(IsHoveredPropertyKey, value); }
        }

        public bool IsSelected
        {
            get { return (bool)GetValue(IsSelectedProperty); }
            internal set { SetValue(IsSelectedPropertyKey, value); }
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            //------------------
            string eventName = _eventMessageBuilder.BuildMessage(EventTree);
            uint eventTime = EventTree.Time;
            ToolTip = string.Concat(eventName, Environment.NewLine, eventTime, "ms");
        }

        internal void UpdateLocationAndSize(Size containerSize)
        {
            const double minWidth = 1.0;
            double containerWidth = containerSize.Width;
            double k = containerWidth / _endTime;
            double left = Math.Round(k * EventTree.BeginLifetime);
            double width = Math.Round(k * (EventTree.EndLifetime - EventTree.BeginLifetime));
            Width = width >= minWidth ? width : minWidth;
            SetValue(Canvas.LeftProperty, left);
        }

        protected override void OnMouseEnter(MouseEventArgs e)
        {
            base.OnMouseEnter(e);
            IsHovered = true;
        }

        protected override void OnMouseLeave(MouseEventArgs e)
        {
            base.OnMouseEnter(e);
            IsHovered = false;
        }

        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonDown(e);
            IsSelected = true;
        }

        private void SynchronizeIsHovered(bool oldValue, bool newValue)
        {
            if (oldValue == newValue)
            {
                return;
            }
            if (newValue)
            {
                _timeline.HoveredItem = this;
            }
            else if (ReferenceEquals(_timeline.HoveredItem, this))
            {
                _timeline.HoveredItem = null;
            }
        }

        private void SynchronizeIsSelected(bool oldValue, bool newValue)
        {
            if (oldValue == newValue)
            {
                return;
            }
            if (newValue)
            {
                _timeline.SelectedItem = this;
            }
            else if (_timeline.SelectedItem == this)
            {
                _timeline.SelectedItem = null;
            }
        }
        
        private static void OnIsHoveredPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            ThreadTimelineItem item = (ThreadTimelineItem)sender;
            item.SynchronizeIsHovered((bool)e.OldValue, (bool)e.NewValue);
        }

        private static void OnIsSelectedPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            ThreadTimelineItem item = (ThreadTimelineItem)sender;
            item.SynchronizeIsSelected((bool)e.OldValue, (bool)e.NewValue);
        }
    }
}
