using System;

namespace Chronos.Communication.Managed
{
    public class ChannelRegistrationException : CommunicationException
    {
        /// <summary>
        /// Initializes a new instance of the Chronos.Communication.Remote.ChannelRegistrationException class with a specified error message.
        /// </summary>
        /// <param name="message">A message that describes the error.</param>
        public ChannelRegistrationException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the Chronos.Communication.Remote.ChannelRegistrationException class with
        /// a specified error message and a reference to the inner exception that is
        /// the cause of this exception.
        /// </summary>
        /// <param name="message">A message that describes the error.</param>
        /// <param name="innerException">The exception that is the cause of the current exception.</param>
        public ChannelRegistrationException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
