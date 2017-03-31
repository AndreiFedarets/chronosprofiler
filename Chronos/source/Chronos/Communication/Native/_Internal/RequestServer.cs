using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;

namespace Chronos.Communication.Native
{
    internal sealed class RequestServer : BaseObject, IRequestServer
    {
        private readonly object _streamLock;
        private readonly Dictionary<Guid, RequestHandler> _handlers;
        private volatile IServerStream _serverStream;

        public RequestServer()
        {
            _streamLock = new object();
            _handlers = new Dictionary<Guid, RequestHandler>();
        }

        public void Run(IServerStream serverStream)
        {
            lock (_streamLock)
            {
                if (_serverStream == null)
                {
                    _serverStream = serverStream;
                    _serverStream.ProcessRequest += OnProcessRequest;
                }
            }
        }

        public IDisposable RegisterHandler(IRequestServerHandler handler)
        {
            lock (Lock)
            {
                Guid uid = handler.Uid;
                if (_handlers.ContainsKey(uid))
                {
                    throw new TempException();
                }
                RequestHandler requestHandler = new RequestHandler(handler, this);
                _handlers[uid] = requestHandler;
                return requestHandler;
            }
        }

        internal bool UnregisterHandler(Guid uid)
        {
            lock (Lock)
            {
                return _handlers.Remove(uid);
            }   
        }

        public override void Dispose()
        {
            lock (Lock)
            {
                _serverStream.Dispose();
                _handlers.Clear();
                base.Dispose();
            }
        }

        private void OnProcessRequest(Stream stream)
        {
            Result resultCode = Result.S_OK;
            object result = null;
            Invoker invoker = new Invoker(stream, true);
            Guid operationCode = invoker.ReadOperationId();
            RequestHandler handler;
            lock (Lock)
            {
                _handlers.TryGetValue(operationCode, out handler);
            }
            if (handler == null)
            {
                resultCode = Result.SEC_E_UNSUPPORTED_FUNCTION;
            }
            else
            {
                try
                {
                    object[] arguments = invoker.ReadArguments(handler.Signature);
                    result = handler.Invoke(arguments);
                }
                catch (Exception exception)
                {
                    LoggingProvider.Current.Log(TraceEventType.Error, exception);
                    resultCode = (Result)Marshal.GetExceptionCode();
                }
            }
            if (_serverStream.IsConnected)
            {
                invoker.WriteResultCode((int)resultCode);
                invoker.WriteResult(result);
                invoker.Flush();
            }
        }
    }
}
