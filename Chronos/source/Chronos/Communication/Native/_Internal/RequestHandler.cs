using System;
using System.Linq;
using System.Reflection;

namespace Chronos.Communication.Native
{
    internal sealed class RequestHandler : IDisposable
    {
        private const string HandleMethodName = "Handle";
        private readonly RequestServer _requestServer;
        private readonly IRequestServerHandler _handler;
        private readonly MethodInfo _method;

        public RequestHandler(IRequestServerHandler handler, RequestServer requestServer)
        {
            _method = handler.GetType().GetMethod(HandleMethodName);
            if (_method == null)
            {
                throw new TempException();
            }
            _requestServer = requestServer;
            _handler = handler;
            Signature = _method.GetParameters().Select(x => x.ParameterType).ToArray();
            ReturnType = _method.ReturnType;
        }

        public Type[] Signature { get; private set; }

        public Type ReturnType { get; private set; }

        public object Invoke(object[] arguments)
        {
            return _method.Invoke(_handler, arguments);
        }

        public void Dispose()
        {
            _requestServer.UnregisterHandler(_handler.Uid);
        }
    }
}
