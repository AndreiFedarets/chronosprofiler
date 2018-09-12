using System;
using System.Runtime.InteropServices;

namespace Chronos.Communication.Native
{
    internal sealed class ManagedDataHandlerRouter : IManagedDataHandler, IDisposable
    {
        private IntPtr _handler;

        public ManagedDataHandlerRouter(INativeDataHandler nativeDataHandler)
        {
            _handler = nativeDataHandler.DataHandlerPointer;
        }

        [DllImport("Chronos.Agent.dll", EntryPoint = "DataHandler_HandlePackage", CallingConvention = CallingConvention.Cdecl)]
        private static extern bool HandlePackage(IntPtr token, IntPtr data, int dataSize);

        public bool HandlePackage(NativeArray array)
        {
            return HandlePackage(_handler, array.Data, array.Length);
        }

        public void Dispose()
        {
            _handler = IntPtr.Zero;
        }
    }
}
