using Chronos.Communication.Managed;
using System.Xml.Linq;

namespace Chronos.Settings
{
    internal sealed class TcpChannelSettings : SettingsElement, ITcpChannelSettings
    {
        public const string ElementName = "TcpChannel";
        private const string PortAttributeName = "Port";

        public TcpChannelSettings(XElement element)
            : base(element)
        {
        }

        public ushort Port
        {
            get
            {
                XAttribute attribute = Element.Attribute(PortAttributeName);
                return attribute.ValueAsUshort();
            }
            set
            {
                XAttribute attribute = Element.Attribute(PortAttributeName);
                attribute.Value = value.ToString(); ;
            }
        }

        public ChannelSettings GetChannelSettings()
        {
            Communication.Managed.TcpChannelSettings channelSettings = new Communication.Managed.TcpChannelSettings(Port);
            return channelSettings;
        }
    }
}
