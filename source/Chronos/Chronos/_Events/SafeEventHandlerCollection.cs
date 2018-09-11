using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Chronos
{
    public class SafeEventHandlerCollection<T> : IDisposable where  T : EventArgs
    {
        private readonly List<EventHandler<T>> _handlers;

        public SafeEventHandlerCollection()
        {
            _handlers = new List<EventHandler<T>>();
        }

        public void Add(EventHandler<T> handler)
        {
            lock (_handlers)
            {
                _handlers.Add(handler);
            }
        }

        public void Remove(EventHandler<T> handler)
        {
            lock (_handlers)
            {
                _handlers.Remove(handler);
            }
        }

        public void Raise(object sender, Func<T> getEventArgs)
        {
            AggregateException exception = null;
            lock (_handlers)
            {
                if (_handlers.Count > 0)
                {
                    T eventArgs = getEventArgs();
                    exception = RaiseInternal(sender, eventArgs);
                }
            }
            if (exception != null)
            {
                //throw exception;
                LoggingProvider.Current.Log(TraceEventType.Information, exception);
            }
        }

        public void Raise(object sender, T eventArgs)
        {
            AggregateException exception = null;
            lock (_handlers)
            {
                if (_handlers.Count > 0)
                {
                    exception = RaiseInternal(sender, eventArgs);
                }
            }
            if (exception != null)
            {
                throw exception;
            }
        }

        private AggregateException RaiseInternal(object sender, T eventArgs)
        {
            List<Exception> exceptions = null;
            //We cannot copy handlers to speed-up unlock, because there may be situation when copy was created and
            //right after that subscriber did unsubscribe and cleanup - it will receive notification and may break.
            foreach (EventHandler<T> handler in _handlers)
            {
                try
                {
                    handler(sender, eventArgs);
                }
                catch (Exception exception)
                {
                    if (exceptions == null)
                    {
                        exceptions = new List<Exception>();
                    }
                    exceptions.Add(exception);
                }
            }
            AggregateException aggregateException = null;
            if (exceptions != null)
            {
                aggregateException = new AggregateException(exceptions);
            }
            return aggregateException;
        }

        public void Dispose()
        {
            _handlers.Clear();
        }
    }
}
