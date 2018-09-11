using System;
using System.Runtime.Serialization;

namespace Chronos
{
    public class ExtensionNotFoundException : ProfilerException
    {
        public ExtensionNotFoundException(Guid uid)
            : base(string.Format("Extension with uid '{0}' was not found", uid))
        {
            ExtensionUid = uid;
        }

        public ExtensionNotFoundException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }

        public Guid ExtensionUid { get; private set; }
    }
}
