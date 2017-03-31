using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Ipc;

namespace Chronos.Communication.Managed
{
    internal class IpcChannelController : ChannelControllerBase
    {
        private readonly IpcChannelSettings _channelSettings;

        public IpcChannelController(IpcChannelSettings channelSettings)
        {
            _channelSettings = channelSettings;
            if (string.IsNullOrEmpty(channelSettings.ChannelName))
            {
                throw new InvalidChannelSettingsException("Channel name cannot be empty");
            }
        }

        protected override IChannel CreateChannelInternal()
        {
            BinaryServerFormatterSinkProvider serverProvider = new BinaryServerFormatterSinkProvider();
            serverProvider.TypeFilterLevel = System.Runtime.Serialization.Formatters.TypeFilterLevel.Full;
            BinaryClientFormatterSinkProvider clientProvider = new BinaryClientFormatterSinkProvider();
            System.Collections.IDictionary properties = new System.Collections.Hashtable();
            properties["portName"] = _channelSettings.ChannelName;
            properties["exclusiveAddressUse"] = true;
            properties["authorizedGroup"] = GetAuthorizedGroup();
            IpcChannel channel = new IpcChannel(properties, clientProvider, serverProvider);
            return channel;
        }
    }
}
