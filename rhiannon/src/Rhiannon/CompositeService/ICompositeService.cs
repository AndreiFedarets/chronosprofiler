
namespace Rhiannon.CompositeService
{
	public interface ICompositeService
	{
		void RegisterOperation(IServiceOperation operation, int order);

		void Execute(int operationCode, OperationContext context);
	}
}
