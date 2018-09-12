using System;
using System.IO.Pipes;
using System.Threading;

namespace Chronos.Communication.Native.NamedPipe
{
    internal class StreamFactory : IStreamFactory
    {
        private readonly Guid _currentApplicationUid;
        private int _lastDataStreamIndex;
        private ServerStream _invokeServerStream;

        public StreamFactory(Guid applicationUid)
        {
            _lastDataStreamIndex = 0;
            _currentApplicationUid = applicationUid;
        }

        public IServerStream CreateDataStream()
        {
            int index = Interlocked.Increment(ref _lastDataStreamIndex) - 1;
            string name = string.Format("chronosprofiler_{0}_data_{1}", _currentApplicationUid.ToString("N").ToUpper(), index);
            ServerStream stream = new ServerStream(name, PipeDirection.In);
            return stream;
        }

        public IServerStream CreateInvokeStream()
        {
            if (_invokeServerStream == null || _invokeServerStream.IsDisposed)
            {
                string name = string.Format("chronosprofiler_{0}_invoke", _currentApplicationUid.ToString("N").ToUpper());
                _invokeServerStream = new ServerStream(name, PipeDirection.InOut);
            }
            return _invokeServerStream;
        }

        public IClientStream OpenInvokeStream(Guid applicationUid)
        {
            string name = string.Format("chronosprofiler_{0}_invoke", applicationUid.ToString("N").ToUpper());
            IClientStream clientStream = new ClientStream(name);
            return clientStream;
        }

        public void Dispose()
        {
            _invokeServerStream.Dispose();
        }
    }
}
