using System;

namespace Rhiannon.CompositeService
{
	public class ServiceOperationAttribute : Attribute
	{
		public ServiceOperationAttribute(int operationCode)
		{
			OperationCode = operationCode;
		}

		public int OperationCode { get; private set; }
	}
}
