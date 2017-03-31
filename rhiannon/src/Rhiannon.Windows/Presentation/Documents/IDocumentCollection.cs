using System;
using System.Collections.Generic;
using System.Windows.Media;

namespace Rhiannon.Windows.Presentation.Documents
{
	public interface IDocumentCollection : IEnumerable<IDocument>
	{
		IDocument Add(IViewBase view);

		IDocument Add(IViewBase view, bool readOnly, bool activate);

		IDocument Add(IViewBase view, bool readOnly, bool activate, object underlyingEntity);

		IDocument Add(string header, string id, IViewBase view);

		IDocument Add(string header, ImageSource icon, string id, IViewBase view, bool readOnly, bool activate);

		IDocument Add(string header, string id, IViewBase view, object underlyingEntity);

		IDocument Add(string header, ImageSource icon, string id, IViewBase view, bool readOnly, bool activate, object underlyingEntity);

		IDocument this[string id] { get; }

		void Close(string id);

		void Activate(string id);

		IDocument ActiveDocument { get; set; }

		void Clear();

		event EventHandler<DocumentEventArgs> DocumentOpened;

		event EventHandler<DocumentEventArgs> DocumentClosed;
	}
}
