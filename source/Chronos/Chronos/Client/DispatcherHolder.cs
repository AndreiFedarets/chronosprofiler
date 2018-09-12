using System;

namespace Chronos.Client
{
    public static class DispatcherHolder
    {
        private static readonly object Lock;
        private static IDispatcher _dispatcher;

        static DispatcherHolder()
        {
            Lock = new object();
        }

        private static IDispatcher CurrentDispatcher
        {
            get
            {
                if (_dispatcher == null)
                {
                    lock (Lock)
                    {
                        if (_dispatcher == null)
                        {
                            _dispatcher = new DefaultDispatcher();
                        }
                    }
                }
                return _dispatcher;
            }
            set
            {
                lock (Lock)
                {
                    _dispatcher = value;
                }
            }
        }

        public static void SetDispatcher(IDispatcher dispatcher)
        {
            CurrentDispatcher = dispatcher;
        }

        public static void Invoke(Action action)
        {
            CurrentDispatcher.Invoke(action);
            //if (_dispatcher.CheckAccess())
            //{
            //    action();
            //}
            //else
            //{
            //    _dispatcher.Invoke(action);
            //}
        }

        public static void BeginInvoke(Action action)
        {
            CurrentDispatcher.BeginInvoke(action);
            //if (_dispatcher.CheckAccess())
            //{
            //    ThreadPool.QueueUserWorkItem(s => Invoke(action));
            //}
            //else
            //{
            //    if (Thread.CurrentThread.GetApartmentState() == ApartmentState.MTA)
            //    {
            //        ThreadPool.QueueUserWorkItem(s => Invoke(action));
            //    }
            //    else
            //    {
            //        _dispatcher.BeginInvoke(action);
            //    }
            //}
        }

        public static void Initialize(IDispatcher dispatcher)
        {
            _dispatcher = dispatcher;
        }
    }
}
