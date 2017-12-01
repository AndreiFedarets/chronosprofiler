namespace Chronos.Client.Win
{
    internal sealed class DaemonApplicationExtensionAdapter : IApplicationExtensionAdapter
    {
        private Daemon.IApplication _application;

        public void BeginInitialize(IChronosApplication application)
        {
            _application = (Daemon.IApplication)application;
        }

        public void BeginShutdown()
        {
        }

        public void EndInitialize()
        {
        }

        public void EndShutdown()
        {
        }
    }
}
