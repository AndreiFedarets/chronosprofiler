using System.Windows;
using System.Windows.Controls;
using Chronos.Core;

namespace Chronos.Client.Win.Controls
{
	public class TraceExecutionGraphBlock : Control
	{
		public static readonly DependencyProperty EventProperty;
		public static readonly DependencyProperty ProcessInfoProperty;
		public static readonly DependencyProperty ParentSizeProperty;
		public static readonly DependencyProperty IsHoveredProperty;

		static TraceExecutionGraphBlock()
		{
			DefaultStyleKeyProperty.OverrideMetadata(typeof(TraceExecutionGraphBlock), new FrameworkPropertyMetadata(typeof(TraceExecutionGraphBlock)));
			EventProperty = DependencyProperty.Register("Event", typeof(IEvent), typeof(TraceExecutionGraphBlock), new PropertyMetadata());
			ProcessInfoProperty = DependencyProperty.Register("ProcessInfo", typeof(ProcessInfo), typeof(TraceExecutionGraphBlock), new PropertyMetadata());
			ParentSizeProperty = DependencyProperty.Register("ParentSize", typeof(Size), typeof(TraceExecutionGraphBlock), new PropertyMetadata(OnParentSizePropertyChanged));
			IsHoveredProperty = DependencyProperty.Register("IsHovered", typeof(bool), typeof(TraceExecutionGraphBlock), new PropertyMetadata(OnIsHoveredPropertyChanged));
		}

		private readonly TraceExecutionGraph _graph;

		public TraceExecutionGraphBlock(TraceExecutionGraph graph)
		{
			_graph = graph;
		}

		public bool IsHovered
		{
			get { return (bool)GetValue(IsHoveredProperty); }
			set { SetValue(IsHoveredProperty, value); }
		}

		public Size ParentSize
		{
			get { return (Size)GetValue(ParentSizeProperty); }
			set { SetValue(ParentSizeProperty, value); }
		}

		public double Left
		{
			get { return (double)GetValue(Canvas.LeftProperty); }
			set { SetValue(Canvas.LeftProperty, value); }
		}

		public IEvent Event
		{
			get { return (IEvent)GetValue(EventProperty); }
			set { SetValue(EventProperty, value); }
		}

		public ProcessInfo ProcessInfo
		{
			get { return (ProcessInfo)GetValue(ProcessInfoProperty); }
			set { SetValue(ProcessInfoProperty, value); }
		}

		protected override void OnMouseEnter(System.Windows.Input.MouseEventArgs e)
		{
			base.OnMouseEnter(e);
			IsHovered = true;
		}

		protected override void OnMouseLeave(System.Windows.Input.MouseEventArgs e)
		{
			base.OnMouseLeave(e);
			IsHovered = false;
		}

		private static void OnParentSizePropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
		{
			//TraceExecutionGraphBlock block = (TraceExecutionGraphBlock) sender;
			//Size parentSize = (Size)e.NewValue;
			//double ratio = parentSize.Width / (block.ProcessInfo.EndTime - block.ProcessInfo.StartTime);
			//double left = ratio * (block.Event.BeginTime - block.ProcessInfo.StartTime);
			//double width = ratio * (block.Event.EndTime - block.Event.BeginTime);
			//if (width < 1)
			//{
			//    width = 1;
			//}
			//block.Width = width;
			//block.Height = parentSize.Height;
			//block.Left = left;
		}

		private static void OnIsHoveredPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
		{
			TraceExecutionGraphBlock block = (TraceExecutionGraphBlock)sender;
			block._graph.HoveredEvent = block.Event;
		}
	}
}
