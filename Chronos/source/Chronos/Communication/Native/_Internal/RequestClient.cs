using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;

namespace Chronos.Communication.Native
{
    internal sealed class RequestClient : IRequestClient, IDisposable
    {
        private readonly IClientStream _clientStream;

        public RequestClient(IClientStream clientStream)
        {
            _clientStream = clientStream;
        }

        public object Invoke(Guid operationId, Type returnType, params object[] arguments)
        {
            try
            {
                using (Stream stream = _clientStream.Connect())
                {
                    Invoker invoker = new Invoker(stream, true);
                    invoker.WriteOperationId(operationId);
                    invoker.WriteArguments(arguments);
                    invoker.Flush();
                    int resultCode = invoker.ReadResultCode();
                    if (resultCode != (int)Result.S_OK)
                    {
                        Exception exception = Marshal.GetExceptionForHR(resultCode);
                        throw exception;
                    }
                    object result = invoker.ReadResult(returnType);
                    return result;
                }
            }
            catch (Exception exception)
            {
                LoggingProvider.Current.Log(TraceEventType.Error, exception);
                throw;
            }
        }

        public void Dispose()
        {
            
        }
    }
}
