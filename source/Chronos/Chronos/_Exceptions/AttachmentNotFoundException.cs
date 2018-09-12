using System;
using System.Runtime.Serialization;

namespace Chronos
{
    public class AttachmentNotFoundException : ProfilerException
    {
        public AttachmentNotFoundException(Guid targetUid)
            : base(string.Format("Attachment with uid '{0}' was not found", targetUid))
        {
            TargetUid = targetUid;
        }

        public AttachmentNotFoundException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
            
        }

        public Guid TargetUid { get; private set; }
    }
}
