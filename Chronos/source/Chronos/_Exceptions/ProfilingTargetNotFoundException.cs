using System;
using System.Runtime.Serialization;

namespace Chronos
{
    public class ProfilingTargetNotFoundException : ProfilerException
    {
        public ProfilingTargetNotFoundException(Guid profilingTargetUid)
            : base(string.Format("Profiling Target with token '{0}' not found", profilingTargetUid))
        {
        }

        public ProfilingTargetNotFoundException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }

    }
}
