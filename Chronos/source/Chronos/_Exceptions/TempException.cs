using System;
using System.Runtime.Serialization;

namespace Chronos
{
    [Serializable]
    public class TempException : ProfilerException
    {
        public TempException()
        {
        }

        public TempException(string message)
            : base(message)
        {
        }

        public TempException(Exception exception)
            : base(string.Empty, exception)
        {
        }

        protected TempException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
            
        }
    }
}
