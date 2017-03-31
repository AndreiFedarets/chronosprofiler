using System;
using System.IO;
using Chronos.Core;

namespace Chronos.Daemon.Internal
{
    internal interface IConvertedPage : IDisposable
    {
        uint ThreadId { get; set; }

        uint CallstackId { get; set; }

        uint PageIndex { get; set; }

        uint BeginPageRange { get; set; }

        uint EndPageRange { get; set; }

        uint BeginLifetime { get; set; }

        uint EndLifetime { get; set; }

        PageState Flag { get; set; }

        IntPtr Data { get; set; }

        int DataSize { get; set; }

        Stream OpenRead();

        Stream OpenWrite();

        CallstackInfo GetInfo();

        bool IsEmpty { get; }
    }
}
