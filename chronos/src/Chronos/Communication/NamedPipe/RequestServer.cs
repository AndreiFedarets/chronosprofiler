using System;
using System.IO;
using Rhiannon.Logging;
using Rhiannon.Serialization.Marshaling;

namespace Chronos.Communication.NamedPipe
{
	public class RequestServer : IRequestServer
	{
		private readonly IServerStreamController _controller;
		private readonly ServerCallbacks _callbacks;

		public RequestServer(string pipeName, ServerCallbacks callbacks)
		{
			_callbacks = callbacks;
			_controller = new NamedPipeServerStreamController(pipeName);
			_controller.ProcessRequest += OnProcessRequest;
		}

		private void OnProcessRequest(Stream stream)
		{
			long operationCode = Int64Marshaler.Demarshal(stream);
			ServerCallback callback = _callbacks[operationCode];
			object[] arguments = new object[callback.Signature.Length];
			for (int i = 0; i < callback.Signature.Length; i++)
			{
				Type type = callback.Signature[i];
				arguments[i] = MarshalingManager.Demarshal(type, stream);
			}
			try
			{
				using (MemoryStream memorystream = new MemoryStream())
				{
					object resultObject = _callbacks.Invoke(callback, arguments);
					Int64Marshaler.Marshal((long)ResultCode.Ok, memorystream);
					if (callback.ReturnType != typeof (void))
					{
						MarshalingManager.Marshal(resultObject, memorystream);
					}
					stream.Write(memorystream.ToArray(), 0, (int)memorystream.Length);
				}
			}
			catch (Exception exception)
			{
				LoggingProvider.Log(exception, Policy.Core);
				ResultCode resultCode;
				string message;
				if (exception is OperationException)
				{
					OperationException operationException = (OperationException) exception;
					resultCode = operationException.Result;
					message = operationException.Message;
				}
				else
				{
					resultCode = ResultCode.UnknownError;
					message = exception.Message;
				}
				//If pipe is alive send error
				if (_controller.IsConnected)
				{
					Int64Marshaler.Marshal((long)resultCode, stream);
					StringMarshaler.Marshal(message, stream);
				}
			}
		}

		public void Dispose()
		{
			_controller.Dispose();
		}
	}
}
