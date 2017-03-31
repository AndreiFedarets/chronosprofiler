namespace Rhiannon.Ribbon.Client
{
	public interface IControlCallbackCollection
	{
		IControlCallback this[string id] { get; }
	}
}
