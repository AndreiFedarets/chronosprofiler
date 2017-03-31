using System;
using System.IO.Pipes;
using System.Runtime.InteropServices;
using Chronos.Communication.NamedPipe;
using Chronos.Core;
using Rhiannon.Extensions;
using Rhiannon.Logging;

namespace Chronos.Daemon.Internal
{
    internal class ThreadStream
    {
        private byte[] _dataReadBuffer;
        private byte[] _headerReadBuffer;
        private bool _disposed;
        private readonly NamedPipeServerStream _stream;
        private readonly IThreadStreamProcessor _processor;

        public ThreadStream(uint index, Guid daemonToken, IThreadStreamProcessor processor, uint callPageSize)
        {
            _disposed = false;
            _processor = processor;
            PipeSecurity security = NamedPipeServerStreamController.GetLowLevelPipeSecurity();
            _stream = new NamedPipeServerStream(PipeNameFormatter.GetDaemonServerThreadPipeName(daemonToken, index), PipeDirection.In, 1,
                                                PipeTransmissionMode.Byte, PipeOptions.Asynchronous, (int)callPageSize * 10, 0, security);
            _stream.BeginWaitForConnection(BeginRead, null);
            _dataReadBuffer = new byte[callPageSize * IntermediateEvent.RwSize];
            //threadId + callstackId + pageIndex + length + flag + beginLifetime + endLifetime
            _headerReadBuffer = new byte[sizeof(uint) + sizeof(uint) + sizeof(uint) + sizeof(int) + sizeof(byte) + sizeof(uint) + sizeof(uint)];
        }

        public event EventHandler Connected;

        unsafe private void BeginRead(IAsyncResult asyncResult)
        {
            if (_disposed)
            {
                return;
            }
            Connected.SafeInvoke(this, EventArgs.Empty);
            try
            {
                _stream.EndWaitForConnection(asyncResult);
            }
            catch
            {
                return;
            }
            while (true)
            {
                try
                {
                    _stream.Read(_headerReadBuffer, 0, _headerReadBuffer.Length);
                    if (!_stream.IsConnected)
                    {
                        return;
                    }
                    uint threadId;
                    uint callstackId;
                    uint pageIndex;
                    int dataSize;
                    byte flag;
                    uint beginLifetime;
                    uint endLifetime;
                    fixed (byte* headerReadBufferPointer = _headerReadBuffer)
                    {
                        if (headerReadBufferPointer == null)
                        {
                            return;
                        }
                        int offset = 0;

                        threadId = *(uint*)(headerReadBufferPointer + offset);
                        offset += sizeof(uint);

                        callstackId = *(uint*)(headerReadBufferPointer + offset);
                        offset += sizeof(uint);

                        pageIndex = *(uint*)(headerReadBufferPointer + offset);
                        offset += sizeof(uint);

                        dataSize = *(int*)(headerReadBufferPointer + offset);
                        offset += sizeof(int);

                        flag = *(headerReadBufferPointer + offset);
                        offset += sizeof(byte);

                        beginLifetime = *(uint*)(headerReadBufferPointer + offset);
                        offset += sizeof(uint);

                        endLifetime = *(uint*)(headerReadBufferPointer + offset);
                    }
                    SourcePage sourcePage = new SourcePage(threadId, callstackId, pageIndex, (PageState)flag, beginLifetime, endLifetime, dataSize);
                    _stream.Read(_dataReadBuffer, 0, dataSize);
                    Marshal.Copy(_dataReadBuffer, 0, sourcePage.Data, dataSize);
                    _processor.ProcessPage(sourcePage);
                }
                catch (ObjectDisposedException)
                {

                }
                catch (Exception exception)
                {
                    LoggingProvider.Log(exception, Policy.Core);
                    break;
                }
            }
        }

        public void Dispose()
        {
            try
            {
                _disposed = true;
                _stream.Dispose();
            }
            catch (Exception exception)
            {
                LoggingProvider.Log(exception, Policy.Core);
            }
            _dataReadBuffer = null;
            _headerReadBuffer = null;
        }
    }
}
