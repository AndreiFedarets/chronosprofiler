using System;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Microsoft.Practices.Unity;
using Rhiannon.Extensions;
using Rhiannon.Resources;
using Rhiannon.Windows.Presentation.ApplyResourceBehavior;
using Rhiannon.Windows.Presentation.Commands;

namespace Rhiannon.Windows.Presentation
{
	public class UserControlView : UserControl, IViewBase
	{
		public static readonly DependencyProperty IsBusyProperty;
		public static readonly DependencyProperty TitleProperty;
		public static readonly DependencyProperty IconProperty;

		private bool _resourcesApplied;

		static UserControlView()
		{
			DefaultStyleKeyProperty.OverrideMetadata(typeof(UserControlView), new FrameworkPropertyMetadata(typeof(UserControlView)));
			IsBusyProperty = DependencyProperty.Register("IsBusy", typeof(bool), typeof(UserControlView));
			TitleProperty = DependencyProperty.Register("Title", typeof(string), typeof(UserControlView), new PropertyMetadata(string.Empty));
			IconProperty = DependencyProperty.Register("Icon", typeof (ImageSource), typeof (UserControlView));
		}

		public UserControlView(IViewModelBase viewModel)
		{
			Loaded += OnLoaded;
			Unloaded += OnUnloaded;
			ViewModel = viewModel;
			viewModel.Initialize(this);
			KeyboardCommands = new KeyboardCommandColection(this);
			viewModel.InitializeKeyboardCommands(KeyboardCommands);
			base.LostFocus += OnLostFocus;
			HorizontalAlignment = HorizontalAlignment.Stretch;
			VerticalAlignment = VerticalAlignment.Stretch;
		}

		[Dependency]
		public IResourceProvider ResourceProvider { get; set; }

		public string Title
		{
			get { return (string)GetValue(TitleProperty); }
			set { SetValue(TitleProperty, value); }
		}

		public ImageSource Icon
		{
			get { return (ImageSource) GetValue(IconProperty); }
			set { SetValue(IconProperty, value); }
		}

		public bool IsBusy
		{
			get { return (bool)GetValue(IsBusyProperty); }
			set
			{
				Delegate @delegate = new ThreadStart(()=> SetValue(IsBusyProperty, value));
				Dispatcher.Invoke(@delegate);
			}
		}

		public KeyboardCommandColection KeyboardCommands { get; private set; }

		public IViewModelBase ViewModel
		{
			get { return (IViewModelBase) DataContext; }
			private set { DataContext = value; }
		}

		public event Action CloseRequest;

		public event Action ViewLoaded;

		public void Close()
		{
			CloseRequest.SafeInvoke();
		}

		public void Invoke(Action action)
		{
            if (Dispatcher.Thread.ManagedThreadId == Thread.CurrentThread.ManagedThreadId)
            {
                action();
            }
            else
            {
                ThreadStart @delegate = new ThreadStart(action);
                Dispatcher.Invoke(@delegate);
            }
		}

		public void BeginInvoke(Action action)
		{
			ThreadStart @delegate = new ThreadStart(action);
			Dispatcher.BeginInvoke(@delegate);
		}

		private void InvokeLoaded()
		{
			Action handler = ViewLoaded;
			if (handler != null)
			{
				handler();
			}
		}

		protected override void OnInitialized(EventArgs e)
		{
			base.OnInitialized(e);
			InvokeLoaded();
		}

		public new event Action<IViewBase> LostFocus;

		private void InvokeLostFocus()
		{
			Action<IViewBase> handler = LostFocus;
			if (handler != null)
			{
				handler(this);
			}
		}

		private void OnLostFocus(object sender, RoutedEventArgs e)
		{
			InvokeLostFocus();
		}

		private void OnLoaded(object sender, RoutedEventArgs e)
		{
			ApplyResources();
		}

		private void OnUnloaded(object sender, RoutedEventArgs e)
		{
			Loaded -= OnLoaded;
			Unloaded -= OnUnloaded;
			base.LostFocus -= OnLostFocus;
		}

		public void ApplyResources()
		{
			if (_resourcesApplied)
			{
				return;
			}
			ResourceBehaviorCollection.Resolve<UserControlView>().ApplyRecursively(this, ResourceProvider);
			_resourcesApplied = true;
		}

		public void Dispose()
		{
			if (ViewModel != null)
			{
				ViewModel.Dispose(); 
			}
		}
	}
}
