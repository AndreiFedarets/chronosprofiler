using System;
using System.Runtime.Serialization;

namespace Chronos
{
    [Serializable]
    public class RemoteApplicationUnavailableException : Exception
    {
        public RemoteApplicationUnavailableException(Exception exception)
            : base(string.Empty, exception)
        {
            
        }

        public RemoteApplicationUnavailableException(string message)
            : base(message)
        {
        }

        public RemoteApplicationUnavailableException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
