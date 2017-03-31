using System;
using System.Runtime.Serialization;

namespace Chronos
{
    public class ProfilingTypeNotFoundException : ProfilerException
    {
        public ProfilingTypeNotFoundException(Guid profilingTypeUid)
            : base(string.Format("Profiling Type with uid '{0}' was not found", profilingTypeUid))
        {
            ProfilingTypeUid = profilingTypeUid;
        }
        
        public ProfilingTypeNotFoundException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
            
        }

        public Guid ProfilingTypeUid { get; private set; }
    }
}
