using System.Windows.Media;

namespace Rhiannon.Windows.Presentation.Documents
{
	internal class Document : PropertyChangedNotifier, IDocument
	{
		private readonly DocumentCollection _documents;
		private bool _isActive;

		private string _title;
		private ImageSource _icon;

		public Document(string title, ImageSource icon, string id, IViewBase view, bool readOnly, object underlyingEntity, DocumentCollection documents)
		{
			Title = title;
			Icon = icon;
			Id = id;
			View = view;
			ReadOnly = readOnly;
			UnderlyingEntity = underlyingEntity;
			_documents = documents;
		}

		public bool IsActive
		{
			get { return _isActive; }
			set
			{
				if (value)
                {
                    _documents.ActiveDocument = this;
                }
				else
				{
				    _documents.ActiveDocument = null;
				    _documents.ActiveDocument = this;
				}
			}
		}

		public ImageSource Icon
		{
			get { return _icon; }
			set { SetPropertyAndNotifyChanged(() => Icon, ref _icon, value); }
		}

		public string Title
		{
			get { return _title; }
			set { SetPropertyAndNotifyChanged(() => Title, ref _title, value); }
		}

		public string Id { get; private set; }

		public IViewBase View { get; private set; }

		public bool ReadOnly { get; private set; }

		public object UnderlyingEntity { get; private set; }

		public void Close()
		{
			_documents.Close(Id);
		}

		internal void SetIsActiveProperty(bool value)
		{
			SetPropertyAndNotifyChanged(() => IsActive, ref _isActive, value);
		}
	}
}
