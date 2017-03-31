using System;

namespace Rhiannon.Windows.Presentation.Commands
{
	public class SyncCommand<T> : SyncCommandBase
	{
		private readonly Action<T> _execute;
		private readonly Func<T, bool> _canExecute;

		public SyncCommand(Action<T> execute, Func<T, bool> canExecute)
		{
			_execute = execute;
			_canExecute = canExecute;
		}

		public SyncCommand(Action<T> execute)
			: this(execute, null)
		{
			_execute = execute;
		}

        protected SyncCommand()
        {
            
        }

        public sealed override bool CanExecute(object parameter)
		{
            return CanExecute(CastParameter<T>(parameter));
		}

        public sealed override void Execute(object parameter)
		{
            Execute(CastParameter<T>(parameter));
		}

        public virtual bool CanExecute(T parameter)
        {
            if (_canExecute != null)
            {
                return _canExecute(parameter);
            }
            return base.CanExecute(parameter);
        }

        public virtual void Execute(T parameter)
        {
            if (_execute != null)
            {
                _execute(parameter);
            }
        }
	}
}
