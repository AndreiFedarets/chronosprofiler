using System;
using System.Windows;
using Rhiannon.Extensions;

namespace Rhiannon.Windows.Presentation
{
	public class WindowView : Window, IWindow, IDisposable
	{
		private IViewBase _view;

		public WindowView()
		{
			Loaded += OnLoaded;
		}

		private void OnLoaded(object sender, RoutedEventArgs routedEventArgs)
		{
			InvokeLoaded();
			Loaded -= OnLoaded;
		}

		public IViewBase View
		{
			get { return _view; }
		}

		public void Dispose()
		{
			
		}

		protected override void OnContentChanged(object oldContent, object newContent)
		{
			base.OnContentChanged(oldContent, newContent);
			_view = this.FindFirstChild<IViewBase>();
			if (_view != null)
			{
				_view.CloseRequest += ViewOnCloseRequest;
			}
		}

		private void ViewOnCloseRequest()
		{
			Close();
		}

		public event Action ViewLoaded;

		private void InvokeLoaded()
		{
			Action handler = ViewLoaded;
			if (handler != null)
			{
				handler();
			}
		}

		public T GetViewAs<T>() where T : IViewBase
		{
			return (T)View;
		}

		public void Open()
		{
			Show();
		}

		public bool? OpenDialog()
		{
			return ShowDialog();
		}

		public bool CanMaximize
		{
			get { return WindowStyle == WindowStyle.ToolWindow; }
			set { WindowStyle = value ? WindowStyle.SingleBorderWindow : WindowStyle.ToolWindow;  }
		}

		public bool CanMinimize
		{
			get { return WindowStyle == WindowStyle.ToolWindow; }
			set { WindowStyle = value ? WindowStyle.SingleBorderWindow : WindowStyle.ToolWindow; }
		}
	}
}
