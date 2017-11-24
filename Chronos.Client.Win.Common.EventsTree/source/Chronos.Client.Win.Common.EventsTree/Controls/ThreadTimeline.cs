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

        private readonly ObservableCollection<ThreadTimelineItem> _board;
        private ItemsControl _itemsControl;
        private uint _threadUid;
        private IEnumerable<ISingleEventTree> _eventTrees;
        private IProfilingTimer _profilingTimer;

        static ThreadTimeline()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ThreadTimeline), new FrameworkPropertyMetadata(typeof(ThreadTimeline)));
        }

        public ThreadTimeline(uint threadUid, List<ISingleEventTree> eventTrees, IProfilingTimer profilingTimer)
        {
            _threadUid = threadUid;
            _eventTrees = eventTrees;
            _profilingTimer = profilingTimer;
            _board = new ObservableCollection<ThreadTimelineItem>();
        }

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
            foreach (ISingleEventTree eventTree in _eventTrees)
            {
                ThreadTimelineItem item = new ThreadTimelineItem(eventTree, _profilingTimer);
                _board.Add(item);
            }
        }

        private static void OnEventsPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            ThreadTimeline view = (ThreadTimeline)sender;
            view.InitializeChildren();
        }
    }
}
