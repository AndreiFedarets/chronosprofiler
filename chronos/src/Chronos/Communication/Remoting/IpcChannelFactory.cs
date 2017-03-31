using System;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Ipc;
using System.Security.Principal;

namespace Chronos.Communication.Remoting
{
	public class IpcChannelFactory : IChannelFactory
	{
		private Channel _defaultChannel;

		public void Initialize()
		{
			_defaultChannel = CreateChannel(Guid.NewGuid().ToString());
			_defaultChannel.Register();
		}

		public void Dispose()
		{
			_defaultChannel.Unregister();
		}

		private static string GetAuthorizedGroup()
		{
			// Get SID code for the EveryOne user
			SecurityIdentifier sid = new SecurityIdentifier(WellKnownSidType.WorldSid, null);
			// Get the NT account related to the SID
			NTAccount account = sid.Translate(typeof(NTAccount)) as NTAccount;
			return account == null ? string.Empty : account.Value;
		}


		public Channel CreateChannel(string channelName)
		{
			string uri = string.Format("ipc://{0}", channelName);
			BinaryServerFormatterSinkProvider serverProvider = new BinaryServerFormatterSinkProvider();
			serverProvider.TypeFilterLevel = System.Runtime.Serialization.Formatters.TypeFilterLevel.Full;
			BinaryClientFormatterSinkProvider clientProvider = new BinaryClientFormatterSinkProvider();
			System.Collections.IDictionary properties = new System.Collections.Hashtable();
			properties["portName"] = channelName;
			properties["exclusiveAddressUse"] = true;
			properties["authorizedGroup"] = GetAuthorizedGroup();
			IpcChannel channel = new IpcChannel(properties, clientProvider, serverProvider);
			return new Channel(channel, channelName, uri);
		}

		public string GetServerUri(string port)
		{
			string uri = string.Format("ipc://{0}/{0}", port);
			return uri;
		}
	}
}
