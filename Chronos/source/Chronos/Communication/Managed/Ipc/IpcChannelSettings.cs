using System;

namespace Chronos.Communication.Managed
{
    [Serializable]
    public class IpcChannelSettings : ChannelSettings
    {
        public IpcChannelSettings(Guid channelName)
        {
            ChannelName = channelName.ToString();
        }

        public string ChannelName { get; private set; }

        public override string GetChannedName(string machineAddress)
        {
            return string.Format("ipc://{0}", ChannelName);
        }

        internal override IChannelController CreateController()
        {
            return new IpcChannelController(this);
        }
    }
}
