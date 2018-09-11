using System.Runtime.Serialization;

namespace Chronos.Communication.Managed
{
    public class InvalidConnectionSettingsException : CommunicationException
    {
        /// <summary>
        /// Initializes a new instance of the Chronos.Communication.Remote.InvalidConnectionSettingsException class with serialized data.
        /// </summary>
        /// <param name="info">The object that holds the serialized object data.</param>
        /// <param name="context">The contextual information about the source or destination.</param>
        public InvalidConnectionSettingsException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
            
        }
        
        /// <summary>
        /// Initializes a new instance of the Chronos.Communication.Remote.InvalidConnectionSettingsException class with a specified error message.
        /// </summary>
        /// <param name="message">A message that describes the error.</param>
        public InvalidConnectionSettingsException(string message)
            : base(message)
        {
            
        }
       
    }
}
