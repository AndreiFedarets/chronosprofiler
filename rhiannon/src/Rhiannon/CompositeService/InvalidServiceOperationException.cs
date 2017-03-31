using System;
using System.Runtime.Serialization;

namespace Rhiannon.CompositeService
{
	public class InvalidServiceOperationException : CompositeServiceException
	{
		public InvalidServiceOperationException(SerializationInfo serializationInfo, StreamingContext streamingContext)
			: base(serializationInfo, streamingContext)
		{
		}

		public InvalidServiceOperationException(string message)
			: base(message)
		{
		}

		public InvalidServiceOperationException(string message, Exception innerException)
			: base(message, innerException)
		{
		}

		public InvalidServiceOperationException()
		{
			
		}
	}
}
