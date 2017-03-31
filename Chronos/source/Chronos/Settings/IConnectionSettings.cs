using System;

namespace Chronos.Settings
{
    public interface IConnectionSettings
    {
        Guid ObjectUid { get; }

        string MachineAddress { get; set; }

        IChannelSettings Channel { get; }

        Communication.Managed.ConnectionSettings GetConnectionSettings();
    }
}
