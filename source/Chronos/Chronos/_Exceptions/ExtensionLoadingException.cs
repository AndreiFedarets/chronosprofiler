﻿using System;
using System.Runtime.Serialization;

namespace Chronos
{
    public class ExtensionLoadingException : ProfilerException
    {
        /// <summary>
        /// Initializes a new instance of the Chronos.ExtensionLoadingException class with serialized data.
        /// </summary>
        /// <param name="info">The object that holds the serialized object data.</param>
        /// <param name="context">The contextual information about the source or destination.</param>
        protected ExtensionLoadingException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
            
        }
        
        /// <summary>
        /// Initializes a new instance of the Chronos.ExtensionLoadingException class with a specified error message.
        /// </summary>
        /// <param name="message">A message that describes the error.</param>
        public ExtensionLoadingException(string message)
            : base(message)
        {
            
        }
       
        /// <summary>
        /// Initializes a new instance of the Chronos.ExtensionLoadingException class with
        /// a specified error message and a reference to the inner exception that is
        /// the cause of this exception.
        /// </summary>
        /// <param name="message">A message that describes the error.</param>
        /// <param name="innerException">The exception that is the cause of the current exception.</param>
        public ExtensionLoadingException(string message, Exception innerException)
            : base(message, innerException)
        {

        }
    }
}
