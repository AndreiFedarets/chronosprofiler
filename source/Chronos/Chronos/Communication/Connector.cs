using System;
using Chronos.Communication.Managed;
using Chronos.Communication.Native;

namespace Chronos.Communication
{
    public static class Connector
    {
        public static Guid ApplicationUid { get; private set; }

        public static void Initialize(Guid applicationUid)
        {
            ApplicationUid = applicationUid;
            Managed = new ManagedConnector();
            Managed.CreateChannel(CreateDefaultChannelSettings());
            Native = new NativeConnector(applicationUid);
        }

        public static ManagedConnector Managed { get; private set; }

        public static NativeConnector Native { get; private set; }

        private static ChannelSettings CreateDefaultChannelSettings()
        {
            IpcChannelSettings channelSettings = new IpcChannelSettings(ApplicationUid);
            return channelSettings;
        }
    }
}
