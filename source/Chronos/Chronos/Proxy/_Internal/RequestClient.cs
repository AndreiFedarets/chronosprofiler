using System;
using Chronos.Communication.Native;

namespace Chronos.Proxy
{
    internal sealed class RequestClient : ProxyBaseObject<IRequestClient>, IRequestClient
    {
        public RequestClient(IRequestClient remoteObject)
            : base(remoteObject)
        {
        }

        public object Invoke(Guid operationId, Type returnType, params object[] args)
        {
            return Execute(() => RemoteObject.Invoke(operationId, returnType, args));
        }
    }
}
