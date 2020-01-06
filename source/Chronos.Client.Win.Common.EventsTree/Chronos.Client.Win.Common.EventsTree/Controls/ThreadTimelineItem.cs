using Chronos.Common.EventsTree;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace Chronos.Client.Win.Controls.Common.EventsTree
{
    //[TemplatePart(Name = ItemRectanglePartName, Type = typeof(Rectangle))]
    public sealed class ThreadTimelineItem : Control
    {
        //private const string ItemRectanglePartName = "ItemRectangle";

        private static readonly DependencyPropertyKey IsHoveredPropertyKey;
        public static readonly DependencyProperty IsHoveredProperty;
        private static readonly DependencyPropertyKey IsSelectedPropertyKey;
        public static readonly DependencyProperty IsSelectedProperty;

        private readonly IEventMessageBuilder _eventMessageBuilder;
        private readonly Timeline _timeline;
        private readonly uint _endTime;
        private readonly uint _minTime;
        private readonly uint _maxTime;

        static ThreadTimelineItem()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ThreadTimelineItem), new FrameworkPropertyMetadata(typeof(ThreadTimelineItem)));
            IsHoveredPropertyKey = DependencyProperty.RegisterReadOnly("IsHovered", typeof(bool), typeof(ThreadTimelineItem), new PropertyMetadata(OnIsHoveredPropertyChanged));
            IsHoveredProperty = IsHoveredPropertyKey.DependencyProperty;
            IsSelectedPropertyKey = DependencyProperty.RegisterReadOnly("IsSelected", typeof(bool), typeof(ThreadTimelineItem), new PropertyMetadata(OnIsSelectedPropertyChanged));
            IsSelectedProperty = IsSelectedPropertyKey.DependencyProperty;
        }

        public ThreadTimelineItem(Timeline timeline, IEventMessageBuilder eventMessageBuilder, ISingleEventTree eventTree, uint endTime, uint minTime, uint maxTime)
        {
            _timeline = timeline;
            _eventMessageBuilder = eventMessageBuilder;
            EventTree = eventTree;
            _endTime = endTime;
            _minTime = minTime;
            _maxTime = maxTime;
        }

        protected override void OnMouseDoubleClick(MouseButtonEventArgs e)
        {
            base.OnMouseDoubleClick(e);
            _timeline.OnOpenRequest(EventTree);
        }

        public ISingleEventTree EventTree { get; private set; }

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
            //------------------
            double eventPercent = NativeEventHelper.GetEventPercent(eventTime, _minTime, _maxTime);
            Background = new SolidColorBrush(PercentsToColorConverter.Convert(eventPercent));
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
