using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Chronos.Accessibility.IIS;

namespace Chronos.Common.WebApplication
{
    internal class ProfilingTargetController : IProfilingTargetController
    {
        private readonly ProfilingTargetSettings _settings;
        private readonly IInternetInformationServiceAccessor _internetInformationService;
        private List<Process> _processes;

        public ProfilingTargetController(ProfilingTargetSettings settings, IInternetInformationServiceAccessor internetInformationService)
        {
            _settings = settings;
            _internetInformationService = internetInformationService;
            _processes = new List<Process>();
        }

        public bool IsActive
        {
            get
            {
                lock (_processes)
                {
                    _processes.ForEach(x => x.Refresh());
                    return _processes.Any(x => !x.HasExited);   
                }
            }
        }

        public event EventHandler<ProfilingTargetControllerEventArgs> TargetStopped;

        public void Start()
        {
            //_process = Launcher.Launch(_settings.ConsoleSession, _settings.FileFullName, _settings.Arguments, _settings.WorkingDirectory, _settings.EnvironmentVariables);
            //_process.Exited += OnProcessExited;
            //_process.EnableRaisingEvents = true;
        }

        public void Stop()
        {
            //_process.Refresh();
            //if (_process.MainWindowHandle != IntPtr.Zero)
            //{
            //    Action action = () => _process.CloseMainWindow();
            //    IAsyncResult asyncResult = action.BeginInvoke(null, null);
            //    asyncResult.AsyncWaitHandle.WaitOne(ProcessWindowClosingTimeout);
            //}
            //if (IsActive)
            //{
            //    _process.Kill();
            //}
        }

        private void OnProcessExited(object sender, EventArgs e)
        {
            Process process = (Process) sender;
            process.Exited -= OnProcessExited;
            lock (_processes)
            {
                _processes.Remove(process);
            }
            ProfilingTargetControllerEventArgs.RaiseEvent(TargetStopped, this);
        }

        internal void AttachProcess(ConfigurationSettings configurationSettings, SessionSettings sessionSettings, int processId)
        {
            Process process = Process.GetProcessById(processId);
            lock (_processes)
            {
                process.EnableRaisingEvents = true;
                process.Exited += OnProcessExited;
                _processes.Add(process);   
            }
        }
    }
}
