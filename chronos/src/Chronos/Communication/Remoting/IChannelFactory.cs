using System;

namespace Chronos.Communication.Remoting
{
	public interface IChannelFactory : IDisposable
	{
		Channel CreateChannel(string port);

		string GetServerUri(string port);

		void Initialize();
	}
}
