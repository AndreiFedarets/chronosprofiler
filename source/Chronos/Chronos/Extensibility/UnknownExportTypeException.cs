using System.Runtime.Serialization;

namespace Chronos.Extensibility
{
    public class UnknownExportTypeException : ProfilerException
    {
        /// <summary>
        /// Initializes a new instance of the Chronos.Extensibility.UnknownExportTypeException class with serialized data.
        /// </summary>
        /// <param name="info">The object that holds the serialized object data.</param>
        /// <param name="context">The contextual information about the source or destination.</param>
        protected UnknownExportTypeException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
            
        }
        
        /// <summary>
        /// Initializes a new instance of the Chronos.Extensibility.UnknownExportTypeException class with a specified error message.
        /// </summary>
        /// <param name="message">A message that describes the error.</param>
        public UnknownExportTypeException(string message)
            : base(message)
        {
            
        }
    }
}
