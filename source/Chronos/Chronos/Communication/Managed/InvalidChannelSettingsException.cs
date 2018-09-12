using System.Runtime.Serialization;

namespace Chronos.Communication.Managed
{
    public class InvalidChannelSettingsException : CommunicationException
    {
        /// <summary>
        /// Initializes a new instance of the Chronos.Communication.Remote.InvalidChannelSettingsException class with serialized data.
        /// </summary>
        /// <param name="info">The object that holds the serialized object data.</param>
        /// <param name="context">The contextual information about the source or destination.</param>
        public InvalidChannelSettingsException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
            
        }
        
        /// <summary>
        /// Initializes a new instance of the Chronos.Communication.Remote.InvalidChannelSettingsException class with a specified error message.
        /// </summary>
        /// <param name="message">A message that describes the error.</param>
        public InvalidChannelSettingsException(string message)
            : base(message)
        {
            
        }
       
    }
}
