using Chronos.Common.EventsTree;
using System.Windows;
using System.Windows.Controls;

namespace Chronos.Client.Win.Controls.Common.EventsTree
{
    public class ThreadTimelineItem : Control
    {
        private readonly ThreadTimeline _parent;
        private readonly ISingleEventTree _eventTree;
        private readonly IProfilingTimer _profilingTimer;

        static ThreadTimelineItem()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ThreadTimelineItem), new FrameworkPropertyMetadata(typeof(ThreadTimelineItem)));
        }

        public ThreadTimelineItem(ISingleEventTree eventTree, IProfilingTimer profilingTimer)
        {
            _eventTree = eventTree;
            _profilingTimer = profilingTimer;
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            UpdateLocationAndSize();
        }

        protected override Size MeasureOverride(Size constraint)
        {
            UpdateLocationAndSize();
            return base.MeasureOverride(constraint);
        }

        private void UpdateLocationAndSize()
        {
            double parentWidth = _parent.ActualWidth;

        }
    }
}
