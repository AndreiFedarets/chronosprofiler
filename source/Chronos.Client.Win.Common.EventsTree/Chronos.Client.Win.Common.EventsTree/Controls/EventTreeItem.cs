using System;
using Chronos.Client.Win.Converters;
using Chronos.Common.EventsTree;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Collections;
using System.Collections.ObjectModel;
using Adenium;

namespace Chronos.Client.Win.Controls.Common.EventsTree
{
    //[TemplatePart(Name = HeaderPanelPartName, Type = typeof(Panel))]
    //[TemplatePart(Name = ContentPanelPartName, Type = typeof(Panel))]
    //[TemplatePart(Name = FooterPanelPartName, Type = typeof(Panel))]
    [TemplatePart(Name = ChildrenIndicatorBorderPartName, Type = typeof(Border))]
    [TemplatePart(Name = PercentsColorIndicatorBorderPartName, Type = typeof(Border))]
    [TemplatePart(Name = PercentTextBlockPartName, Type = typeof(TextBlock))]
    [TemplatePart(Name = NameTextBlockPartName, Type = typeof(TextBlock))]
    [TemplatePart(Name = TimeTextBlockPartName, Type = typeof(TextBlock))]
    [TemplatePart(Name = HitsTextBlockPartName, Type = typeof(TextBlock))]
    public class EventTreeItem : Control
    {
        //private const string HeaderPanelPartName = "HeaderPanel";
        //private const string ContentPanelPartName = "ContentPanel";
        //private const string FooterPanelPartName = "FooterPanel";
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
        private readonly EventsTreeView _treeView;
        private readonly IEvent _event;
        private readonly int _level;
        private EventTreeSortType _eventsSortType;
        private EventTreeItemCollection _children;
        private Lazy<string> _eventText;
        //private Panel _headerPanel;
        //private Panel _contentPanel;
        //private Panel _footerPanel;

        static EventTreeItem()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(EventTreeItem), new FrameworkPropertyMetadata(typeof(EventTreeItem)));
            LevelOffsetProperty = DependencyProperty.Register("LevelOffset", typeof(int), typeof(EventTreeItem), new PropertyMetadata(OnLevelOffsetPropertyChanged));
            IsExpandedPropertyKey = DependencyProperty.RegisterReadOnly("IsExpanded", typeof(bool), typeof(EventTreeItem), new PropertyMetadata());
            IsExpandedProperty = IsExpandedPropertyKey.DependencyProperty;
            IsHoveredPropertyKey = DependencyProperty.RegisterReadOnly("IsHovered", typeof(bool), typeof(EventTreeItem), new PropertyMetadata(OnIsHoveredPropertyChanged));
            IsHoveredProperty = IsHoveredPropertyKey.DependencyProperty;
            IsSelectedPropertyKey = DependencyProperty.RegisterReadOnly("IsSelected", typeof(bool), typeof(EventTreeItem), new PropertyMetadata(OnIsSelectedPropertyChanged));
            IsSelectedProperty = IsSelectedPropertyKey.DependencyProperty;
        }

        public EventTreeItem(IEvent @event, IEventMessageBuilder eventMessageBuilder, EventTreeItem parent, EventsTreeView treeView, int level, EventTreeSortType sortType)
        {
            _event = @event;
            _eventMessageBuilder = eventMessageBuilder;
            ParentItem = parent;
            _treeView = treeView;
            _level = level;
            _eventsSortType = sortType;
            _eventText = new Lazy<string>(GetEventText);
        }

        public EventTreeItem ParentItem { get; private set; }

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

        public ReadOnlyCollection<EventTreeItem> Children
        {
            get
            {
                InitializeChildren();
                return _children;
            }
        }

        public string EventText
        {
            get { return _eventText.Value; }
        }

        private int BoardIndex
        {
            get { return _treeView.IndexOf(this); }
        }

        private bool AreChildrenInitialized
        {
            get { return _children != null; }
        }

        public void Select(bool scroll)
        {
            if (ParentItem != null)
            {
                ParentItem.Expand();
            }
            IsSelected = true;
            if (scroll)
            {
                _treeView.ScrollTo(this);
            }
        }

        public void Expand()
        {
            if (IsExpanded)
            {
                return;
            }
            if (ParentItem != null)
            {
                ParentItem.Expand();   
            }
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
            foreach (EventTreeItem item in _children)
            {
                item.Collapse();
            }
            _treeView.RemoveRange(BoardIndex + 1, _children.Count);
            IsExpanded = false;
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            //------------------
            //_headerPanel = GetTemplateChild(HeaderPanelPartName) as Panel;
            //_contentPanel = GetTemplateChild(ContentPanelPartName) as Panel;
            //_footerPanel = GetTemplateChild(FooterPanelPartName) as Panel;
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
                nameTextBlock.Text = EventText;
            }
            SynchronizeLevelOffset(LevelOffset);
        }

        private string GetEventText()
        {
            return _eventMessageBuilder.BuildMessage(_event);
        }

        private void Sort()
        {
            _treeView.RemoveRange(BoardIndex + 1, _children.Count);
            _children.Sort(_eventsSortType);
            _treeView.InsertRange(BoardIndex + 1, _children);
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
            List<EventTreeItem> children = new List<EventTreeItem>();
            if (_event.HasChildren)
            {
                foreach (IEvent @event in _event.Children)
                {
                    EventTreeItem item = new EventTreeItem(@event, _eventMessageBuilder, this, _treeView, _level + 1, EventsSortType);
                    children.Add(item);
                }
                _children = new EventTreeItemCollection(children);
                _children.Sort(_eventsSortType);
                //TODO: why do we have it here ???
                DispatcherExtensions.DoEvents();
            }
            else
            {
                _children = new EventTreeItemCollection();
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
            EventTreeItem item = (EventTreeItem)sender;
            int oldValue = (int) e.OldValue;
            int newValue = (int) e.NewValue;
            if (oldValue != newValue)
            {
                item.SynchronizeLevelOffset(newValue);
            }
        }

        private static void OnIsHoveredPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            EventTreeItem item = (EventTreeItem)sender;
            item.SynchronizeIsHovered((bool)e.OldValue, (bool)e.NewValue);
        }

        private static void OnIsSelectedPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            EventTreeItem item = (EventTreeItem)sender;
            item.SynchronizeIsSelected((bool)e.OldValue, (bool)e.NewValue);
        }
    }
}
