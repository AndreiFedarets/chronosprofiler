using System;
using System.Runtime.Remoting;

namespace Chronos.Communication.Remoting
{
	public class RemotingFactory
	{
		private static readonly object Lock;
		private static RemotingFactory _current;
		private IChannelFactory _channelFactory;

		static RemotingFactory()
		{
			Lock = new object();
		}

		private RemotingFactory()
		{

		}

		public static RemotingFactory Current
		{
			get
			{
				lock (Lock)
				{
					return _current ?? (_current = new RemotingFactory());
				}
			}
		}

		public void Initialize(IChannelFactory channelFactory)
		{
			_channelFactory = channelFactory;
			_channelFactory.Initialize();
		}

		public void Share<T>(MarshalByRefObject server, string port)
		{
			Channel channel = _channelFactory.CreateChannel(port);
			if (!channel.IsRegistered)
			{
				channel.Register();
			}
			RemotingServices.Marshal(server, port, typeof(T));
		}

		public T Connect<T>(string port)
		{
			string serverUri = _channelFactory.GetServerUri(port);
			T value = (T)Activator.GetObject(typeof(T), serverUri);
			return value;
		}
	}
}
