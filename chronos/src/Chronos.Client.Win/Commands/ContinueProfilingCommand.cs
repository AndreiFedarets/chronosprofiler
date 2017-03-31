using Chronos.Core;
using Chronos.Daemon;
using Rhiannon.Windows.Presentation.Commands;
using System;

namespace Chronos.Client.Win.Commands
{
    public class ContinueProfilingCommand : SyncCommand
    {
        private readonly IDaemonApplication _application;

        public ContinueProfilingCommand(IDaemonApplication application)
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
            return _application.State == SessionState.Paused;
        }

        public override void Execute()
        {
            _application.ContinueProfiling();
        }
    }
}
