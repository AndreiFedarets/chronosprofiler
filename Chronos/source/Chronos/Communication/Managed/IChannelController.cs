using System;

namespace Chronos.Communication.Managed
{
    internal interface IChannelController : IDisposable
    {
        void CreateChannel();
    }
}
