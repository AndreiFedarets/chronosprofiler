namespace Chronos.Client.Win
{
    public sealed class ApplicationExtensionAdapter : IApplicationExtensionAdapter
    {
        private IApplicationExtensionAdapter _adapter;

        public void BeginInitialize(IChronosApplication application)
        {
            if (application is Host.IApplication)
            {
                _adapter = new HostApplicationExtensionAdapter();
            }
            else if (application is Daemon.IApplication)
            {
                _adapter = new DaemonApplicationExtensionAdapter();
            }
            else
	        {
                _adapter = new EmptyApplicationExtensionAdapter();
	        }
            _adapter.BeginInitialize(application);
        }

        public void EndInitialize()
        {
            _adapter.EndInitialize();
        }

        public void BeginShutdown()
        {
            _adapter.BeginShutdown();
        }

        public void EndShutdown()
        {
            _adapter.EndShutdown();
        }
    }
}
