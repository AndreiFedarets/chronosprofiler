using Chronos.Common.EventsTree;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;

namespace Chronos.Client.Win.Controls.Common.EventsTree
{
    [TemplatePart(Name = ItemsControlPartName, Type = typeof(ItemsControl))]
    public class ThreadTimeline : Control
    {
        private const string ItemsControlPartName = "ItemsControl";

        private readonly ObservableCollection<ThreadTimelineItem> _collection;
        private readonly uint _threadUid;
        private readonly IEnumerable<ISingleEventTree> _eventTrees;
        private readonly uint _endTime;
        private readonly Timeline _timeline;
        private ItemsControl _itemsControl;

        static ThreadTimeline()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ThreadTimeline), new FrameworkPropertyMetadata(typeof(ThreadTimeline)));
        }

        public ThreadTimeline(Timeline timeline, uint threadUid, List<ISingleEventTree> eventTrees, uint endTime)
        {
            _timeline = timeline;
            _threadUid = threadUid;
            _eventTrees = eventTrees;
            _endTime = endTime;
            _collection = new ObservableCollection<ThreadTimelineItem>();
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
            InitializeChildren();
        }

        private void UpdateLocationAndSize()
        {
            Size containerSize = new Size(ActualWidth, ActualHeight);
            foreach (ThreadTimelineItem item in _collection)
            {
                item.UpdateLocationAndSize(containerSize);
            }
        }

        protected override void OnRenderSizeChanged(SizeChangedInfo sizeInfo)
        {
            UpdateLocationAndSize();
            base.OnRenderSizeChanged(sizeInfo);
        }

        private void InitializeChildren()
        {
            _collection.Clear();
            foreach (ISingleEventTree eventTree in _eventTrees)
            {
                ThreadTimelineItem item = new ThreadTimelineItem(_timeline, eventTree, _endTime);
                _collection.Add(item);
            }
        }

        private static void OnEventsPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            ThreadTimeline view = (ThreadTimeline)sender;
            view.InitializeChildren();
        }
    }
}
