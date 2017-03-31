using System;
using Rhiannon.Threading;

namespace Rhiannon.Windows.Presentation.Commands
{
	public class AsyncCommand : AsyncCommandBase
	{
		private readonly Action _execute;
		private readonly Func<bool> _canExecute;

		public AsyncCommand(Action execute, Func<bool> canExecute, ITaskFactory taskFactory)
			: base(taskFactory)
		{
			_execute = execute;
			_canExecute = canExecute;
		}

		public AsyncCommand(Action execute, ITaskFactory taskFactory)
			: this(execute, null, taskFactory)
		{

		}

        protected AsyncCommand(ITaskFactory taskFactory)
            : base(taskFactory)
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
