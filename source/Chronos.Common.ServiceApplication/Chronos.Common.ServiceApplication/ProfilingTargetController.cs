using System;
using System.Diagnostics;
using Chronos.Accessibility.WS;

namespace Chronos.Common.ServiceApplication
{
    internal class ProfilingTargetController : IProfilingTargetController
    {
        private const int ServiceProcessClosingTimeout = 10000;
        private readonly ProfilingTargetSettings _settings;
        private readonly IWindowsService _service;
        private bool _isServiceInitialyRunning;
        private Process _serviceProcess;

        public ProfilingTargetController(ProfilingTargetSettings settings, IWindowsService service)
        {
            _settings = settings;
            _service = service;
        }

        public bool IsActive
        {
            get
            {
                if (_serviceProcess == null)
                {
                    return false;
                }
                _serviceProcess.Refresh();
                return !_serviceProcess.HasExited;
            }
        }

        public event EventHandler<ProfilingTargetControllerEventArgs> TargetStopped;

        public void Start()
        {
            _isServiceInitialyRunning = _service.IsRunning;
            _service.Stop();
            _service.SetEnvironmentVariables(_settings.EnvironmentVariables);
            _service.Start();
            //remove Environment variables to prevent profiling after service started next time
            _service.RemoveEnvironmentVariables();
            _serviceProcess = _service.GetServiceProcess();
            if (_serviceProcess == null || _serviceProcess.HasExited)
            {
                //TODO: log
                return;
            }
            _serviceProcess.Exited += OnProcessExited;
            _serviceProcess.EnableRaisingEvents = true;   
        }

        public void Stop()
        {
            if (_serviceProcess != null)
            {
                _serviceProcess.Refresh();
                if (!_serviceProcess.HasExited)
                {
                    Action action = () => _service.Stop();
                    IAsyncResult asyncResult = action.BeginInvoke(null, null);
                    asyncResult.AsyncWaitHandle.WaitOne(ServiceProcessClosingTimeout);
                    if (IsActive)
                    {
                        _serviceProcess.Kill();
                    }   
                }
            }
            if (_isServiceInitialyRunning)
            {
                _service.Start();
            }
        }

        private void OnProcessExited(object sender, EventArgs e)
        {
            _serviceProcess.Exited -= OnProcessExited;
            ProfilingTargetControllerEventArgs.RaiseEvent(TargetStopped, this);
            _serviceProcess = null;
        }
    }
}
