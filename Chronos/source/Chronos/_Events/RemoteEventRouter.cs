using System;
using System.ComponentModel;
using System.Reflection;

namespace Chronos
{
    public sealed class RemoteEventRouter<T> : RemoteBaseObject where T : EventArgs
    {
        private Delegate _externalHandler;
        private Delegate _internalHandler;
        private object _source;
        private object _sender;
        private EventInfo _eventInfo;
        private Func<T, T> _argsConverter;
        private Action<T> _afterPublishing;
        private volatile bool _subscribed;

        public RemoteEventRouter(object source, string eventName, object sender = null, Func<T, T> argsConverter = null, Action<T> afterPublishing = null)
        {
            _source = source;
            _sender = sender;
            _afterPublishing = afterPublishing;
            _argsConverter = argsConverter;
            _internalHandler = (EventHandler<T>)OnRemoteEvent;
            Type sourceType = _source.GetType();
            _eventInfo = sourceType.GetEvent(eventName);
        }

        public event EventHandler<T> Event
        {
            add
            {
                VerifyDisposed();
                Subscribe();
                _externalHandler = Delegate.Combine(_externalHandler, value);
            }
            remove
            {
                VerifyDisposed();
                _externalHandler = Delegate.Remove(_externalHandler, value);
            }
        }

        public void Subscribe()
        {
            lock (Lock)
            {
                VerifyDisposed();
                if (!_subscribed)
                {
                    _eventInfo.AddEventHandler(_source, _internalHandler);
                    _subscribed = true;
                }
            }
        }

        public void Unsubscribe()
        {
            lock (Lock)
            {
                VerifyDisposed();
                if (_subscribed)
                {
                    _eventInfo.RemoveEventHandler(_source, _internalHandler);
                }
            }
        }

        public override void Dispose()
        {
            Unsubscribe();
            _internalHandler = null;
            _source = null;
            _sender = null;
            _eventInfo = null;
            _argsConverter = null;
            _afterPublishing = null;
            base.Dispose();
        }

        [Browsable(false)]
        public void OnRemoteEvent(object sender, T eventArgs)
        {
            EventHandler<T> handler = (EventHandler<T>)_externalHandler;
            if (handler != null)
            {
                if (_sender != null)
                {
                    sender = _sender;
                }
                if (_argsConverter != null)
                {
                    eventArgs = _argsConverter(eventArgs);
                }
                handler(sender, eventArgs);
                if (_afterPublishing != null)
                {
                    _afterPublishing(eventArgs);
                }
            }
        }
    }
}
