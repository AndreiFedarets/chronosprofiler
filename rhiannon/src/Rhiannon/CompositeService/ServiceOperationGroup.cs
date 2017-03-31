using System.Collections.Generic;

namespace Rhiannon.CompositeService
{
	internal class ServiceOperationGroup
	{
		private readonly List<KeyValuePair<int, IServiceOperation>> _operations;

		public ServiceOperationGroup()
		{
			_operations = new List<KeyValuePair<int, IServiceOperation>>();
		}

		public void Register(IServiceOperation operation, int order)
		{
 			_operations.Add(new KeyValuePair<int, IServiceOperation>(order, operation));
		}

		public void Execute(OperationContext context)
		{
			foreach (KeyValuePair<int, IServiceOperation> operation in _operations)
			{
				operation.Value.Execute(context);
			}
		}

		public void Validate(OperationContext context)
		{
			foreach (KeyValuePair<int, IServiceOperation> operation in _operations)
			{
				operation.Value.Validate(context);
			}
		}
	}
}
