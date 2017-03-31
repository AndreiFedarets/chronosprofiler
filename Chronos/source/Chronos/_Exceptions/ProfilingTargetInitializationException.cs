using System;
using System.Runtime.Serialization;

namespace Chronos
{
    public class ProfilingTargetInitializationException : ProfilerException
    {
        public ProfilingTargetInitializationException(Guid profilingTargetUid, Exception exception)
            : base(string.Format("Error during initialization of profiling target with uid '{0}'", profilingTargetUid), exception)
        {
            ProfilingTargetUid = profilingTargetUid;
        }

        protected ProfilingTargetInitializationException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
            
        }

        public Guid ProfilingTargetUid { get; private set; }
    }
}
