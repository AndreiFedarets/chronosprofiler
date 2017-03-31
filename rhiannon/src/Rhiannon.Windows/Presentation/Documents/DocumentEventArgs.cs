using System;

namespace Rhiannon.Windows.Presentation.Documents
{
	public class DocumentEventArgs : EventArgs
	{
		public DocumentEventArgs(IDocument document)
		{
			Document = document;
		}

		public IDocument Document { get; private set; }
	}
}
