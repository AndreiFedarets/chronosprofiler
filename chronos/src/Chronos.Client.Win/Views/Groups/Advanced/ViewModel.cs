using System;
using System.Windows.Input;
using Rhiannon.Presentation;
using Rhiannon.Presentation.Commands;
using Rhiannon.Presentation.Documents;

namespace Chronos.Client.Win.Views.Groups.Advanced
{
	public class ViewModel : ViewModelBase, IViewModel
	{
		private readonly IViewsManager _viewsManager;

		public ViewModel(IAdvancedDocumentCollection advancedDocuments, IViewsManager viewsManager)
		{
			Documents = advancedDocuments;
			_viewsManager = viewsManager;
		}

		public IAdvancedDocumentCollection Documents { get; private set; }

		public ICommand CloseDocumentCommand { get; private set; }

		protected override void InitializeInternal()
		{
			base.InitializeInternal();
			//IViewBase homePageView = _viewsManager.Resolve(ViewNames.Pages.ConfigurationsPage);
			//Documents.Add(homePageView, true, true);
			//IViewBase sessionsPageView = _viewsManager.Resolve(ViewNames.Pages.SessionsPage);
			//Documents.Add(sessionsPageView, true, false);
			//IViewBase optionsPageView = _viewsManager.Resolve(ViewNames.BaseViews.Options);
			//Documents.Add(optionsPageView, true, false);

			CloseDocumentCommand = new SyncCommand<IDocument>(CloseDocument);
		}

		private void CloseDocument(IDocument document)
		{
			IDisposable disposable = document.UnderlyingEntity as IDisposable;
			if (disposable != null)
			{
				disposable.Dispose();
			}
			Documents.Remove(document.Id);
		}
	}
}
