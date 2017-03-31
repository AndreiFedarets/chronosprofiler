using System;

namespace Chronos.Communication.Native
{
    public interface INativeDataHandler : IDataHandler
    {
        IntPtr DataHandlerPointer { get; }
    }
}
