using System;
using System.Runtime.Serialization;

namespace Chronos.Communication.Managed
{
    /// <summary>
    /// Base exception for communication exceptions
    /// </summary>
    public class CommunicationException : ProfilerException
    {
        /// <summary>
        /// Initializes a new instance of the Chronos.Communication.Remote.CommunicationException class.
        /// </summary>
        public CommunicationException()
        {
            
        }
       
        /// <summary>
        /// Initializes a new instance of the Chronos.Communication.Remote.CommunicationException class with serialized data.
        /// </summary>
        /// <param name="info">The object that holds the serialized object data.</param>
        /// <param name="context">The contextual information about the source or destination.</param>
        public CommunicationException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
            
        }
        
        /// <summary>
        /// Initializes a new instance of the Chronos.Communication.Remote.CommunicationException class with a specified error message.
        /// </summary>
        /// <param name="message">A message that describes the error.</param>
        public CommunicationException(string message)
            : base(message)
        {
            
        }
       
        /// <summary>
        /// Initializes a new instance of the Chronos.Communication.Remote.CommunicationException class with
        /// a specified error message and a reference to the inner exception that is
        /// the cause of this exception.
        /// </summary>
        /// <param name="message">A message that describes the error.</param>
        /// <param name="innerException">The exception that is the cause of the current exception.</param>
        public CommunicationException(string message, Exception innerException)
            : base(message, innerException)
        {

        }
    }
}
