using System;

namespace Chronos.Communication.Managed
{
    [Serializable]
    public class TcpChannelSettings : ChannelSettings
    {
        public TcpChannelSettings(ushort port)
        {
            Port = port;
        }

        public ushort Port { get; private set; }

        public override string GetChannedName(string machineAddress)
        {
            return string.Format("tcp://{0}:{1}", machineAddress, Port);
        }

        internal override IChannelController CreateController()
        {
            return new TcpChannelController(this);
        }
    }
}
