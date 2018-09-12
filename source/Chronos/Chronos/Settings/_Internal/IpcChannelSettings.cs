using Chronos.Communication.Managed;
using System.Xml.Linq;

namespace Chronos.Settings
{
    internal sealed class IpcChannelSettings : SettingsElement, IIpcChannelSettings
    {
        public const string ElementName = "IpcChannel";

        public IpcChannelSettings(XElement element)
            : base(element)
        {
        }

        public ChannelSettings GetChannelSettings()
        {
            Communication.Managed.IpcChannelSettings channelSettings = new Communication.Managed.IpcChannelSettings(Host.Application.ApplicationUid);
            return channelSettings;
        }
    }
}
