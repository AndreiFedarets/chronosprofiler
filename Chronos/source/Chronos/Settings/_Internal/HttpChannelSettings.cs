using Chronos.Communication.Managed;
using System;
using System.Xml.Linq;

namespace Chronos.Settings
{
    internal sealed class HttpChannelSettings : SettingsElement, ITcpChannelSettings
    {
        public const string ElementName = "HttpChannel";

        public HttpChannelSettings(XElement element)
            : base(element)
        {
        }

        public ChannelSettings GetChannelSettings()
        {
            //Communication.Managed.HttpChannelSettings channelSettings = new Communication.Managed.HttpChannelSettings(...);
            //return channelSettings;
            throw new NotImplementedException();
        }
    }
}
