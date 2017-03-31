using System;

namespace Chronos.Client.Win.Commands
{
    public abstract class ExtendedCommandBase : IExtendedCommand
    {
        public virtual string Id
        {
            get { return GetType().Name; }
        }

        public event EventHandler CanExecuteChanged;

        public virtual bool CanExecute(object parameter)
        {
            return true;
        }

        public virtual void Execute(object parameter)
        {

        }

        internal virtual void ExecuteInternal(object parameter)
        {

        }

        protected void OnCanExecuteChanged(EventArgs e)
        {
            EventHandler handler = CanExecuteChanged;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        protected T CastParameter<T>(object parameter)
        {
            if (parameter == null)
            {
                return default(T);
            }
            return (T) parameter;
        }

        public virtual void Dispose()
        {

        }
    }
}
