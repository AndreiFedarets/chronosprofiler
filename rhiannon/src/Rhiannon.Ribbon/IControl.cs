namespace Rhiannon.Ribbon
{
	public interface IControl
	{
		string Id { get; }

		bool Enabled { get; }

		bool Visible { get; }

		string Label { get; }

		void Invalidate();
	}
}
