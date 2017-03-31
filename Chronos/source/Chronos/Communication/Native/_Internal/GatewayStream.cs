using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.ExceptionServices;
using System.Security;

namespace Chronos.Communication.Native
{
    internal sealed class GatewayStream : IDisposable
    {
        private const int DataBufferMaxSize = 3 * 1024 * 1024; //3mb
        private readonly IServerStream _serverStream;
        private readonly IManagedDataHandler[] _handlers;
        private bool _disposed;

        public GatewayStream(IServerStream serverStream, IManagedDataHandler[] handlers)
        {
            _disposed = false;
            _handlers = handlers;
            _serverStream = serverStream;
            _serverStream.ProcessRequest += Read;
            //dataMarker + dataSize
        }

        public void Start()
        {

        }

        [SecurityCritical]
        [HandleProcessCorruptedStateExceptions]
        unsafe private void Read(Stream stream)
        {
            byte[] dataReadBuffer = new byte[DataBufferMaxSize];
            byte[] headerReadBuffer = new byte[sizeof(byte) + sizeof(int)];
            while (!_disposed)
            {
                try
                {
                    int read = stream.Read(headerReadBuffer, 0, headerReadBuffer.Length);
                    byte dataMarker;
                    int dataSize;
                    if (read == 0)
                    {
                        continue;
                    }
                    fixed (byte* headerReadBufferPointer = headerReadBuffer)
                    {
                        dataMarker = *(headerReadBufferPointer);
                        dataSize = *(int*)(headerReadBufferPointer + 1);
                    }
                    if (dataSize > dataReadBuffer.Length)
                    {
                        LoggingProvider.Current.Log(TraceEventType.Error, string.Format("Attempt to read data bigger than buffer. Buffer size: {0}. Data size: {1}", dataReadBuffer.Length, dataSize));
                        continue;
                    }
                    stream.Read(dataReadBuffer, 0, dataSize);
                    NativeArray array = NativeArray.Copy(dataReadBuffer, dataSize);
                    IManagedDataHandler handler = _handlers[dataMarker];
                    if (handler == null || handler.HandlePackage(array))
                    {
                        array.Dispose();
                    }
                }
                catch (ObjectDisposedException objectDisposedException)
                {
                    LoggingProvider.Current.Log(TraceEventType.Information, objectDisposedException);
                    return;
                }
                catch (Exception exception)
                {
                    LoggingProvider.Current.Log(TraceEventType.Warning, exception);
                    break;
                }
            }
        }

        public void Dispose()
        {
            try
            {
                _disposed = true;
                _serverStream.Disconnect();
            }
            catch (Exception exception)
            {
                LoggingProvider.Current.Log(TraceEventType.Warning, exception);
            }
        }
    }
}
