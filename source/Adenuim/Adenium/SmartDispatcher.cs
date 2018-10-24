using System;
using System.Threading;
using System.Windows.Threading;

namespace Adenium
{
    public sealed class SmartDispatcher : IDispatcher
    {
        private static readonly ThreadLocal<IDispatcher> CurrentDispatcher;
        private static readonly IDispatcher MainDispatcher;
        private readonly Dispatcher _dispatcher;

        static SmartDispatcher()
        {
            MainDispatcher = new SmartDispatcher();
            CurrentDispatcher = new ThreadLocal<IDispatcher>(() => new SmartDispatcher());
        }

        public SmartDispatcher()
            : this(Dispatcher.CurrentDispatcher)
        {
        }

        public SmartDispatcher(Dispatcher dispatcher)
        {
            _dispatcher = dispatcher;
        }

        public static IDispatcher Current
        {
            get { return CurrentDispatcher.Value; }
        }

        public static IDispatcher Main
        {
            get { return MainDispatcher; }
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

        public static void Initialize()
        {
            //This method is empty because it's just trigger for static constructor
        }
    }
}
