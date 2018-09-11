using System;
using System.IO;
using System.Runtime.InteropServices;
using Chronos.Win32;

namespace Chronos
{
    internal sealed class AgentLibrary : IAgentLibrary, IDisposable
    {
        private readonly UnmangedLibrary _agentLibrary;
        private readonly DisposableTracker _disposableTracker;

        public AgentLibrary()
        {
            _disposableTracker = new DisposableTracker(this);
            _agentLibrary = new UnmangedLibrary(LibraryFullName);
        }

        public static string LibraryFullName
        {
            get
            {
                string path = typeof(AgentLibrary).GetAssemblyPath();
                string fullName = Path.Combine(path, "Chronos.Agent.dll");
                return fullName;
            }
        }

        public void SetupCrashDumpLogger(string dumpsDirectoryPath)
        {
            lock (_agentLibrary)
            {
                VerifyDisposed();
                SetupCrashDumpLoggerFunction function = _agentLibrary.GetFunction<SetupCrashDumpLoggerFunction>("SetupCrashDumpLogger");
                function(dumpsDirectoryPath);
            }
        }

        public IntPtr GatewayServerCreate(Guid sessionUid)
        {
            lock (_agentLibrary)
            {
                VerifyDisposed();
                GatewayServerCreateFunction function = _agentLibrary.GetFunction<GatewayServerCreateFunction>("GatewayServer_Create");
                return function(sessionUid);
            }
        }

        public bool GatewayServerIsLocked(IntPtr gatewayToken)
        {
            lock (_agentLibrary)
            {
                VerifyDisposed();
                GatewayServerIsLockedFunction function = _agentLibrary.GetFunction<GatewayServerIsLockedFunction>("GatewayServer_IsLocked");
                return function(gatewayToken);
            }
        }

        public void GatewayServerStart(IntPtr gatewayToken, byte threadsCount)
        {
            lock (_agentLibrary)
            {
                VerifyDisposed();
                GatewayServerStartFunction function = _agentLibrary.GetFunction<GatewayServerStartFunction>("GatewayServer_Start");
                function(gatewayToken, threadsCount);
            }
        }

        public void GatewayServerRegisterHandler(IntPtr gatewayToken, byte dataMarker, IntPtr handlerToken)
        {
            lock (_agentLibrary)
            {
                VerifyDisposed();
                GatewayServerRegisterHandlerFunction function = _agentLibrary.GetFunction<GatewayServerRegisterHandlerFunction>("GatewayServer_RegisterHandler");
                int result = function(gatewayToken, dataMarker, handlerToken);
                if (result != 0)
                {
                    Exception exception = Marshal.GetExceptionForHR(result);
                    throw new TempException(exception);
                }
            }
        }

        public void GatewayServerLock(IntPtr gatewayToken)
        {
            lock (_agentLibrary)
            {
                VerifyDisposed();
                GatewayServerLockFunction function = _agentLibrary.GetFunction<GatewayServerLockFunction>("GatewayServer_Lock");
                function(gatewayToken);
            }
        }

        public void GatewayServerDestroy(IntPtr gatewayToken)
        {
            lock (_agentLibrary)
            {
                VerifyDisposed();
                GatewayServerDestroyFunction function = _agentLibrary.GetFunction<GatewayServerDestroyFunction>("GatewayServer_Destroy");
                function(gatewayToken);
            }
        }

        void IDisposable.Dispose()
        {
            lock (_agentLibrary)
            {
                VerifyDisposed();
                _agentLibrary.Dispose();
                _disposableTracker.Dispose();
            }
        }

        private void VerifyDisposed()
        {
            _disposableTracker.VerifyDisposed();
        }
        
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void SetupCrashDumpLoggerFunction([MarshalAs(UnmanagedType.LPWStr)]string sessionUid);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate IntPtr GatewayServerCreateFunction(Guid sessionUid);
        
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate bool GatewayServerIsLockedFunction(IntPtr gatewayToken);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void GatewayServerStartFunction(IntPtr gatewayToken, byte streamsCount);
        
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate int GatewayServerRegisterHandlerFunction(IntPtr gatewayToken, byte dataMarker, IntPtr handlerToken);
        
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void GatewayServerLockFunction(IntPtr gatewayToken);
        
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void GatewayServerDestroyFunction(IntPtr gatewayToken);
    }
}
