using System;
using System.Runtime.Serialization;

namespace Chronos
{
    public class ApplicationExtensionNotFoundException : ProfilerException
    {
        public ApplicationExtensionNotFoundException(Guid extensionUid)
            : base(string.Format("Application extension with uid '{0}' was not found", extensionUid))
        {
            ExtensionUid = extensionUid;
        }

        public ApplicationExtensionNotFoundException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
            
        }

        public Guid ExtensionUid { get; private set; }
    }
}
