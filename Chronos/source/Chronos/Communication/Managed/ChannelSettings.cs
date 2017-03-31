using System;

namespace Chronos.Communication.Managed
{
    [Serializable]
    public abstract class ChannelSettings
    {
        public abstract string GetChannedName(string machineAddress);

        internal abstract IChannelController CreateController();
    }
}
