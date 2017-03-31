using System;
using System.Runtime.Serialization;

namespace Rhiannon.CompositeService
{
	public class InvalidOperationContextException : CompositeServiceException
	{
		public InvalidOperationContextException(SerializationInfo serializationInfo, StreamingContext streamingContext)
			: base(serializationInfo, streamingContext)
		{
		}

		public InvalidOperationContextException(string message) : base(message)
		{
		}

		public InvalidOperationContextException(string message, Exception innerException)
			: base(message, innerException)
		{
		}
	}
}
