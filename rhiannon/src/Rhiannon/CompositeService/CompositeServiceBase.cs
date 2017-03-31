using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;

namespace Rhiannon.CompositeService
{
	public class CompositeServiceBase : ICompositeService
	{
		private readonly IDictionary<int, ServiceOperationGroup> _operations;

		public CompositeServiceBase()
		{
			_operations = new Dictionary<int, ServiceOperationGroup>();
		}

		public void RegisterOperation(IServiceOperation operation, int operationCode, int order)
		{
			ServiceOperationGroup operationGroup;
			if (!_operations.TryGetValue(operationCode, out operationGroup))
			{
				operationGroup = new ServiceOperationGroup();
			}
			operationGroup.Register(operation, order);
		}

		public void RegisterOperation(IServiceOperation operation, int operationCode)
		{
			RegisterOperation(operation, operationCode, int.MaxValue);
		}

		public void Execute(int operationCode, OperationContext context)
		{
			ServiceOperationGroup operationGroup = _operations[operationCode];
			operationGroup.Validate(context);
			operationGroup.Execute(context);
		}

		protected void ExecuteCurrentOperation(OperationContext context)
		{
			StackTrace stackTrace = new StackTrace(false);
			StackFrame stackFrame = stackTrace.GetFrame(1);
			MethodBase sourceMethod = stackFrame.GetMethod();
			object[] attributes = sourceMethod.GetCustomAttributes(typeof (ServiceOperationAttribute), true);
			ServiceOperationAttribute attribute = (ServiceOperationAttribute) attributes.FirstOrDefault();
			if (attribute == null)
			{
				throw new InvalidServiceOperationException();
			}
			Execute(attribute.OperationCode, context);
		}
	}
}
