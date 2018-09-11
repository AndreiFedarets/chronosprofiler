using System;
using System.IO;

namespace Chronos.Communication.Native
{
    public delegate void ProcessRequestHandler(Stream stream);

    public interface IServerStream : IDisposable
    {
        bool IsConnected { get; }

        bool IsDisposed { get; }

        event ProcessRequestHandler ProcessRequest;

        void Disconnect();
    }
}
