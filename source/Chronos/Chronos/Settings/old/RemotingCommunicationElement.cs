using System.Configuration;

namespace Chronos.Config
{
    public class RemotingCommunicationElement : ConfigurationElement
    {
        private const string IpcProtocolPropertyName = "ipc";
        private const string TcpProtocolPropertyName = "tcp";
        private const string HttpProtocolPropertyName = "http";

        [ConfigurationProperty(IpcProtocolPropertyName, IsRequired = false)]
        public IpcProtocolElement Ipc
        {
            get { return (IpcProtocolElement)this[IpcProtocolPropertyName]; }
            set { this[IpcProtocolPropertyName] = value; }
        }

        [ConfigurationProperty(TcpProtocolPropertyName, IsRequired = false)]
        public TcpProtocolElement Tcp
        {
            get { return (TcpProtocolElement)this[TcpProtocolPropertyName]; }
            set { this[TcpProtocolPropertyName] = value; }
        }

        [ConfigurationProperty(HttpProtocolPropertyName, IsRequired = false)]
        public HttpProtocolElement Http
        {
            get { return (HttpProtocolElement)this[HttpProtocolPropertyName]; }
            set { this[HttpProtocolPropertyName] = value; }
        }
    }
}
