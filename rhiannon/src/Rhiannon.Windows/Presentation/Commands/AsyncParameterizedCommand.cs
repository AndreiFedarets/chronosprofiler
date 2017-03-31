using System;
using Rhiannon.Threading;

namespace Rhiannon.Windows.Presentation.Commands
{
	public class AsyncCommand<T> : AsyncCommandBase
	{
		private readonly Action<T> _execute;
		private readonly Func<T, bool> _canExecute;

		public AsyncCommand(Action<T> execute, Func<T, bool> canExecute, ITaskFactory taskFactory)
			: base(taskFactory)
		{
			_execute = execute;
			_canExecute = canExecute;
		}

		public AsyncCommand(Action<T> execute, ITaskFactory taskFactory)
			: this(execute, null, taskFactory)
		{

		}

        protected AsyncCommand(ITaskFactory taskFactory)
            : base(taskFactory)
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
