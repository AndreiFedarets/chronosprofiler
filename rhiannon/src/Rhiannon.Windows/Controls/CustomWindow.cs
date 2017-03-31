using System;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Rhiannon.Windows.Controls
{
	[TemplatePart(Name = MinimizeButtonPartName, Type = typeof(Button))]
	[TemplatePart(Name = RestoreButtonPartName, Type = typeof(Button))]
	[TemplatePart(Name = MaximizeButtonPartName, Type = typeof(Button))]
	[TemplatePart(Name = CloseButtonPartName, Type = typeof(Button))]
	[TemplatePart(Name = WindowBorderPartName, Type = typeof(Border))]
	[TemplatePart(Name = HeaderBorderPartName, Type = typeof(Border))]
	[TemplatePart(Name = TopLeftResizeShapePartName, Type = typeof(Shape))]
	[TemplatePart(Name = TopCenterResizeShapePartName, Type = typeof(Shape))]
	[TemplatePart(Name = TopRightResizeShapePartName, Type = typeof(Shape))]
	[TemplatePart(Name = CenterLeftResizeShapePartName, Type = typeof(Shape))]
	[TemplatePart(Name = CenterRightResizeShapePartName, Type = typeof(Shape))]
	[TemplatePart(Name = BottomLeftResizeShapePartName, Type = typeof(Shape))]
	[TemplatePart(Name = BottomCenterResizeShapePartName, Type = typeof(Shape))]
	[TemplatePart(Name = BottomRightResizeShapePartName, Type = typeof(Shape))]
	public class CustomWindow : Window
	{
		private const string TopLeftResizeShapePartName = "TopLeftResizeShape";
		private const string TopCenterResizeShapePartName = "TopCenterResizeShape";
		private const string TopRightResizeShapePartName = "TopRightResizeShape";
		private const string CenterLeftResizeShapePartName = "CenterLeftResizeShape";
		private const string CenterRightResizeShapePartName = "CenterRightResizeShape";
		private const string BottomLeftResizeShapePartName = "BottomLeftResizeShape";
		private const string BottomCenterResizeShapePartName = "BottomCenterResizeShape";
		private const string BottomRightResizeShapePartName = "BottomRightResizeShape";
		private const double DefaultWindowResizeAreaSize = 10.0;
		private const string MinimizeButtonPartName = "MinimizeButton";
		private const string RestoreButtonPartName = "RestoreButton";
		private const string MaximizeButtonPartName = "MaximizeButton";
		private const string CloseButtonPartName = "CloseButton";
		private const string WindowBorderPartName = "WindowBorder";
		private const string HeaderBorderPartName = "HeaderBorder";
		public static readonly DependencyProperty CanMaximizeProperty;
		public static readonly DependencyProperty CanMinimizeProperty;
		public static readonly DependencyProperty AdditionalIconProperty;
		public static readonly DependencyProperty TitleHeightProperty;
		public static readonly DependencyProperty WindowResizeAreaSizeProperty;

		private readonly Shape[,] _resizeShapes;
		private Button _minimizeButton;
		private Button _restoreButton;
		private Button _maximizeButton;
		private Button _closeButton;
		private Border _windowBorder;
		private Border _headerBorder;
		private HwndSource _hwndSource;
 

		static CustomWindow()
		{
			DefaultStyleKeyProperty.OverrideMetadata(typeof(CustomWindow), new FrameworkPropertyMetadata(typeof(CustomWindow)));
			CanMaximizeProperty = DependencyProperty.Register("CanMaximize", typeof(bool), typeof(CustomWindow), new PropertyMetadata(true, OnCanMaximizePropertyChanged));
			CanMinimizeProperty = DependencyProperty.Register("CanMinimize", typeof(bool), typeof(CustomWindow), new PropertyMetadata(true, OnCanMinimizePropertyChanged));
			AdditionalIconProperty = DependencyProperty.Register("AdditionalIcon", typeof(ImageSource), typeof(CustomWindow));
			TitleHeightProperty = DependencyProperty.Register("TitleHeight", typeof(double), typeof(CustomWindow), new PropertyMetadata(32.0));
			WindowResizeAreaSizeProperty = DependencyProperty.Register("WindowResizeAreaSize", typeof(double), typeof(CustomWindow), new PropertyMetadata(DefaultWindowResizeAreaSize));
		}

		public CustomWindow()
		{
			SynchronizeWithWindowState();
			StateChanged += OnStateChanged;
			_resizeShapes = new Shape[3,3];
		}

		public double WindowResizeAreaSize
		{
			get { return (double)GetValue(WindowResizeAreaSizeProperty); }
			set { SetValue(WindowResizeAreaSizeProperty, value); }
		}

		public double TitleHeight
		{
			get { return (double)GetValue(TitleHeightProperty); }
			set { SetValue(TitleHeightProperty, value); }
		}

		public ImageSource AdditionalIcon
		{
			get { return (ImageSource) GetValue(AdditionalIconProperty); }
			set { SetValue(AdditionalIconProperty, value); }
		}

		public bool CanMaximize
		{
			get { return (bool) GetValue(CanMaximizeProperty); }
			set { SetValue(CanMaximizeProperty, value); }
		}

		public bool CanMinimize
		{
			get { return (bool)GetValue(CanMinimizeProperty); }
			set { SetValue(CanMinimizeProperty, value); }
		}

		protected override void OnInitialized(EventArgs e)
		{
			SourceInitialized += OnSourceInitialized;
			base.OnInitialized(e);
		}

		private void OnSourceInitialized(object sender, EventArgs e)
		{
			_hwndSource = (HwndSource)PresentationSource.FromVisual(this);
		}

		public override void OnApplyTemplate()
		{
			base.OnApplyTemplate();
			//MinimizeButton
			if (_minimizeButton != null)
			{
				_minimizeButton.Click -= OnMinimizeButtonClick;
			}
			_minimizeButton = GetTemplateChild(MinimizeButtonPartName) as Button;
			if (_minimizeButton != null)
			{
				_minimizeButton.Click += OnMinimizeButtonClick;
			}
			//RestoreButton
			if (_restoreButton != null)
			{
				_restoreButton.Click -= OnRestoreButtonClick;
			}
			_restoreButton = GetTemplateChild(RestoreButtonPartName) as Button;
			if (_restoreButton != null)
			{
				_restoreButton.Click += OnRestoreButtonClick;
			}
			//MaximizeButton
			if (_maximizeButton != null)
			{
				_maximizeButton.Click -= OnMaximizeButtonClick;
			}
			_maximizeButton = GetTemplateChild(MaximizeButtonPartName) as Button;
			if (_maximizeButton != null)
			{
				_maximizeButton.Click += OnMaximizeButtonClick;
			}
			//CloseButton
			if (_closeButton != null)
			{
				_closeButton.Click -= OnCloseButtonClick;
			}
			_closeButton = GetTemplateChild(CloseButtonPartName) as Button;
			if (_closeButton != null)
			{
				_closeButton.Click += OnCloseButtonClick;
			}
			//HeaderBorder
			if (_headerBorder != null)
			{
				_headerBorder.MouseDown -= OnHeaderBorderPreviewMouseDown;
			}
			_headerBorder = GetTemplateChild(HeaderBorderPartName) as Border;
			if (_headerBorder != null)
			{
				_headerBorder.MouseDown += OnHeaderBorderPreviewMouseDown;
			}
			//WindowBorder
			_windowBorder = GetTemplateChild(WindowBorderPartName) as Border;
			//ResizeShapes
			InitializeResizeShape(0, 0, TopLeftResizeShapePartName);
			InitializeResizeShape(0, 1, TopCenterResizeShapePartName);
			InitializeResizeShape(0, 2, TopRightResizeShapePartName);
			InitializeResizeShape(1, 0, CenterLeftResizeShapePartName);
			InitializeResizeShape(1, 2, CenterRightResizeShapePartName);
			InitializeResizeShape(2, 0, BottomLeftResizeShapePartName);
			InitializeResizeShape(2, 1, BottomCenterResizeShapePartName);
			InitializeResizeShape(2, 2, BottomRightResizeShapePartName);

			SynchronizeWithWindowState();
			UpdateMaximizeButtonVisibility();
			UpdateMinimizeButtonVisibility();
		}

		private void InitializeResizeShape(int row, int column, string name)
		{
			Shape resizeShape = _resizeShapes[row, column];
			if (resizeShape != null)
			{
				resizeShape.PreviewMouseDown -= OnResizeShapeMouseDown;
			}
			resizeShape = GetTemplateChild(name) as Shape;
			if (resizeShape != null)
			{
				resizeShape.PreviewMouseDown += OnResizeShapeMouseDown;
			}
			_resizeShapes[row, column] = resizeShape;
		}

		[DllImport("user32.dll", CharSet = CharSet.Auto)]
		private static extern IntPtr SendMessage(IntPtr hWnd, UInt32 msg, IntPtr wParam, IntPtr lParam);

		private void ResizeWindow(ResizeDirection direction)
		{
			SendMessage(_hwndSource.Handle, 0x112, (IntPtr)(61440 + direction), IntPtr.Zero);
		}

		private void OnResizeShapeMouseDown(object sender, MouseButtonEventArgs e)
		{
			Shape shape = (Shape)sender;
			ResizeDirection resizeDirection = (ResizeDirection)shape.Tag;
			ResizeWindow(resizeDirection);
			//switch (shape.Name)
			//{
			//    case "top":
			//        Cursor = Cursors.SizeNS;
			//        ResizeWindow(ResizeDirection.Top);
			//        break;
			//    case "bottom":
			//        Cursor = Cursors.SizeNS;
			//        ResizeWindow(ResizeDirection.Bottom);
			//        break;
			//    case "left":
			//        Cursor = Cursors.SizeWE;
			//        ResizeWindow(ResizeDirection.Left);
			//        break;
			//    case "right":
			//        Cursor = Cursors.SizeWE;
			//        ResizeWindow(ResizeDirection.Right);
			//        break;
			//    case "topLeft":
			//        Cursor = Cursors.SizeNWSE;
			//        ResizeWindow(ResizeDirection.TopLeft);
			//        break;
			//    case "topRight":
			//        Cursor = Cursors.SizeNESW;
			//        ResizeWindow(ResizeDirection.TopRight);
			//        break;
			//    case "bottomLeft":
			//        Cursor = Cursors.SizeNESW;
			//        ResizeWindow(ResizeDirection.BottomLeft);
			//        break;
			//    case "bottomRight":
			//        Cursor = Cursors.SizeNWSE;
			//        ResizeWindow(ResizeDirection.BottomRight);
			//        break;
			//    default:
			//        break;
			//}
		}

		private void OnHeaderBorderPreviewMouseDown(object sender, MouseButtonEventArgs eventArgs)
		{
			if (Mouse.LeftButton == MouseButtonState.Pressed)
			{
				DragMove();
			}
			eventArgs.Handled = false;
		}

		public IntPtr GetWindowHandle()
		{
			WindowInteropHelper helper = new WindowInteropHelper(this);
			return helper.Handle;
		}

		private void OnStateChanged(object sender, EventArgs e)
		{
			SynchronizeWithWindowState();
			//if (WindowState == WindowState.Maximized)
			//{
			//    // Make sure window doesn't overlap with the taskbar.
			//    IntPtr handle = GetWindowHandle();
			//    System.Windows.Forms.Screen screen = System.Windows.Forms.Screen.FromHandle(handle);
			//    if (screen.Primary)
			//    {
			//        double left = SystemParameters.WorkArea.Left;
			//        double top = SystemParameters.WorkArea.Top;
			//        double right = SystemParameters.PrimaryScreenWidth - SystemParameters.WorkArea.Right;
			//        double bottom = SystemParameters.PrimaryScreenHeight - SystemParameters.WorkArea.Bottom;
			//        _windowBorder.Padding = new Thickness(left, top, right, bottom);
			//    }
			//}
		}

		private void SynchronizeWithWindowState()
		{
			switch (WindowState)
			{
				case WindowState.Maximized:
					if (_restoreButton != null && CanMaximize)
					{
						_restoreButton.Visibility = Visibility.Visible;
					}
					if (_maximizeButton != null && CanMaximize)
					{
						_maximizeButton.Visibility = Visibility.Collapsed;
					}
					WindowResizeAreaSize = 0;
					break;
				case WindowState.Normal:
					if (_restoreButton != null && CanMaximize)
					{
						_restoreButton.Visibility = Visibility.Collapsed;	
					}
					if (_maximizeButton != null && CanMaximize)
					{
						_maximizeButton.Visibility = Visibility.Visible;
					}
					WindowResizeAreaSize = DefaultWindowResizeAreaSize;
					break;
			}
		}

		private void OnMinimizeButtonClick(object sender, RoutedEventArgs routedEventArgs)
		{
			WindowState = WindowState.Minimized;
		}

		private void OnRestoreButtonClick(object sender, RoutedEventArgs routedEventArgs)
		{
			WindowState = WindowState.Normal;
		}

		private void OnMaximizeButtonClick(object sender, RoutedEventArgs routedEventArgs)
		{
			WindowState = WindowState.Maximized;
		}

		private void OnCloseButtonClick(object sender, RoutedEventArgs routedEventArgs)
		{
			Close();
		}

		private void UpdateMaximizeButtonVisibility()
		{
			Visibility visibility = CanMaximize ? Visibility.Visible : Visibility.Collapsed;
			if (_restoreButton != null)
			{
				_restoreButton.Visibility = visibility;
			}
			if (_maximizeButton != null)
			{
				_maximizeButton.Visibility = visibility;
			}
			SynchronizeWithWindowState();
		}

		private void UpdateMinimizeButtonVisibility()
		{
			Visibility visibility = CanMinimize ? Visibility.Visible : Visibility.Collapsed;
			if (_minimizeButton != null)
			{
				_minimizeButton.Visibility = visibility;
			}
			SynchronizeWithWindowState();
		}

		private static void OnCanMaximizePropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs args)
		{
			CustomWindow customWindow = (CustomWindow) sender;
			customWindow.UpdateMaximizeButtonVisibility();
		}

		private static void OnCanMinimizePropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs args)
		{
			CustomWindow customWindow = (CustomWindow)sender;
			customWindow.UpdateMinimizeButtonVisibility();
		}
	}
}
