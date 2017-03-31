using System;

namespace Chronos.Communication.NamedPipe
{
	public interface IServerInvoke : IDisposable
	{
		object Invoke(long operationCode, Type resultType, params object[] arguments);
	}
}
