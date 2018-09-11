using System;

namespace Chronos
{
    public interface IAgentLibrary
    {
        IntPtr GatewayServerCreate(Guid sessionUid);

        bool GatewayServerIsLocked(IntPtr gatewayToken);

        void GatewayServerStart(IntPtr gatewayToken, byte threadsCount);

        void GatewayServerRegisterHandler(IntPtr gatewayToken, byte dataMarker, IntPtr handlerToken);

        void GatewayServerLock(IntPtr gatewayToken);

        void GatewayServerDestroy(IntPtr gatewayToken);
    }
}
