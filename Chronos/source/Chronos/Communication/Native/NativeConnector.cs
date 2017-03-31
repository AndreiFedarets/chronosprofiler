using System;

namespace Chronos.Communication.Native
{
    public sealed class NativeConnector
    {
        public NativeConnector(Guid applicationUid)
        {
            StreamFactory = new NamedPipe.StreamFactory(applicationUid);
        }

        public IStreamFactory StreamFactory { get; private set; }
    }
}
