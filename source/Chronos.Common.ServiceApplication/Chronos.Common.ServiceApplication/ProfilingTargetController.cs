using System;
using System.Diagnostics;
using Chronos.Accessibility.WS;

namespace Chronos.Common.ServiceApplication
{
    internal class ProfilingTargetController : IProfilingTargetController
    {
        private const int ServiceProcessClosingTimeout = 10000;
        private readonly ProfilingTargetSettings _settings;
        private readonly IServiceController _service;
        private Process _serviceProcess;

        public ProfilingTargetController(ProfilingTargetSettings settings, IServiceController service)
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
            _service.Stop();
            _service.SetEnvironmentVariables(_settings.EnvironmentVariables);
            _service.Start();
            _serviceProcess = _service.GetServiceProcess();
            _serviceProcess.Exited += OnProcessExited;
            _serviceProcess.EnableRaisingEvents = true;
        }

        public void Stop()
        {
            _serviceProcess.Refresh();
            Action action = () => _service.Stop();
            IAsyncResult asyncResult = action.BeginInvoke(null, null);
            asyncResult.AsyncWaitHandle.WaitOne(ServiceProcessClosingTimeout);
            if (IsActive)
            {
                _serviceProcess.Kill();
            }
        }

        private void OnProcessExited(object sender, EventArgs e)
        {
            _serviceProcess.Exited -= OnProcessExited;
            _service.RemoveEnvironmentVariables();
            ProfilingTargetControllerEventArgs.RaiseEvent(TargetStopped, this);
        }
    }
}
