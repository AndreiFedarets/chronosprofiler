using System;

namespace Chronos
{
    public abstract class RemoteBaseObject : MarshalByRefObject, IDisposable
    {
        private readonly DisposableTracker _disposableTracker;
        private readonly object _lock;

        protected RemoteBaseObject()
            : this(true)
        {
        }

        protected RemoteBaseObject(bool trackExpirationTime)
        {
            _disposableTracker = new DisposableTracker(this);
            _lock = new object();
        }

        protected object Lock
        {
            get { return _lock; }
        }

        public override object InitializeLifetimeService()
        {
            return null;
        }

        protected void VerifyDisposed()
        {
            _disposableTracker.VerifyDisposed();
        }

        public virtual void Dispose()
        {
            VerifyDisposed();
            _disposableTracker.Dispose();
        }
    }
}
