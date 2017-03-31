using System;
using System.Collections.Generic;

namespace Chronos.Core
{
    public interface IThreadTraceCollection : IEnumerable<IThreadTrace>
    {
        IThreadTrace this[ThreadInfo threadInfo] { get; }

        event Action Reloaded;

        void Reload();

		long TotalTime { get; }
    }
}
