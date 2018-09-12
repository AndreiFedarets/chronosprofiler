using System.Configuration;
using System.Xml;

namespace Chronos.Config
{
    public abstract class RemotingProtocolElement : ConfigurationElement
    {
        public abstract string Key { get; }

        public void DeserializeElement(XmlReader reader)
        {
            DeserializeElement(reader, true);
        }
    }
}
