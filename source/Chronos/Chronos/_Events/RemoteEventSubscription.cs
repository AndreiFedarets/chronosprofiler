using System;
using System.Reflection;

namespace Chronos
{
    public class RemoteEventSubscription<T> : RemoteBaseObject where T : EventArgs
    {
        private readonly EventHandler<T> _handler;
        private readonly Delegate _internalHandler;
        private readonly Delegate _externalHandler;
        private readonly object _remoteObject;
        private EventInfo _eventInfo;
        private bool _subscribed;

        public RemoteEventSubscription(object remoteObject, string eventName, EventHandler<T> handler)
        {
            _handler = handler;
            _subscribed = false;
            _remoteObject = remoteObject;
            _internalHandler = (EventHandler<T>)OnRemoteEvent;
            _externalHandler = handler;
            Type sourceType = _remoteObject.GetType();
            _eventInfo = sourceType.GetEvent(eventName);
            if (_eventInfo == null)
            {
                throw new TempException(string.Format("Type {0} does not contain event with name {1}", sourceType, eventName));
            }
        }

        public void OnEvent(object sender, T eventArgs)
        {
            _handler(sender, eventArgs);
        }

        public void Subscribe()
        {
            lock (Lock)
            {
                VerifyDisposed();
                if (!_subscribed)
                {
                    _eventInfo.AddEventHandler(_remoteObject, _internalHandler);
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
                    _eventInfo.RemoveEventHandler(_remoteObject, _internalHandler);
                }
            }

        }

        public override void Dispose()
        {
            Unsubscribe();
            base.Dispose();
        }

        public void OnRemoteEvent(object sender, T eventArgs)
        {
            EventHandler<T> handler = (EventHandler<T>)_externalHandler;
            if (handler != null)
            {
                handler(sender, eventArgs);
            }
        }
    }
}
