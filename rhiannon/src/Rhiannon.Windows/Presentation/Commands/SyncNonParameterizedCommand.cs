using System;

namespace Rhiannon.Windows.Presentation.Commands
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

		public sealed override bool CanExecute(object parameter)
		{
		    return CanExecute();
		}

        public sealed override void Execute(object parameter)
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
