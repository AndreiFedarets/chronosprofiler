using System;
using System.Windows.Threading;

namespace Chronos.Client.Win
{
    public abstract class PropertyChangedBaseEx : Caliburn.Micro.PropertyChangedBase, IDisposable
    {
        private readonly object _lock;
        private readonly DisposableTracker _disposableTracker;

        protected PropertyChangedBaseEx()
        {
            _disposableTracker = new DisposableTracker(this);
            _lock = new object();
            Dispatcher = Dispatcher.CurrentDispatcher;
        }

        protected object Lock
        {
            get { return _lock; }
        }

        protected Dispatcher Dispatcher { get; private set; }

        protected object Invoke(Action action)
        {
            if (Dispatcher.CheckAccess())
            {
                action();
                return null;
            }
            return Dispatcher.Invoke(action);
        }

        protected T Invoke<T>(Func<T> func)
        {
            if (Dispatcher.CheckAccess())
            {
                return func();
            }
            return (T)Dispatcher.Invoke(func);
        }

        protected void VerifyDisposed()
        {
            _disposableTracker.VerifyDisposed();
        }

        public virtual void Dispose()
        {
            _disposableTracker.Dispose();
        }
    }
}
