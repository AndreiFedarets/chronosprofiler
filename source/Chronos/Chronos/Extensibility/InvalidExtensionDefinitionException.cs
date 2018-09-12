using System;
using System.Runtime.Serialization;

namespace Chronos.Extensibility
{
    public class InvalidExtensionDefinitionException : ProfilerException
    {
        /// <summary>
        /// Initializes a new instance of the Chronos.Extensibility.InvalidExtensionDefinitionException class with serialized data.
        /// </summary>
        /// <param name="info">The object that holds the serialized object data.</param>
        /// <param name="context">The contextual information about the source or destination.</param>
        protected InvalidExtensionDefinitionException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
            
        }
        
        /// <summary>
        /// Initializes a new instance of the Chronos.Extensibility.InvalidExtensionDefinitionException class with a specified error message.
        /// </summary>
        /// <param name="message">A message that describes the error.</param>
        /// <param name="innerException">Inner exception.</param>
        public InvalidExtensionDefinitionException(string message, Exception innerException)
            : base(message, innerException)
        {
            
        }
    }
}
