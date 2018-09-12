using System;

namespace Chronos.Communication.Native
{
    public interface IRequestServer
    {
        IDisposable RegisterHandler(IRequestServerHandler handler);
    }
}
