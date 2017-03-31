using System.Windows;
using System.Windows.Controls;
using Chronos.Core;

namespace Chronos.Client.Win.Controls
{
	[TemplatePart(Name = LeftBorderPartName, Type = typeof(Border))]
	[TemplatePart(Name = CenterBorderPartName, Type = typeof(Border))]
	[TemplatePart(Name = RightBorderPartName, Type = typeof(Border))]
	public class UnitLifetimeVisual : Control
	{
		private const double DefaultWidth = 200;
		private const double DefaultHeight = 15;
		private const string LeftBorderPartName = "LeftBorder";
		private const string CenterBorderPartName = "CenterBorder";
		private const string RightBorderPartName = "RightBorder";

		public static readonly DependencyProperty UnitProperty;
		public static readonly DependencyProperty ProcessInfoProperty;

		static UnitLifetimeVisual()
		{
			DefaultStyleKeyProperty.OverrideMetadata(typeof(UnitLifetimeVisual), new FrameworkPropertyMetadata(typeof(UnitLifetimeVisual)));
			UnitProperty = DependencyProperty.Register("Unit", typeof(UnitBase), typeof(UnitLifetimeVisual), new PropertyMetadata());
			ProcessInfoProperty = DependencyProperty.Register("ProcessInfo", typeof(ProcessInfo), typeof(UnitLifetimeVisual), new PropertyMetadata());
		}

		private Border _leftBorder;
		private Border _centerBorder;
		private Border _rightBorder;

		public UnitBase Unit
		{
			get { return (UnitBase)GetValue(UnitProperty); }
			set { SetValue(UnitProperty, value); }
		}

		public ProcessInfo ProcessInfo
		{
			get { return (ProcessInfo)GetValue(ProcessInfoProperty); }
			set { SetValue(ProcessInfoProperty, value); }
		}

		public override void OnApplyTemplate()
		{
			base.OnApplyTemplate();
			_leftBorder = GetTemplateChild(LeftBorderPartName) as Border;
			_centerBorder = GetTemplateChild(CenterBorderPartName) as Border;
			_rightBorder = GetTemplateChild(RightBorderPartName) as Border;
			Initialize();
		}

		private void Initialize()
		{
			//if (_centerBorder == null || _leftBorder == null || _rightBorder == null || Unit == null || ProcessInfo == null)
			//{
			//    return;
			//}
			//Height = DefaultHeight;

			//double ratio = DefaultWidth / (ProcessInfo.EndTime - ProcessInfo.StartTime);
			//double centerWidth = ratio * (Unit.EndLifetime - Unit.BeginLifetime);
			//double leftWidth = ratio * (Unit.BeginLifetime - ProcessInfo.StartTime);
			//if (centerWidth < 1)
			//{
			//    centerWidth = 1;
			//}
			//_centerBorder.Width = centerWidth;
			//_leftBorder.Width = leftWidth;
			//_rightBorder.Width = Math.Abs(DefaultWidth - centerWidth - leftWidth);
		}
	}
}
