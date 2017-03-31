using System;
using System.Runtime.Serialization;

namespace Rhiannon.CompositeService
{
	[Serializable]
	public class CompositeServiceException : Exception
	{
		public CompositeServiceException(SerializationInfo serializationInfo, StreamingContext streamingContext)
			: base(serializationInfo, streamingContext)
		{
			
		}

		public CompositeServiceException(string message)
			: base(message)
		{
			
		}

		public CompositeServiceException(string message, Exception innerException)
			: base(message, innerException)
		{
			
		}

		public CompositeServiceException()
		{
			
		}
	}
}
