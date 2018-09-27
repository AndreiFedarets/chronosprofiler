using System;
using System.Threading;
using System.Windows.Threading;

namespace Chronos.Client.Win
{
    internal sealed class WindowsDispatcher : IDispatcher
    {
        private readonly Dispatcher _dispatcher;

        public WindowsDispatcher()
            : this(Dispatcher.CurrentDispatcher)
        {
        }

        public WindowsDispatcher(Dispatcher dispatcher)
        {
            _dispatcher = dispatcher;
        }

        public void BeginInvoke(Action action)
        {
            _dispatcher.BeginInvoke(action);
        }

        public void Invoke(Action action)
        {
            if (Equals(_dispatcher.Thread, Thread.CurrentThread))
            {
                action();
            }
            else
            {
                _dispatcher.Invoke(action);
            }
        }

        public T Invoke<T>(Func<T> action)
        {
            if (Equals(_dispatcher.Thread, Thread.CurrentThread))
            {
                return action();
            }
            return (T)_dispatcher.Invoke(action);
        }
    }
}
