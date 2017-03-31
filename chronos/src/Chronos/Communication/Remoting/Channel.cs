using System;
using System.Runtime.Remoting.Channels;
using System.IO;

namespace Chronos.Communication.Remoting
{
	public class Channel
	{
		private readonly string _channelUri;
		private readonly string _channelName;
		private readonly IChannel _channel;

		public Channel(IChannel channel, string channelName, string channelUri)
		{
			_channel = channel;
			_channelName = channelName;
			_channelUri = channelUri;
		}

		public bool IsRegistered { get; private set; }

		public void Register()
		{
			try
			{
				ChannelServices.RegisterChannel(_channel, true);
			}
			catch (Exception)
			{
				
			}
			IsRegistered = true;
		}

		public void Unregister()
		{
			ChannelServices.UnregisterChannel(_channel);
			IsRegistered = false;
		}
	}
}
