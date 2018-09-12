using System;

namespace Chronos.Communication.Native
{
    public interface IGatewayServer : IDisposable
    {
        bool IsLocked { get; }

        void Start();

        void Lock();

        void Register(byte dataMarker, IDataHandler handler);
    }
}
