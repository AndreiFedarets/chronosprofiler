using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;

namespace Chronos
{
    public static class EventExtensions
    {
        public static void SafeInvoke(this Action action)
        {
            Action handler = action;
            if (handler != null)
            {
                handler();
            }
        }

        public static void SafeInvoke<T>(this Action<T> action, T value)
        {
            Action<T> handler = action;
            if (handler != null)
            {
                handler(value);
            }
        }

        public static void SafeInvoke<T>(this EventHandler<T> @event, object sender, T eventArgs) where T : EventArgs
        {
            EventHandler<T> handler = @event;
            if (handler != null)
            {
                handler(sender, eventArgs);
            }
        }

        public static void SafeInvoke(this EventHandler @event, object sender, EventArgs eventArgs)
        {
            EventHandler handler = @event;
            if (handler != null)
            {
                handler(sender, eventArgs);
            }
        }

        public static void SafeInvoke(this NotifyCollectionChangedEventHandler @event, object sender, NotifyCollectionChangedEventArgs eventArgs)
        {
            NotifyCollectionChangedEventHandler handler = @event;
            if (handler != null)
            {
                handler(sender, eventArgs);
            }
        }

        public static void SafeInvoke(this PropertyChangedEventHandler @event, object sender, PropertyChangedEventArgs eventArgs)
        {
            PropertyChangedEventHandler handler = @event;
            if (handler != null)
            {
                handler(sender, eventArgs);
            }
        }

        public static void RaiseEventSafeAndSilent<T>(EventHandler<T> commonHandler, object sender, Func<T> eventArgsFunc) where T : EventArgs
        {
            if (commonHandler != null)
            {
                RaiseEventSilent(commonHandler, sender, eventArgsFunc());
            }
        }

        public static void RaiseEventSilent<T>(EventHandler<T> commonHandler, object sender, T eventArgs) where T : EventArgs
        {
            Delegate[] handlers = commonHandler.GetInvocationList();
            List<Exception> exceptions = null;
            foreach (Delegate handler in handlers)
            {
                try
                {
                    handler.DynamicInvoke(sender, eventArgs);
                }
                catch (Exception exception)
                {
                    exceptions = exceptions ?? new List<Exception>();
                    exceptions.Add(exception);
                }
            }
            if (exceptions != null)
            {
                AggregateException exception = new AggregateException(exceptions);
                //throw exception;
                LoggingProvider.Current.Log(TraceEventType.Information, exception);
            }
        }
    }
}
