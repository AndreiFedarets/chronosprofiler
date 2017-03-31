using System;
using System.Runtime.Serialization;

namespace Chronos
{
    public class FrameworkNotFoundException : ProfilerException
    {
        public FrameworkNotFoundException(Guid frameworkUid)
            : base(string.Format("Framework with uid '{0}' was not found", frameworkUid))
        {
            FrameworkUid = frameworkUid;
        }

        public FrameworkNotFoundException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
            
        }

        public Guid FrameworkUid { get; private set; }
    }
}
