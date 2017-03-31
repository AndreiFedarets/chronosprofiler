using System;

namespace Chronos.Communication.Managed
{
    public class ConnectionSettings
    {
        public ConnectionSettings(ChannelSettings channelSettings, string machineAddress, Guid objectUid)
        {
            ChannelSettings = channelSettings;
            MachineAddress = machineAddress;
            ObjectUid = objectUid;
        }

        public ChannelSettings ChannelSettings { get; private set; }

        public string MachineAddress { get; private set; }

        public Guid ObjectUid { get; private set; }

        public string ObjectUri
        {
            get
            {
                string channelName = ChannelSettings.GetChannedName(MachineAddress);
                string objectUri = string.Format("{0}/{1}", channelName, ObjectUid);
                return objectUri;
            }
        }
    }
}
