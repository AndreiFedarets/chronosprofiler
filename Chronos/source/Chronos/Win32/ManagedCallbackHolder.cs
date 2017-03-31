using System;
using System.Runtime.InteropServices;

namespace Chronos.Win32
{
    public sealed class ManagedCallbackHolder<T> : IDisposable
    {
        private GCHandle _callbackHandle;
        private readonly T _callback;

        public ManagedCallbackHolder(T callback)
        {
            _callback = callback;
            _callbackHandle = GCHandle.Alloc(callback);
        }

        public T Callback
        {
            get { return _callback; }
        }

        public void Dispose()
        {
            _callbackHandle.Free();
        }
    }
}
