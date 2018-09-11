using Chronos.Client.Win.Converters;
using Chronos.Common.EventsTree;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace Chronos.Client.Win.Controls.Common.EventsTree
{
    [TemplatePart(Name = HeaderPanelPartName, Type = typeof(Panel))]
    [TemplatePart(Name = ChildrenIndicatorBorderPartName, Type = typeof(Border))]
    [TemplatePart(Name = PercentsColorIndicatorBorderPartName, Type = typeof(Border))]
    [TemplatePart(Name = PercentTextBlockPartName, Type = typeof(TextBlock))]
    [TemplatePart(Name = ContentPanelPartName, Type = typeof(Panel))]
    [TemplatePart(Name = NameTextBlockPartName, Type = typeof(TextBlock))]
    [TemplatePart(Name = FooterPanelPartName, Type = typeof(Panel))]
    [TemplatePart(Name = TimeTextBlockPartName, Type = typeof(TextBlock))]
    [TemplatePart(Name = HitsTextBlockPartName, Type = typeof(TextBlock))]
    public class EventsTreeItem : Control
    {
        private const string HeaderPanelPartName = "HeaderPanel";
        private const string ContentPanelPartName = "ContentPanel";
        private const string FooterPanelPartName = "FooterPanel";
        private const string ChildrenIndicatorBorderPartName = "ChildrenIndicatorBorder";
        private const string PercentsColorIndicatorBorderPartName = "PercentsColorIndicatorBorder";
        private const string PercentTextBlockPartName = "PercentTextBlock";
        private const string TimeTextBlockPartName = "TimeTextBlock";
        private const string HitsTextBlockPartName = "HitsTextBlock";
        private const string NameTextBlockPartName = "NameTextBlock";

        public static readonly DependencyProperty LevelOffsetProperty;
        private static readonly DependencyPropertyKey IsExpandedPropertyKey;
        public static readonly DependencyProperty IsExpandedProperty;
        private static readonly DependencyPropertyKey IsHoveredPropertyKey;
        public static readonly DependencyProperty IsHoveredProperty;
        private static readonly DependencyPropertyKey IsSelectedPropertyKey;
        public static readonly DependencyProperty IsSelectedProperty;

        private readonly IEventMessageBuilder _eventMessageBuilder;
        private readonly EventsTreeItem _parent;
        private readonly EventsTreeView _treeView;
        private readonly IEvent _event;
        private readonly int _level;
        private EventTreeSortType _eventsSortType;
        private List<EventsTreeItem> _children;
        private Panel _headerPanel;
        private Panel _contentPanel;
        private Panel _footerPanel;

        static EventsTreeItem()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(EventsTreeItem), new FrameworkPropertyMetadata(typeof(EventsTreeItem)));
            LevelOffsetProperty = DependencyProperty.Register("LevelOffset", typeof(int), typeof(EventsTreeItem), new PropertyMetadata(OnLevelOffsetPropertyChanged));
            IsExpandedPropertyKey = DependencyProperty.RegisterReadOnly("IsExpanded", typeof(bool), typeof(EventsTreeItem), new PropertyMetadata());
            IsExpandedProperty = IsExpandedPropertyKey.DependencyProperty;
            IsHoveredPropertyKey = DependencyProperty.RegisterReadOnly("IsHovered", typeof(bool), typeof(EventsTreeItem), new PropertyMetadata(OnIsHoveredPropertyChanged));
            IsHoveredProperty = IsHoveredPropertyKey.DependencyProperty;
            IsSelectedPropertyKey = DependencyProperty.RegisterReadOnly("IsSelected", typeof(bool), typeof(EventsTreeItem), new PropertyMetadata(OnIsSelectedPropertyChanged));
            IsSelectedProperty = IsSelectedPropertyKey.DependencyProperty;
        }

        public EventsTreeItem(IEvent @event, IEventMessageBuilder eventMessageBuilder, EventsTreeItem parent, EventsTreeView treeView, int level, EventTreeSortType sortType)
        {
            _event = @event;
            _eventMessageBuilder = eventMessageBuilder;
            _parent = parent;
            _treeView = treeView;
            _level = level;
            _eventsSortType = sortType;
        }

        public bool IsRootItem
        {
            get { return Event is IEventTree; }
        }

        public EventTreeSortType EventsSortType
        {
            get { return _eventsSortType; }
            set
            {
                _eventsSortType = value;
                Sort();
            }
        }

        public bool IsExpanded
        {
            get { return (bool)GetValue(IsExpandedProperty); }
            private set { SetValue(IsExpandedPropertyKey, value); }
        }

        public int LevelOffset
        {
            get { return (int) GetValue(LevelOffsetProperty); }
            set { SetValue(LevelOffsetProperty, value);}
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

        public IEvent Event
        {
            get { return _event; }
        }

        private int BoardIndex
        {
            get { return _treeView.IndexOf(this); }
        }

        private bool AreChildrenInitialized
        {
            get { return _children != null; }
        }

        public void Expand()
        {
            InitializeChildren();
            _treeView.InsertRange(BoardIndex + 1, _children);
            IsExpanded = true;
        }

        public void Collapse()
        {
            if (!IsExpanded)
            {
                return;
            }
            foreach (EventsTreeItem item in _children)
            {
                item.Collapse();
            }
            _treeView.RemoveRange(BoardIndex + 1, _children.Count);
            IsExpanded = false;
        }

        private void Sort()
        {
            _treeView.RemoveRange(BoardIndex + 1, _children.Count);
            EventTreeItemSorter.Sort(_children, _eventsSortType);
            _treeView.InsertRange(BoardIndex + 1, _children);
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            //------------------
            _headerPanel = GetTemplateChild(HeaderPanelPartName) as Panel;
            _contentPanel = GetTemplateChild(ContentPanelPartName) as Panel;
            _footerPanel = GetTemplateChild(FooterPanelPartName) as Panel;
            double eventPercent = NativeEventHelper.GetEventPercent(_event);
            //------------------
            Border childrenIndicatorBorder = GetTemplateChild(ChildrenIndicatorBorderPartName) as Border;
            if (childrenIndicatorBorder != null)
            {
                childrenIndicatorBorder.Visibility = _event.HasChildren ? Visibility.Visible : Visibility.Hidden;
            }
            //------------------
            Border percentsColorIndicatorBorder = GetTemplateChild(PercentsColorIndicatorBorderPartName) as Border;
            if (percentsColorIndicatorBorder != null)
            {
                percentsColorIndicatorBorder.Visibility = IsRootItem ? Visibility.Collapsed : Visibility.Visible;
                percentsColorIndicatorBorder.Background = new SolidColorBrush(PercentsToColorConverter.Convert(eventPercent));
            }
            //------------------
            TextBlock percentTextBlock = GetTemplateChild(PercentTextBlockPartName) as TextBlock;
            if (percentTextBlock != null)
            {
                percentTextBlock.Text = string.Format("{0}%", eventPercent);
            }
            //------------------
            TextBlock timeTextBlock = GetTemplateChild(TimeTextBlockPartName) as TextBlock;
            if (timeTextBlock != null)
            {
                timeTextBlock.Text = string.Format("{0}ms", _event.Time);
            }
            //------------------
            TextBlock hitsTextBlock = GetTemplateChild(HitsTextBlockPartName) as TextBlock;
            if (hitsTextBlock != null)
            {
                hitsTextBlock.Text = string.Format("{0}hits", _event.Hits);
            }
            //------------------
            TextBlock nameTextBlock = GetTemplateChild(NameTextBlockPartName) as TextBlock;
            if (nameTextBlock != null)
            {
                nameTextBlock.Text = _eventMessageBuilder.BuildMessage(_event);
            }
            SynchronizeLevelOffset(LevelOffset);
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
            if (IsExpanded)
            {
                Collapse();
            }
            else
            {
                Expand();
            }
            IsSelected = true;
        }

        protected void InitializeChildren()
        {
            if (AreChildrenInitialized)
            {
                return;
            }
            _children = new List<EventsTreeItem>();
            if (_event.HasChildren)
            {
                foreach (IEvent @event in _event.Children)
                {
                    EventsTreeItem item = new EventsTreeItem(@event, _eventMessageBuilder, this, _treeView, _level + 1, EventsSortType);
                    _children.Add(item);
                }
                EventTreeItemSorter.Sort(_children, _eventsSortType);
                DispatcherExtensions.DoEvents();
            }
        }

        private void SynchronizeLevelOffset(int offset)
        {
            Thickness margin = Margin;
            margin.Left = offset * _level;
            Margin = margin;
        }

        private void SynchronizeIsHovered(bool oldValue, bool newValue)
        {
            if (oldValue == newValue)
            {
                return;
            }
            if (newValue)
            {
                _treeView.HoveredItem = this;
            }
            else if (_treeView.HoveredItem == this)
            {
                _treeView.HoveredItem = null;
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
                _treeView.SelectedItem = this;
            }
            else if (_treeView.SelectedItem == this)
            {
                _treeView.SelectedItem = null;
            }
        }
        
        private static void OnLevelOffsetPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            EventsTreeItem item = (EventsTreeItem)sender;
            int oldValue = (int) e.OldValue;
            int newValue = (int) e.NewValue;
            if (oldValue != newValue)
            {
                item.SynchronizeLevelOffset(newValue);
            }
        }

        private static void OnIsHoveredPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            EventsTreeItem item = (EventsTreeItem)sender;
            item.SynchronizeIsHovered((bool)e.OldValue, (bool)e.NewValue);
        }

        private static void OnIsSelectedPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            EventsTreeItem item = (EventsTreeItem)sender;
            item.SynchronizeIsSelected((bool)e.OldValue, (bool)e.NewValue);
        }
    }
}
