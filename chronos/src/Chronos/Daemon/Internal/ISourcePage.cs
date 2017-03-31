using System;
using System.IO;

namespace Chronos.Daemon.Internal
{
    internal interface ISourcePage : IDisposable
    {
        uint ThreadId { get; set; }

        uint PageIndex { get; set; }

        uint BeginLifetime { get; set; }

        uint EndLifetime { get; set; }

        PageState Flag { get; set; }

        IntPtr Data { get; set; }

        int DataSize { get; set; }

        bool IsEmpty { get; }

        Stream OpenRead();

        Stream OpenWrite();
    }
}
