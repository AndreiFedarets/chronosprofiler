using System.Windows.Media;

namespace Rhiannon.Windows.Presentation.Documents
{
	public interface IDocument
	{
		string Title { get; }

		ImageSource Icon { get; }

		bool ReadOnly { get; }

		string Id { get; }

		IViewBase View { get; }

		bool IsActive { get; set; }

		object UnderlyingEntity { get; }

		void Close();
	}
}
