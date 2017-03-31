namespace Rhiannon.CompositeService
{
	public class ServiceOperationBase : IServiceOperation
	{
		public virtual void Rollback(OperationContext context)
		{
			
		}

		public virtual OperationBehavior Execute(OperationContext context)
		{
			return OperationBehavior.Continue;
		}

		public virtual void Validate(OperationContext context)
		{

		}
	}
}
