using System;
using System.Runtime.Serialization;

namespace Chronos
{
    public class ProductivityNotFoundException : ProfilerException
    {
        public ProductivityNotFoundException(Guid uid)
            : base(string.Format("Extension with uid '{0}' was not found", uid))
        {
            ProductivityUid = uid;
        }

        public ProductivityNotFoundException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }

        public Guid ProductivityUid { get; private set; }
    }
}
