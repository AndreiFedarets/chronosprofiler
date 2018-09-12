using System;
using System.Runtime.Serialization;

namespace Chronos
{
    public class ProfilingTypeInitializationException : ProfilerException
    {
        public ProfilingTypeInitializationException(Guid profilingTypeUid, Exception exception)
            : base(string.Format("Error during initialization of profiling type with uid '{0}'", profilingTypeUid), exception)
        {
            ProfilingTypeUid = profilingTypeUid;
        }

        protected ProfilingTypeInitializationException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
            
        }

        public Guid ProfilingTypeUid { get; private set; }
    }
}
