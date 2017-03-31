using System;
using System.Collections.Generic;

namespace Chronos.Communication.Native
{
    public sealed class NativeGatewayServer : IGatewayServer
    {
        private readonly IntPtr _token;
        private readonly Dictionary<byte, INativeDataHandler> _handlers;
        private readonly IAgentLibrary _agentLibrary;
        private readonly GatewaySettings _gatewaySettings;
        private readonly DisposableTracker _disposableTracker;

        public NativeGatewayServer(IAgentLibrary agentLibrary, Guid sessionUid, GatewaySettings gatewaySettings)
        {
            _disposableTracker = new DisposableTracker(this);
            _agentLibrary = agentLibrary;
            _gatewaySettings = gatewaySettings;
            _token = agentLibrary.GatewayServerCreate(sessionUid);
            _handlers = new Dictionary<byte, INativeDataHandler>();
        }

        public bool IsLocked
        {
            get
            {
                lock (_handlers)
                {
                    VerifyDisposed();
                    bool locked = _agentLibrary.GatewayServerIsLocked(_token);
                    locked = false;
                    return locked;
                }
            }
        }

        public void Start()
        {
            lock (_handlers)
            {
                VerifyDisposed();
                _agentLibrary.GatewayServerStart(_token, (byte)_gatewaySettings.StreamsCount);
            }
        }

        public void Register(byte dataMarker, IDataHandler handler)
        {
            lock (_handlers)
            {
                VerifyDisposed();
                if (IsLocked)
                {
                    throw new TempException(string.Format("Gateway is already locked. Cannot register handler for marker {0}", dataMarker));
                }
                if (_handlers.ContainsKey(dataMarker))
                {
                    throw new TempException(string.Format("Data handler with marker {0} is already registered", dataMarker));
                }
                INativeDataHandler nativeDataHandler = GetNativeDataHandler(handler);
                if (nativeDataHandler == null)
                {
                    return;
                }
                _handlers.Add(dataMarker, nativeDataHandler);
                _agentLibrary.GatewayServerRegisterHandler(_token, dataMarker, nativeDataHandler.DataHandlerPointer);
            }
        }

        public void Lock()
        {
            lock (_handlers)
            {
                VerifyDisposed();
                _agentLibrary.GatewayServerLock(_token);
            }
        }

        public void Dispose()
        {
            lock (_handlers)
            {
                VerifyDisposed();
                foreach (INativeDataHandler handler in _handlers.Values)
                {
                    //We should dispose routers created by us because they contain unmanaged memory that should be removed
                    NativeDataHandlerRouter router = handler as NativeDataHandlerRouter;
                    if (router != null)
                    {
                        router.Dispose();
                    }
                }
                _agentLibrary.GatewayServerDestroy(_token);
                _disposableTracker.Dispose();
            }
        }

        private INativeDataHandler GetNativeDataHandler(IDataHandler handler)
        {
            INativeDataHandler nativeDataHandler = handler as INativeDataHandler;
            if (nativeDataHandler != null)
            {
                return nativeDataHandler;
            }
            IManagedDataHandler managedDataHandler = handler as IManagedDataHandler;
            if (managedDataHandler != null)
            {
                return new NativeDataHandlerRouter(managedDataHandler);
            }
            return null;
        }

        private void VerifyDisposed()
        {
            _disposableTracker.VerifyDisposed();
        }
    }
}
