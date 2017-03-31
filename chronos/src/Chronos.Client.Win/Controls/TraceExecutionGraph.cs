using System.Windows;
using System.Windows.Controls;
using Chronos.Core;

namespace Chronos.Client.Win.Controls
{
	[TemplatePart(Name = BlocksPanelPartName, Type = typeof(ItemsControl))]
	public class TraceExecutionGraph : Control
	{
		private const string BlocksPanelPartName = "BlocksPanel";

		public static readonly DependencyProperty TraceProperty;
		public static readonly DependencyProperty HoveredEventProperty;
		public static readonly DependencyProperty SelectedEventProperty;
		public static readonly DependencyProperty ProcessInfoProperty;

		static TraceExecutionGraph()
		{
			DefaultStyleKeyProperty.OverrideMetadata(typeof(TraceExecutionGraph), new FrameworkPropertyMetadata(typeof(TraceExecutionGraph)));
			TraceProperty = DependencyProperty.Register("Trace", typeof(IThreadTrace), typeof(TraceExecutionGraph), new PropertyMetadata());
			HoveredEventProperty = DependencyProperty.Register("HoveredEvent", typeof(IEvent), typeof(TraceExecutionGraph), new PropertyMetadata(HoveredEventPropertyChanged));
			SelectedEventProperty = DependencyProperty.Register("SelectedEvent", typeof(IEvent), typeof(TraceExecutionGraph), new PropertyMetadata(default(IEvent), SelectedEventPropertyChangedCallback));
			ProcessInfoProperty = DependencyProperty.Register("ProcessInfo", typeof(ProcessInfo), typeof(TraceExecutionGraph));
		}

		private Canvas _blocksPanel;
		private TraceExecutionGraphBlock _hoveredBlock;

		public ProcessInfo ProcessInfo
		{
			get { return (ProcessInfo)GetValue(ProcessInfoProperty); }
			set { SetValue(ProcessInfoProperty, value); }
		}

		public IThreadTrace Trace
		{
			get { return (IThreadTrace)GetValue(TraceProperty); }
			set { SetValue(TraceProperty, value); }
		}

		public IEvent SelectedEvent
		{
			get { return (IEvent)GetValue(SelectedEventProperty); }
			set { SetValue(SelectedEventProperty, value); }
		}

		public IEvent HoveredEvent
		{
			get { return (IEvent)GetValue(HoveredEventProperty); }
			set { SetValue(HoveredEventProperty, value); }
		}

		public override void OnApplyTemplate()
		{
			base.OnApplyTemplate();
			_blocksPanel = (Canvas)(GetTemplateChild(BlocksPanelPartName));
			Initialize();
		}

		private void Initialize()
		{
			if (_blocksPanel == null || Trace == null)
			{
				return;
			}
			//foreach (ICallstack callstack in Trace)
			//{
			//    TraceExecutionGraphBlock block = new TraceExecutionGraphBlock(this);
			//    block.Event = callstack.EntryPoint;
			//    block.ProcessInfo = ProcessInfo;
			//    _blocksPanel.Children.Add(block);
			//}
		}

		protected override Size MeasureOverride(Size constraint)
		{
			Size size = base.MeasureOverride(constraint);
			return size;
		}

		protected override void OnRenderSizeChanged(SizeChangedInfo sizeInfo)
		{
			base.OnRenderSizeChanged(sizeInfo);
			if (_blocksPanel == null)
			{
				return;
			}
			Size size = new Size(_blocksPanel.ActualWidth, _blocksPanel.ActualHeight);
			foreach (object item in _blocksPanel.Children)
			{
				TraceExecutionGraphBlock block = (TraceExecutionGraphBlock)item;
				block.ParentSize = size;
			}
		}

		private static void HoveredEventPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
		{
			TraceExecutionGraph graph = (TraceExecutionGraph) sender;
			IEvent @event = (IEvent)e.NewValue;
			if (@event == null)
			{
				return;
			}
			TraceExecutionGraphBlock blockToHover = null;
			foreach (object item in graph._blocksPanel.Children)
			{
				TraceExecutionGraphBlock block = (TraceExecutionGraphBlock)item;
				if (block.Event == @event)
				{
					blockToHover = block;
					break;
				}
			}
			if (graph._hoveredBlock == blockToHover)
			{
				return;
			}
			if (graph._hoveredBlock != null)
			{
				graph._hoveredBlock.IsHovered = false;
			}
			if (blockToHover != null)
			{
				blockToHover.IsHovered = true;
			}
			graph._hoveredBlock = blockToHover;
		}

		private static void SelectedEventPropertyChangedCallback(DependencyObject sender, DependencyPropertyChangedEventArgs e)
		{
			TraceExecutionGraph graph = (TraceExecutionGraph)sender;
			IEvent coreEvent = (IEvent)e.NewValue;
			if (graph != null && coreEvent != null)
			{
				//graph.Navigate(coreEvent);
			}
		}

	}
}
