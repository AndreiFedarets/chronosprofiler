using System;
using System.Diagnostics;
using Chronos.Win32;

namespace Chronos.Common.StandaloneApplication
{
    internal class ProfilingTargetController : IProfilingTargetController
    {
        private readonly ProfilingTargetSettings _settings;
        private const int ProcessWindowClosingTimeout = 5000;
        private Process _process;

        public ProfilingTargetController(ProfilingTargetSettings settings)
        {
            _settings = settings;
        }

        public bool IsActive
        {
            get
            {
                _process.Refresh();
                return !_process.HasExited;
            }
        }

        public event EventHandler<ProfilingTargetControllerEventArgs> TargetStopped;

        public void Start()
        {
            _process = Launcher.Launch(_settings.ConsoleSession, _settings.FileFullName, _settings.Arguments, _settings.WorkingDirectory, _settings.EnvironmentVariables);
            _process.Exited += OnProcessExited;
            _process.EnableRaisingEvents = true;
        }

        public void Stop()
        {
            _process.Refresh();
            if (_process.MainWindowHandle != IntPtr.Zero)
            {
                Action action = () => _process.CloseMainWindow();
                IAsyncResult asyncResult = action.BeginInvoke(null, null);
                asyncResult.AsyncWaitHandle.WaitOne(ProcessWindowClosingTimeout);
            }
            if (IsActive)
            {
                _process.Kill();
            }
        }

        private void OnProcessExited(object sender, EventArgs e)
        {
            _process.Exited -= OnProcessExited;
            ProfilingTargetControllerEventArgs.RaiseEvent(TargetStopped, this);
        }
    }
}
