using System;

namespace Chronos.Client.Win.Commands
{
    public class SyncCommand : SyncCommandBase
    {
        private readonly Action _execute;
        private readonly Func<bool> _canExecute;

        public SyncCommand(Action execute, Func<bool> canExecute)
        {
            _execute = execute;
            _canExecute = canExecute;
        }

        public SyncCommand(Action execute)
            : this(execute, null)
        {
            _execute = execute;
        }

        protected SyncCommand()
        {

        }

        public override sealed bool CanExecute(object parameter)
        {
            return CanExecute();
        }

        public override sealed void Execute(object parameter)
        {
            Execute();
        }

        public virtual bool CanExecute()
        {
            if (_canExecute != null)
            {
                return _canExecute();
            }
            return base.CanExecute(null);
        }

        public virtual void Execute()
        {
            if (_execute != null)
            {
                _execute();
            }
        }
    }
}
