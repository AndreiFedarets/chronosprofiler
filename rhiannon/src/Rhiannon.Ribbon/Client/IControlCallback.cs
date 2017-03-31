namespace Rhiannon.Ribbon.Client
{
	public interface IControlCallback
	{
		bool GetVisible();

		bool GetEnabled();

		string GetLabel();

		void ControlLoaded(IControl control);

		string ControlId { get; }
	}
}
