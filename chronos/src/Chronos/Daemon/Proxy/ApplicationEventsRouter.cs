using System;

namespace Chronos.Daemon.Proxy
{
	internal class ApplicationEventsRouter : MarshalByRefObject, IDisposable
	{
		private readonly IDaemonApplication _application;
		private readonly Action _stateChanged;

		public ApplicationEventsRouter(IDaemonApplication application, Action stateChanged)
		{
			_application = application;
			_stateChanged = stateChanged;
			_application.StateChanged += OnStateChanged;
		}

		public void OnStateChanged()
		{
			_stateChanged.BeginInvoke(null, null);
		}

		public void Dispose()
		{
			_application.StateChanged -= OnStateChanged;
		}

		public override object InitializeLifetimeService()
		{
			return null;
		}
	}
}
