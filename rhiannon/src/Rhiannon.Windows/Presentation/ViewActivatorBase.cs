using System;
using System.Windows;
using System.Windows.Data;
using System.Windows.Interop;
using Rhiannon.Unity;
using Rhiannon.Windows.Controls;
using Rhiannon.Win32;

namespace Rhiannon.Windows.Presentation
{
	public class ViewActivatorBase<TViewInterface, TViewImplementation, TViewModelInterface, TViewModelImplementation> : IViewActivatorBase
		where TViewImplementation : TViewInterface
		where TViewModelImplementation : TViewModelInterface
		where TViewInterface : IViewBase
		where TViewModelInterface : IViewModelBase
	{
		public ViewActivatorBase(string viewName)
			: this(viewName, false)
		{
			ViewName = viewName;
		}

		public ViewActivatorBase(string viewName, bool singletonViewModel)
		{
			ViewName = viewName;
		}

		public virtual Type ViewType
		{
			get { return typeof(TViewInterface); }
		}

		public virtual IViewBase Activate(IContainer container, params object[] args)
		{
			container.RegisterType<TViewInterface, TViewImplementation>();
			container.RegisterType<TViewModelInterface, TViewModelImplementation>();
			return container.Resolve<TViewInterface>();
		}

		public string ViewName { get; private set; }

        public virtual IWindow ActivateAndWrap(IContainer container, params object[] args)
        {
            return ActivateAndWrapInternal(container, false, args);
        }

        protected IWindow ActivateAndWrapInternal(IContainer container, bool child, params object[] args)
        {
            IViewBase view = Activate(container, args);
            IWindow window = CreateWindow(child);
            window.Content = view;
            BindTitles(window, view);
            BindIcons(window, view);
            window.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            window.WindowState = WindowState.Normal;
            return window;
        }

        private IWindow CreateWindow(bool child)
		{
            WindowView window = new WindowView();
            if (child)
            {
                WindowInteropHelper helper = new WindowInteropHelper(window);
                helper.Owner = NativeMethods.GetActiveWindow();
            }
			return window;
		}

		private void BindTitles(IWindow window, IViewBase view)
		{
			Binding binding = new Binding("Title");
			binding.Source = view;
			((FrameworkElement)window).SetBinding(Window.TitleProperty, binding);
		}

		private void BindIcons(IWindow window, IViewBase view)
		{
			Binding binding = new Binding("Icon");
			binding.Source = view;
			((FrameworkElement)window).SetBinding(CustomWindow.AdditionalIconProperty, binding);
		}
    }
}
