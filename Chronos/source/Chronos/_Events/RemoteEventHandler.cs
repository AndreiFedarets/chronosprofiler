using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.Remoting;
using System.Threading;

namespace Chronos
{
    public sealed class RemoteEventHandler<T> : RemoteBaseObject where T : EventArgs
    {
        private readonly List<EventHandler<T>> _handlers;
        private readonly object _sender;

        public RemoteEventHandler(object sender)
        {
            _sender = sender;
            _handlers = new List<EventHandler<T>>();
        }

        public void Invoke(Func<T> eventArgument)
        {
            IList<Exception> exceptions;
            lock (Lock)
            {
                if (_handlers.Count == 0)
                {
                    return;
                }
                IList<EventHandler<T>> brokenHandlers = new List<EventHandler<T>>();
                exceptions = new List<Exception>();
                T argument = eventArgument();
                for (int i = 0; i < _handlers.Count; i++)
                {
                    EventHandler<T> handler = _handlers[i];
                    InvokeHandler(handler, argument, exceptions, brokenHandlers);
                }
                ProcessBrokenHandlers(brokenHandlers);
            }
            ProcessExceptions(exceptions);
        }

        private void InvokeAsync(Func<T> eventArgument)
        {
            IList<Exception> exceptions;
            lock (Lock)
            {
                if (_handlers.Count == 0)
                {
                    return;
                }
                IList<EventHandler<T>> brokenHandlers = new ConcurentList<EventHandler<T>>();
                exceptions = new ConcurentList<Exception>();
                T argument = eventArgument();
                WaitHandle[] waitHandles = new WaitHandle[_handlers.Count];
                for (int i = 0; i < _handlers.Count; i++)
                {
                    EventHandler<T> handler = _handlers[i];
                    Action<EventHandler<T>> invoke = a => InvokeHandler(a, argument, exceptions, brokenHandlers);
                    IAsyncResult asyncResult = invoke.BeginInvoke(handler, null, null);
                    waitHandles[i] = asyncResult.AsyncWaitHandle;
                }
                WaitHandle.WaitAll(waitHandles);
                ProcessBrokenHandlers(brokenHandlers);
            }
            ProcessExceptions(exceptions);
        }

        private void ProcessBrokenHandlers(IList<EventHandler<T>> handlers)
        {
            if (handlers != null && handlers.Count > 0)
            {
                foreach (EventHandler<T> action in handlers)
                {
                    _handlers.Remove(action);
                }
            }
        }
        
        private void ProcessExceptions(IList<Exception> exceptions)
        {
            if (exceptions != null && exceptions.Count > 0)
            {
                AggregateException exception = new AggregateException(exceptions);
                //throw exception;
                LoggingProvider.Current.Log(TraceEventType.Information, exception);
            }
        }

        private void InvokeHandler(EventHandler<T> action, T argument, IList<Exception> exceptions, IList<EventHandler<T>> brokenActions)
        {
            try
            {
                action.Invoke(_sender, argument);
            }
            catch (RemotingException)
            {
                brokenActions.Add(action);
            }
            catch (Exception exception)
            {
                exceptions.Add(exception);
            }
        }

        public void Add(EventHandler<T> action)
        {
            lock (Lock)
            {
                _handlers.Add(action);
            }
        }

        public void Remove(EventHandler<T> action)
        {
            lock (Lock)
            {
                _handlers.Remove(action);
            }
        }

        public override void Dispose()
        {
            lock (Lock)
            {
                _handlers.Clear();
                base.Dispose();
            }
        }
    }
}
