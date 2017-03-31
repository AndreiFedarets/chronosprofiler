using System;
using System.Reflection;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Messaging;
using System.Runtime.Remoting.Proxies;
using System.Globalization;

namespace Rhiannon.Interception
{
	public class Interceptor : RealProxy, IRemotingTypeInfo
	{
		private readonly object _target;
		private readonly Type _targetType;
		private readonly MethodBase _method;

		public Interceptor(object target)
			: base(typeof(MarshalByRefObject))
		{
			_target = target;
			_targetType = _target.GetType();
			_method = _targetType.GetMethod("get_CreationTime");
		}

		public string TypeName { get; set; }

		public bool CanCastTo(Type fromType, object o)
		{
			return true;
		}

		public override IMessage Invoke(IMessage message)
		{
			IMethodCallMessage methodMessage = (IMethodCallMessage)message;
			MethodBase callingMethod = methodMessage.MethodBase;
			object[] arguments = methodMessage.Args;
			BindingFlags flags = BindingFlags.InvokeMethod;
			//MethodBase realMethod = _targetType.GetMethod(callingMethod.Name);

			object returnValue = _method.Invoke(_target, flags, null, arguments, CultureInfo.CurrentCulture);
			return new ReturnMessage(returnValue, methodMessage.Args, methodMessage.ArgCount, methodMessage.LogicalCallContext, methodMessage);
		}
	}
}
