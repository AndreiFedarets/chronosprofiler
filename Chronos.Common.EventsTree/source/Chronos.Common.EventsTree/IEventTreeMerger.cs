using System;
using System.Collections.Generic;

namespace Chronos.Common.EventsTree
{
    public interface IEventTreeMerger : IDisposable
    {
        IEventTree[] Merge(IEnumerable<ISingleEventTree> source, EventTreeMergeType mergeType);
    }
}
