using System;
using System.Runtime.Serialization;

namespace Chronos
{
    public class FrameworkAlreadyExistsException : ProfilerException
    {
        public FrameworkAlreadyExistsException(Guid frameworkUid)
            : base(string.Format("Framework with uid '{0}' already exists", frameworkUid))
        {
            FrameworkUid = frameworkUid;
        }

        public FrameworkAlreadyExistsException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
            
        }

        public Guid FrameworkUid { get; private set; }
    }
}
