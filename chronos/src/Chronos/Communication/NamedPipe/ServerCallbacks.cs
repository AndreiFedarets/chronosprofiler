using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Chronos.Communication.NamedPipe
{
	public abstract class ServerCallbacks : IServerInvoke
	{
		private IDictionary<long, ServerCallback> _callbacks;

		public ServerCallback this[long operationCode]
		{
			get
			{
				if (_callbacks == null)
				{
					_callbacks = Initialize();
				}
				return _callbacks[operationCode];
			}
		}

		public object Invoke(long operationCode, Type resultType, object[] arguments)
		{
			ServerCallback serverCallback = this[operationCode];
			return serverCallback.Invoke(resultType, arguments);
		}

		public object Invoke(ServerCallback callback, object[] arguments)
		{
			return callback.Invoke(callback.ReturnType, arguments);
		}

		public virtual void Dispose()
		{

		}

		private IDictionary<long, ServerCallback> Initialize()
		{
			IDictionary<long, ServerCallback> callbacks = new Dictionary<long, ServerCallback>();
			foreach (MethodInfo methodInfo in GetType().GetMethods())
			{
				object[] attributes = methodInfo.GetCustomAttributes(typeof(OperationHandlerAttribute), true);
				if (attributes.Any())
				{
					OperationHandlerAttribute attribute = (OperationHandlerAttribute)attributes[0];
					callbacks.Add(attribute.OperationCode, new ServerCallback(attribute.OperationCode, methodInfo, this));
				}
			}
			return callbacks;
		}
	}
}
