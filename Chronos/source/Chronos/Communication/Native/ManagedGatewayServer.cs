using System;
using System.Collections.Generic;

namespace Chronos.Communication.Native
{
    public sealed class ManagedGatewayServer : IGatewayServer
    {
        private const int DataHandlersMaxCount = 256;
        private volatile bool _isLocked;
        private readonly IStreamFactory _streamFactory;
        private readonly IManagedDataHandler[] _handlers;
        private readonly List<GatewayStream> _streams;
        private readonly GatewaySettings _gatewaySettings;
        private readonly DisposableTracker _disposableTracker;

        public ManagedGatewayServer(IStreamFactory streamFactory, GatewaySettings gatewaySettings)
        {
            _disposableTracker = new DisposableTracker(this);
            _gatewaySettings = gatewaySettings;
            _streamFactory = streamFactory;
            _handlers = new IManagedDataHandler[DataHandlersMaxCount];
            _streams = new List<GatewayStream>();
        }

        public bool IsLocked
        {
            get { return _isLocked; }
        }

        public void Start()
        {
            lock (_streams)
            {
                for (int i = 0; i < _gatewaySettings.StreamsCount; i++)
                {
                    IServerStream serverStream = _streamFactory.CreateDataStream();
                    GatewayStream gatewayStream = new GatewayStream(serverStream, _handlers);
                    _streams.Add(gatewayStream);
                    gatewayStream.Start();
                }
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
                if (_handlers[dataMarker] != null)
                {
                    throw new TempException(string.Format("Data handler with marker {0} is already registered", dataMarker));
                }
                if (handler == null)
                {
                    return;
                }
                IManagedDataHandler managedDataHandler = GetManagedDataHandler(handler);
                if (managedDataHandler == null)
                {
                    return;
                }
                _handlers[dataMarker] = managedDataHandler;
            }
        }

        public void Lock()
        {
            lock (_handlers)
            {
                VerifyDisposed();
                _isLocked = true;
            }
        }

        public void Dispose()
        {
            lock (_handlers)
            {
                VerifyDisposed();
                foreach (GatewayStream stream in _streams)
                {
                    stream.Dispose();
                }
                for (short i = 0; i < DataHandlersMaxCount; i++)
                {
                    _handlers[i] = null;
                }
                _streams.Clear();
                _disposableTracker.Dispose();
            }
        }

        private IManagedDataHandler GetManagedDataHandler(IDataHandler handler)
        {
            IManagedDataHandler managedDataHandler = handler as IManagedDataHandler;
            if (managedDataHandler != null)
            {
                return managedDataHandler;
            }
            INativeDataHandler nativeDataHandler = handler as INativeDataHandler;
            if (nativeDataHandler != null)
            {
                return new ManagedDataHandlerRouter(nativeDataHandler);
            }
            return null;
        }

        private void VerifyDisposed()
        {
            _disposableTracker.VerifyDisposed();
        }
    }
}
