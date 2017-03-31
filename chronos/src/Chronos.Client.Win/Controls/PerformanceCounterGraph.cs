using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using Chronos.Core;

namespace Chronos.Client.Win.Controls
{
	[TemplatePart(Name = PathPartName, Type = typeof(ItemsControl))]
	public class PerformanceCounterGraph : Control
	{
		private const string PathPartName = "Path";
		public static readonly DependencyProperty PerformanceCounterProperty;
		public static readonly DependencyProperty ProcessInfoProperty;
		

		static PerformanceCounterGraph()
		{
			DefaultStyleKeyProperty.OverrideMetadata(typeof(PerformanceCounterGraph), new FrameworkPropertyMetadata(typeof(PerformanceCounterGraph)));
			PerformanceCounterProperty = DependencyProperty.Register("PerformanceCounter", typeof(IPerformanceCounter), typeof(PerformanceCounterGraph), new PropertyMetadata(default(IPerformanceCounter)));
			ProcessInfoProperty = DependencyProperty.Register("ProcessInfo", typeof(ProcessInfo), typeof(PerformanceCounterGraph), new PropertyMetadata(default(ProcessInfo), OnProcessInfoPropertyChanged));
		}

		private long _zeroTime;
		private readonly PathGeometry _geometry;
		private Path _path;

		public PerformanceCounterGraph()
		{
			_geometry = new PathGeometry();
		}

		public IPerformanceCounter PerformanceCounter
		{
			get { return (IPerformanceCounter)GetValue(PerformanceCounterProperty); }
			set { SetValue(PerformanceCounterProperty, value); }
		}

		public ProcessInfo ProcessInfo
		{
			get { return (ProcessInfo)GetValue(ProcessInfoProperty); }
			set { SetValue(ProcessInfoProperty, value); }
		}

		public override void OnApplyTemplate()
		{
			base.OnApplyTemplate();
			_path = (Path)(GetTemplateChild(PathPartName));
			Initialize();
		}

		private void Initialize()
		{
			if (_path == null || PerformanceCounter == null)
			{
				return;
			}
			_geometry.Clear();
			_path.Data = _geometry;
			CounterPoint[] points = PerformanceCounter.ToArray();
			if (points.Length < 2)
			{
				return;
			}
			CounterPoint previous = points[0];
			for (int i = 1; i < points.Length; i++)
			{
				CounterPoint current = points[i];
				LineGeometry line = new LineGeometry(TranslateCounterPoint(previous), TranslateCounterPoint(current));
				_geometry.AddGeometry(line);
				previous = current;
			}
		}

		private Point TranslateCounterPoint(CounterPoint counterPoint)
		{
			double time = (double)(counterPoint.Time - _zeroTime) / 1000000;
			double value = counterPoint.Value;
			return new Point(time, value);
		}

		private static void OnProcessInfoPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
		{
			PerformanceCounterGraph control = (PerformanceCounterGraph) sender;
			ProcessInfo processInfo = (ProcessInfo)e.NewValue;
			if (processInfo == null)
			{
				return;
			}
			control._zeroTime = processInfo.StartTime.ToFileTime();
		}
	}
}
