using System;
using System.Runtime.Serialization;

namespace Chronos
{
    public class FrameworkInitializationException : ProfilerException
    {
        public FrameworkInitializationException(Guid frameworkUid, Exception exception)
            : base(string.Format("Error during initialization of framework with uid '{0}'", frameworkUid), exception)
        {
            FrameworkUid = frameworkUid;
        }

        protected FrameworkInitializationException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
            
        }

        public Guid FrameworkUid { get; private set; }
    }
}
