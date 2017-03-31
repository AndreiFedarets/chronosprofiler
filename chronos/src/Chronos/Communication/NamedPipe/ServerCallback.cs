using System;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Reflection;

namespace Chronos.Communication.NamedPipe
{
	public class ServerCallback
	{
		private readonly MethodInfo _methodInfo;
		private readonly ServerCallbacks _target;

		public ServerCallback(long operationCode, MethodInfo methodInfo, ServerCallbacks target)
		{
			OperationCode = operationCode;
			_methodInfo = methodInfo;
			_target = target;
			Signature = _methodInfo.GetParameters().Select(x => x.ParameterType).ToArray();
			ReturnType = _methodInfo.ReturnType;
		}

		public long OperationCode { get; private set; }

		public Type[] Signature { get; private set; }

		public Type ReturnType { get; private set; }

		public bool RequireUiThread { get; private set; }

		public object Invoke(Type resultType, object[] arguments)
		{
			Debug.Assert(resultType == _methodInfo.ReturnType);
			try
			{
				object result = _methodInfo.Invoke(_target, BindingFlags.InvokeMethod, null, arguments, CultureInfo.CurrentCulture);
				return result;
			}
			catch (Exception exception)
			{
				throw exception.InnerException;
			}
		}
	}
}
