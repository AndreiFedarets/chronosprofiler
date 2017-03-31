using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Media;
using Rhiannon.Extensions;

namespace Rhiannon.Windows.Presentation.Documents
{
	public class DocumentCollection : ObservableCollection<IDocument>, IDocumentCollection
	{
		private Document _activeDocument;

		public IDocument ActiveDocument
		{
			get { return _activeDocument; }
			set
			{
				if (value == null)
				{
					value = this.FirstOrDefault();
				}
				if (value == null)
				{
					return;
				}
				if (_activeDocument == value)
				{
					return;
				}
				if (this[value.Id] == null)
				{
					return;
				}
				if (_activeDocument != null)
				{
					_activeDocument.SetIsActiveProperty(false);
				}
				((Document)value).SetIsActiveProperty(true);
				_activeDocument = (Document) value;
				base.OnPropertyChanged(new PropertyChangedEventArgs("ActiveDocument"));
			}
		}

		public IDocument this[string id]
		{
			get
			{
				IDocument document = this.FirstOrDefault(x => string.Equals(x.Id, id, StringComparison.InvariantCulture));
				return document;
			}
		}

		public event EventHandler<DocumentEventArgs> DocumentOpened;

		public event EventHandler<DocumentEventArgs> DocumentClosed;

		public IDocument Add(IViewBase view)
		{
			view.ApplyResources();
			return Add(view.Title, view.Uid, view);
		}

		public IDocument Add(IViewBase view, bool readOnly, bool activate)
		{
			view.ApplyResources();
			return Add(view.Title, view.Icon, view.Uid, view, readOnly, activate);
		}

		public IDocument Add(IViewBase view, bool readOnly, bool activate, object underlyingEntity)
		{
			view.ApplyResources();
			return Add(view.Title, view.Icon, view.Uid, view, readOnly, activate, underlyingEntity);
		}

		public IDocument Add(string header, string id, IViewBase view)
		{
			view.ApplyResources();
			return Add(header, view.Icon, id, view, false, true);
		}

		public IDocument Add(string title, ImageSource icon, string id, IViewBase view, bool readOnly, bool activate)
		{
			if (this[id] != null)
			{
				throw new ArgumentException("id");
			}
			if (!this.Any())
			{
				activate = true;
			}
			Document document = new Document(title, icon, id, view, readOnly, null, this);
			Add(document);
			if (activate)
			{
				document.IsActive = true;
			}
			DocumentOpened.SafeInvoke(this, new DocumentEventArgs(document));
			return document;
		}

		public IDocument Add(string header, string id, IViewBase view, object underlyingEntity)
		{
			return Add(header, view.Icon, id, view, false, true, underlyingEntity);
		}

		public IDocument Add(string header, ImageSource icon, string id, IViewBase view, bool readOnly, bool activate, object underlyingEntity)
		{
			if (this[id] != null)
			{
				throw new ArgumentException("id");
			}
			if (!this.Any())
			{
				activate = true;
			}
			Document document = new Document(header, icon, id, view, readOnly, underlyingEntity, this);
			Add(document);
			if (activate)
			{
				document.IsActive = true;
			}
			DocumentOpened.SafeInvoke(this, new DocumentEventArgs(document));
			return document;
		}

		public void Close(string id)
		{
			IDocument document = this[id];
			if (document != null)
			{
				Remove(document);
				document.View.Dispose();
                if (ActiveDocument == document)
                {
                    ActiveDocument = this.FirstOrDefault();
                }
			}
			DocumentClosed.SafeInvoke(this, new DocumentEventArgs(document));
		}

		public void Activate(string id)
		{
			IDocument document = this[id];
			if (document != null)
			{
				document.IsActive = true;
			}
		}
	}
}
