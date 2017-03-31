using System;
using System.IO.Pipes;
using Rhiannon.Serialization.Marshaling;

namespace Chronos.Communication.NamedPipe
{
	public class ServerInvoke : IServerInvoke
	{
		private readonly string _pipeName;

		public ServerInvoke(string pipeName)
		{
			_pipeName = pipeName;
		}

		public void Invoke(long operationCode, params object[] arguments)
		{
			Invoke(operationCode, typeof(void), arguments);
		}

		public T Invoke<T>(long operationCode, params object[] arguments)
		{
			return (T) Invoke(operationCode, typeof (T), arguments);
		}

		public object Invoke(long operationCode, Type resultType, params object[] arguments)
		{
			using (NamedPipeClientStream stream = Connect())
			{
				Int64Marshaler.Marshal(operationCode, stream);
				foreach (object agument in arguments)
				{
					MarshalingManager.Marshal(agument, stream);
				}
				ResultCode resultCode = (ResultCode) Int64Marshaler.Demarshal(stream);
				if (resultCode != ResultCode.Ok)
				{
					string message = StringMarshaler.Demarshal(stream);
					throw new OperationException(resultCode, message);
				}
				if (resultType == typeof(void))
				{
					return null;
				}
				object resultObject = MarshalingManager.Demarshal(resultType, stream);
				return resultObject;
			}
		}

		private NamedPipeClientStream Connect()
		{
			NamedPipeClientStream stream = new NamedPipeClientStream(".", _pipeName, PipeDirection.InOut, PipeOptions.Asynchronous);
			stream.Connect();
			return stream;
		}

		public void Dispose()
		{
		}
	}
}
