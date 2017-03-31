using System;

namespace Chronos.Communication.Remoting
{
	public interface IRemotingExecutor
	{
		event Action ConnectionRestored;

		event Action ConnectionFailed;

		T Execute<T>(Func<T> func);

		void Execute(Action action);
	}
}
