using Chronos.Communication.Managed;
using System;
using System.Xml.Linq;

namespace Chronos.Settings
{
    internal sealed class ConnectionSettings : SettingsElement, IConnectionSettings
    {
        private const string MachineAddressAttributeName = "MachineAddress";
        private const string ObjectUidAttributeName = "ObjectUid";

        public ConnectionSettings(XElement element)
            : base(element)
        {
            if (Element.Element(IpcChannelSettings.ElementName) != null)
            {
                Channel = new IpcChannelSettings(Element.Element(IpcChannelSettings.ElementName));
            }
            else if (Element.Element(TcpChannelSettings.ElementName) != null)
            {
                Channel = new TcpChannelSettings(Element.Element(TcpChannelSettings.ElementName));
            }
            else if (Element.Element(HttpChannelSettings.ElementName) != null)
            {
                Channel = new HttpChannelSettings(Element.Element(HttpChannelSettings.ElementName));
            }
            else
            {
                throw new TempException();
            }
        }

        public string MachineAddress
        {
            get
            {
                XAttribute attribute = Element.Attribute(MachineAddressAttributeName);
                return attribute.Value;
            }
            set
            {
                XAttribute attribute = Element.Attribute(MachineAddressAttributeName);
                attribute.Value = value;
            }
        }

        public Guid ObjectUid
        {
            get
            {
                XAttribute attribute = Element.Attribute(ObjectUidAttributeName);
                return attribute.ValueAsGuid();
            }
        }

        public IChannelSettings Channel { get; private set; }

        public Communication.Managed.ConnectionSettings GetConnectionSettings()
        {
            ChannelSettings channelSettings = Channel.GetChannelSettings();
            Communication.Managed.ConnectionSettings connectionSettings = new Communication.Managed.ConnectionSettings(channelSettings, MachineAddress, ObjectUid);
            return connectionSettings;
        }
    }
}
