using System;
using System.Runtime.Serialization;

namespace Chronos
{
    public class ApplicationStartupException : Exception
    {
        public ApplicationStartupException(string message)
            : base(message)
        {
        }

        public ApplicationStartupException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
