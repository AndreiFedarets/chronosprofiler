using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Tcp;

namespace Chronos.Communication.Managed
{
    internal class TcpChannelController : ChannelControllerBase
    {
        private readonly TcpChannelSettings _channelSettings;

        public TcpChannelController(TcpChannelSettings channelSettings)
        {
            _channelSettings = channelSettings;
        }

        protected override IChannel CreateChannelInternal()
        {
            BinaryServerFormatterSinkProvider serverProvider = new BinaryServerFormatterSinkProvider();
            serverProvider.TypeFilterLevel = System.Runtime.Serialization.Formatters.TypeFilterLevel.Full;
            BinaryClientFormatterSinkProvider clientProvider = new BinaryClientFormatterSinkProvider();
            System.Collections.IDictionary properties = new System.Collections.Hashtable();
            properties["port"] = _channelSettings.Port;
            properties["exclusiveAddressUse"] = true;
            properties["authorizedGroup"] = GetAuthorizedGroup();
            TcpChannel channel = new TcpChannel(properties, clientProvider, serverProvider);
            return channel;
        }
    }
}
