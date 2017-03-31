using System;
using System.Runtime.Serialization;

namespace Chronos
{
    public class ProfilingTargetAlreadyExistsException : ProfilerException
    {
        public ProfilingTargetAlreadyExistsException(Guid profilingTargetUid)
            : base(string.Format("Profiling Target with token '{0}' already exists", profilingTargetUid))
        {
        }

        public ProfilingTargetAlreadyExistsException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }

    }
}
