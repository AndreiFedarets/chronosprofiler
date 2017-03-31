using System;
using System.Windows.Input;

namespace ChronosBuildTool
{
    public class Command : ICommand
    {
        private readonly Action<object> _executeFunc;
        private readonly Func<object, bool> _canExecuteFunc;
        private bool? _canExecute;

        public Command(Action<object> execute, Func<object, bool> canExecute)
        {
            _executeFunc = execute;
            _canExecuteFunc = canExecute;
        }

        public Command(Action<object> execute)
            : this(execute, null)
        {
        }

        public bool CanExecute(object parameter)
        {
            if (_canExecute.HasValue)
            {
                return _canExecute.Value;
            }
            if (_canExecuteFunc != null)
            {
                return _canExecuteFunc(parameter);
            }
            return true;
        }

        public event EventHandler CanExecuteChanged;

        public void Execute(object parameter)
        {
            _executeFunc(parameter);
        }

        public void Disable()
        {
            _canExecute = false;
            RaiseCanExecuteChanged();
        }

        public void Enable()
        {
            _canExecute = null;
            RaiseCanExecuteChanged();
        }

        private void RaiseCanExecuteChanged()
        {
            EventHandler handler = CanExecuteChanged;
            if (handler != null)
            {
                handler(this, EventArgs.Empty);
            }
        }
    }
}
