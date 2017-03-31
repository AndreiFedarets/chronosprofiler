using System;
using System.Runtime.Remoting;

namespace Chronos.Proxy
{
    internal sealed class LazyValue<T> : Lazy<T>, IDisposable
    {
        private readonly DisposableTracker _disposableTracker;

        public LazyValue(Func<T> valueFactory)
            : base(valueFactory)
        {
            _disposableTracker = new DisposableTracker(this);
        }

        public new T Value
        {
            get
            {
                VerifyDisposed();
                try
                {
                    return base.Value;
                }
                catch (RemotingException remotingException)
                {
                    throw new RemoteApplicationUnavailableException(remotingException);
                }
            }
        }

        public void Dispose()
        {
            if (IsValueCreated)
            {
                VerifyDisposed();
                Value.TryDispose();
            }
            _disposableTracker.Dispose();
        }

        protected void VerifyDisposed()
        {
            _disposableTracker.VerifyDisposed();
        }
    }
}
