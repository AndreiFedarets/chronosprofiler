using System;
using System.Diagnostics;

namespace Chronos.Client.Win
{
    internal sealed class DaemonApplicationExtensionAdapter : IApplicationExtensionAdapter
    {
        private Daemon.IApplication _application;
        private Process _clientProcess;

        public void BeginInitialize(IChronosApplication application)
        {
            _application = (Daemon.IApplication)application;
            _clientProcess = ApplicationManager.Profiling.RunApplication(_application.Uid);
        }

        public void EndInitialize()
        {
        }

        public void BeginShutdown()
        {
        }

        public void EndShutdown()
        {
        }
    }
}
