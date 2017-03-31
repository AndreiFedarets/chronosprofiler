using System;
using Rhiannon.Extensions;
using Rhiannon.Logging;
using Rhiannon.Resources;
using Rhiannon.Windows.Presentation.Commands;

namespace Rhiannon.Windows.Presentation
{
	public abstract class ViewModelBase : PropertyChangedNotifier, IViewModelBase
	{
		private IViewBase _view;

		public IViewBase View
		{
			get
			{
				while (_view == null)
				{
					DispatcherExtensions.DoEvents();
				}
				return _view;
			}
			private set { _view = value; }
		}

		public bool IsBusy
		{
			get { return View.IsBusy; }
			set { BeginInvoke(() => View.IsBusy = value); }
		}

		protected IResourceProvider ResourceProvider
		{
			get { return View.ResourceProvider; }
		}

		public void Initialize(IViewBase view)
		{
			View = view;
			try
			{
				InitializeInternal();
			}
			catch (Exception exception)
			{
				LoggingProvider.Log(exception, Policy.Presentation);
			}
		}

		protected virtual void InitializeInternal()
		{
			
		}

		protected void StartBusy()
		{
			IsBusy = true;
		}

		protected void EndBusy()
		{
			IsBusy = false;
		}

		public virtual void InitializeKeyboardCommands(KeyboardCommandColection keyboardCommands)
		{

		}

		public virtual void Dispose()
		{
			
		}

		protected void BeginInvoke(Action action)
		{
			View.BeginInvoke(action);
		}

        protected void Invoke(Action action)
        {
            View.Invoke(action);
        }
	}
}
