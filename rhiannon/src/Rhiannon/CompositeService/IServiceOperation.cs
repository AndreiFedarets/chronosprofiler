namespace Rhiannon.CompositeService
{
	public interface IServiceOperation
	{
		void Rollback(OperationContext context);

		OperationBehavior Execute(OperationContext context);

		void Validate(OperationContext context);
	}
}
