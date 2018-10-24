using System;
using System.Reflection;
using System.Windows.Controls;
using System.Windows.Input;

namespace Adenium
{
    public abstract class ControlEventCommandBehavior
    {
        private ICommand _command;
        private object _commandParameter;
        private WeakReference _targetObject;
        private readonly EventHandler _commandCanExecuteChangedHandler;

        protected ControlEventCommandBehavior(Control targetObject)
        {
            TargetObject = targetObject;
            _commandCanExecuteChangedHandler = CommandCanExecuteChanged;
            SubscribeEvent();
        }

        public abstract string EventName { get; }

        protected abstract string HandlerMethodName { get; }

        public ICommand Command
        {
            get { return _command; }
            set
            {
                if (_command != null)
                {
                    _command.CanExecuteChanged -= _commandCanExecuteChangedHandler;
                }
                _command = value;
                if (_command != null)
                {
                    _command.CanExecuteChanged += _commandCanExecuteChangedHandler;
                    UpdateEnabledState();
                }
            }
        }

        public object CommandParameter
        {
            get { return _commandParameter; }
            set
            {
                if (_commandParameter != value)
                {
                    _commandParameter = value;
                    UpdateEnabledState();
                }
            }
        }

        protected Control TargetObject
        {
            get { return _targetObject.Target as Control; }
            private set { _targetObject = new WeakReference(value); }
        }

        private void SubscribeEvent()
        {
            EventInfo eventInfo = TargetObject.GetType().GetEvent(EventName);
            Type eventHandlerType = eventInfo.EventHandlerType;
            MethodInfo methodInfo = GetType().GetMethod(HandlerMethodName);
            Delegate handler = Delegate.CreateDelegate(eventHandlerType, this, methodInfo);
            eventInfo.AddEventHandler(TargetObject, handler);
        }

        protected virtual void UpdateEnabledState()
        {
            UpdateEnabledStateInternal();
        }

        private void UpdateEnabledStateInternal()
        {
            if (TargetObject == null)
            {
                Command = null;
                CommandParameter = null;
                return;
            }
            if (Command != null)
            {
                Control control = TargetObject;
                control.IsEnabled = Command.CanExecute(CommandParameter);
            }
        }

        private void CommandCanExecuteChanged(object sender, EventArgs e)
        {
            UpdateEnabledState();
        }

        protected virtual void ExecuteCommand()
        {
            ExtendedCommandBase extendedCommandBase = Command as ExtendedCommandBase;
            if (extendedCommandBase != null)
            {
                extendedCommandBase.ExecuteInternal(CommandParameter);
            }
            else if (Command != null)
            {
                Command.Execute(CommandParameter);
            }
        }
    }
}
