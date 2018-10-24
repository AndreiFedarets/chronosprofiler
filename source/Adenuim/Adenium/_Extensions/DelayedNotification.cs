using System;
using System.Windows.Threading;

namespace Adenium
{
    public sealed class DelayedNotification<T> where T : class
    {
        private const int DefaultDelay = 750;
        private readonly object _lock;
        private readonly Action<T> _callback;
        private readonly Dispatcher _dispatcher;
        private readonly System.Timers.Timer _timer;
        private volatile T _currentValue;

        public DelayedNotification(Action<T> callback, bool syncronized, int delay)
        {
            _lock = new object();
            _timer = new System.Timers.Timer() { Interval = delay, AutoReset = false };
            _timer.Elapsed += OnTimerElapsed;
            _callback = callback;
            if (syncronized)
            {
                _dispatcher = Dispatcher.CurrentDispatcher;
            }
        }

        public DelayedNotification(Action<T> callback, bool syncronized)
            : this(callback, syncronized, DefaultDelay)
        {
        }

        private void OnTimerElapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            lock (_lock)
            {
                T value = _currentValue;
                Action callback = () => _callback(value);
                if (_dispatcher == null)
                {
                    callback.BeginInvoke(null, null);
                }
                else
                {
                    _dispatcher.BeginInvoke(callback);
                }
            }
        }

        public void SetValue(T value)
        {
            lock (_lock)
            {
                _timer.Stop();
                _currentValue = value;
                _timer.Start();
            }
        }

        public T GetValue()
        {
            return _currentValue;
        }
    }
}
