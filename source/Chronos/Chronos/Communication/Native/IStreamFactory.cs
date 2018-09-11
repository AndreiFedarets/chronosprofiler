using System;

namespace Chronos.Communication.Native
{
    public interface IStreamFactory : IDisposable
    {
        IServerStream CreateDataStream();

        IServerStream CreateInvokeStream();

        IClientStream OpenInvokeStream(Guid applicationUid);
    }
}
