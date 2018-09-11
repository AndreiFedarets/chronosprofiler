using System;
using Chronos.Win32;

namespace Chronos.Communication.Native
{
    internal sealed class NativeDataHandlerRouter : INativeDataHandler, IDisposable
    {
        private readonly IntPtr _nativeDataHandler;
        private readonly IManagedDataHandler _managedDataHandler;
        private readonly ManagedCallbackHolder<NativeMethods.DataHandlerRouter.HandlePackage> _handlePackageCallback;

        public NativeDataHandlerRouter(IManagedDataHandler managedDataHandler)
        {
            _managedDataHandler = managedDataHandler;
            _handlePackageCallback = new ManagedCallbackHolder<NativeMethods.DataHandlerRouter.HandlePackage>(HandlePackage);
            _nativeDataHandler = NativeMethods.DataHandlerRouter.Create(_handlePackageCallback.Callback);
        }

        public IntPtr DataHandlerPointer
        {
            get { return _nativeDataHandler; }
        }

        private bool HandlePackage(IntPtr data, int dataSize)
        {
            NativeArray nativeArray = new NativeArray(data, dataSize);
            bool result = _managedDataHandler.HandlePackage(nativeArray);
            if (result)
            {
                //We should override data pointer, otherwise when GC will call destructor, we will get access violation
                //because data was already removed by native code
                nativeArray.Data = IntPtr.Zero;
            }
            return result;
        }

        public void Dispose()
        {
            NativeMethods.DataHandlerRouter.Destroy(_nativeDataHandler);
            _handlePackageCallback.Dispose();
        }
    }
}
