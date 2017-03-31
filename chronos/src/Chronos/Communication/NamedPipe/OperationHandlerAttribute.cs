using System;

namespace Chronos.Communication.NamedPipe
{
	public class OperationHandlerAttribute : Attribute
	{
		public OperationHandlerAttribute(long operationCode)
		{
			OperationCode = operationCode;
		}

		public long OperationCode { get; private set; }
	}
}
