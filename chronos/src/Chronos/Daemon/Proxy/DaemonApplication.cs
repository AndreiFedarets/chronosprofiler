using System;
using System.Diagnostics;
using Chronos.Core;
using Rhiannon.Extensions;

namespace Chronos.Daemon.Proxy
{
    internal class DaemonApplication : IDaemonApplication, IDisposable
    {
        private readonly IDaemonApplication _application;
        private readonly ApplicationEventsRouter _eventsRouter;
        private IProcessShadow _processShadow;
        private readonly Process _process;

        public DaemonApplication(IDaemonApplication application)
        {
            _application = application;
            _eventsRouter = new ApplicationEventsRouter(application, OnStateChanged);
            _process = Process.GetProcessById(application.ProcessId);
            _process.EnableRaisingEvents = true;
            _process.Exited += OnDaemonProcessExited;
        }

        private void OnDaemonProcessExited(object sender, EventArgs e)
        {
            _process.Exited -= OnDaemonProcessExited;
            Exited.SafeInvoke();
        }

        public SessionState State
        {
            get { return _application.State; }
        }

        public event Action StateChanged;

        public IProcessShadow ProcessShadow
        {
            get
            {
                if (_processShadow == null)
                {
                    IProcessShadow processShadow = _application.ProcessShadow;
                    _processShadow = new ProcessShadow(processShadow);
                }
                return _processShadow;
            }
        }

        public ProcessInfo GetProcessInfo()
        {
            return _application.GetProcessInfo();
        }

        public void Close(bool save)
        {
            _application.Close(save);
        }

        public void StartProfiling(int processId, uint syncTime)
        {
            _application.StartProfiling(processId, syncTime);
        }

        public void StartDecoding()
        {
            _application.StartDecoding();
        }

        private void OnStateChanged()
        {
            StateChanged.SafeInvoke();
        }

        public void Dispose()
        {
            _eventsRouter.Dispose();
        }

        public event Action Exited;

        public string Ping(string message)
        {
            return _application.Ping(message);
        }

        public int ProcessId
        {
            get { return _application.ProcessId; }
        }

        public void PauseProfiling()
        {
            _application.PauseProfiling();
        }

        public void ContinueProfiling()
        {
            _application.ContinueProfiling();
        }

        public void StopProfiling()
        {
            _application.StopProfiling();
        }
    }
}
