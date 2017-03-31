using System;

namespace Chronos
{
    public abstract class BaseObject : IDisposable
    {
        private readonly DisposableTracker _disposableTracker;
        private readonly object _lock;

        protected BaseObject()
        {
            _disposableTracker = new DisposableTracker(this);
            _lock = new object();
        }

        protected virtual object Lock
        {
            get { return _lock; }
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
