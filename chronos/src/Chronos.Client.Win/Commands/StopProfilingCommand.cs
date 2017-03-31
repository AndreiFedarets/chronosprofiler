using System;
using Chronos.Core;
using Chronos.Daemon;
using Rhiannon.Threading;
using Rhiannon.Windows.Presentation.Commands;

namespace Chronos.Client.Win.Commands
{
    public class StopProfilingCommand : AsyncCommand
    {
        private readonly IDaemonApplication _application;

        public StopProfilingCommand(IDaemonApplication application, ITaskFactory taskFactory)
            : base(taskFactory)
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
            return _application.State == SessionState.Profiling || _application.State == SessionState.Paused;
        }

        public override void Execute()
        {
            _application.StopProfiling();
        }

        public override void Dispose()
        {
            _application.StateChanged -= OnStateChanged;
            base.Dispose();
        }
    }
}
