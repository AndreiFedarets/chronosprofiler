using System;
using Chronos.Daemon.Internal;

namespace Chronos.Daemon.Internal
{
    internal interface IThreadStreamProcessor : IDisposable
    {
        void ProcessPage(ISourcePage sourcePage);
    }
}
