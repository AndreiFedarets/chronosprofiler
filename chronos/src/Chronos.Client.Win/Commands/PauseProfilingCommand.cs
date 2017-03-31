using Chronos.Core;
using Chronos.Daemon;
using Rhiannon.Windows.Presentation.Commands;
using System;

namespace Chronos.Client.Win.Commands
{
    public class PauseProfilingCommand : SyncCommand
    {
        private readonly IDaemonApplication _application;

        public PauseProfilingCommand(IDaemonApplication application)
        {
            _application = application;
            _application.StateChanged += OnStateChanged;
        }

        private void OnStateChanged()
        {
            OnCanExecuteChanged(EventArgs.Empty);
        }

        public override bool CanExecute()
        {
            return _application.State == SessionState.Profiling;
        }

        public override void Execute()
        {
            _application.PauseProfiling();
        }

        public override void Dispose()
        {
            _application.StateChanged -= OnStateChanged;
            base.Dispose();
        }
    }
}
