
namespace Rhiannon.Ribbon
{
	public interface IControlPresenter
	{
		string Label { get; set; }

		bool Enabled { get; set; }

		bool Visible { get; set; }
	}
}
