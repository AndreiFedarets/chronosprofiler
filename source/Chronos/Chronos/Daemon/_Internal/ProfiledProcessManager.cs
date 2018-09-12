using System;
using System.Diagnostics;

namespace Chronos.Daemon
{
    internal sealed class ProfiledProcessManager : BaseObject
    {
        private readonly ProcessInformation _processInformation;
        private Process _process;

        public ProfiledProcessManager(int profiledProcessId)
        {
            _process = Process.GetProcessById(profiledProcessId);
            _process.Exited += OnProfiledProcessExited;
            _process.EnableRaisingEvents = true;
            _processInformation = new ProcessInformation(_process.Id, _process.ProcessName, new byte[0], _process.StartTime);
        }

        public event EventHandler Exited;

        public ProcessInformation GetProcessInformation()
        {
            return _processInformation;
        }

        public void Close()
        {
            if (_process != null && !_process.HasExited)
            {
                try
                {
                    if (_process.MainWindowHandle != IntPtr.Zero)
                    {
                        Action action = () => _process.CloseMainWindow();
                        IAsyncResult asyncResult = action.BeginInvoke(null, null);
                        asyncResult.AsyncWaitHandle.WaitOne(5000);
                    }
                }
                catch (Exception exception)
                {
                    LoggingProvider.Current.Log(TraceEventType.Information, exception);
                }
                if (!_process.HasExited)
                {
                    _process.Kill();
                }
            }
        }

        private void OnProfiledProcessExited(object sender, EventArgs e)
        {
            lock (Lock)
            {
                if (_process != null)
                {
                    Exited.SafeInvoke(this, EventArgs.Empty);
                    _process.Exited -= OnProfiledProcessExited;
                    _process = null;
                }
            }
        }

        public override void Dispose()
        {
            lock (Lock)
            {
                Close();
                base.Dispose();   
            }
        }
    }
}
