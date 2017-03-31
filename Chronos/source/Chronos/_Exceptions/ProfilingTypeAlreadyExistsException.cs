using System;
using System.Runtime.Serialization;

namespace Chronos
{
    public class ProfilingTypeAlreadyExistsException : ProfilerException
    {
        public ProfilingTypeAlreadyExistsException(Guid profilingTypeUid)
            : base(string.Format("Profiling Type with uid '{0}' already exists", profilingTypeUid))
        {
            ProfilingTypeUid = profilingTypeUid;
        }

        public ProfilingTypeAlreadyExistsException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
            
        }

        public Guid ProfilingTypeUid { get; private set; }
    }
}
