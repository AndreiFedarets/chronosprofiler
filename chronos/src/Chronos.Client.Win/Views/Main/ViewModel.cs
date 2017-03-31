using System.Windows.Input;
using Chronos.Communication.Remoting;
using Rhiannon.Windows.Presentation;
using Rhiannon.Windows.Presentation.Commands;
using Rhiannon.Windows.Presentation.Documents;

namespace Chronos.Client.Win.Views.Main
{
	public class ViewModel : ViewModelBase, IViewModel
	{
		private readonly IClientApplication _application;
		private readonly IRemotingExecutor _remotingExecutor;
		private readonly IViewsManager _viewsManager;
		private IDocument _homeDocument;
		private IDocument _optionsDocument;
		private string _stateMessage;

		public ViewModel(IDocumentCollection documents, IViewsManager viewsManager, IRemotingExecutor remotingExecutor, IClientApplication application)
		{
			Documents = documents;
			_viewsManager = viewsManager;
			_remotingExecutor = remotingExecutor;
			_application = application;
			_remotingExecutor.ConnectionFailed += OnRemotingExecutorConnectionFailed;
			StateMessage = string.Format("Connected to {0}", _application.Host.MachineName);
		}

		public void OnRemotingExecutorConnectionFailed()
		{
			StateMessage = "Connection to server is failed. Please fix it and restart application";
		}

		public IDocumentCollection Documents { get; private set; }

		public ICommand CloseDocumentCommand { get; private set; }

		public ICommand OpenHomeCommand { get; private set; }

		public ICommand OpenOptionsCommand { get; private set; }

		public string StateMessage
		{
			get { return _stateMessage; }
			set { SetPropertyAndNotifyChanged(() => StateMessage, ref _stateMessage, value); }
		}

		protected override void InitializeInternal()
		{
			base.InitializeInternal();
			IViewBase homeGroupView = _viewsManager.Resolve(ViewNames.Groups.Home);
			_homeDocument = Documents.Add(homeGroupView, true, true);

			IViewBase optionsGroupView = _viewsManager.Resolve(ViewNames.BaseViews.Options);
			_optionsDocument = Documents.Add(optionsGroupView, true, false);

			OpenHomeCommand = new SyncCommand(OpenHome);
			OpenOptionsCommand = new SyncCommand(OpenOptions);

            CloseDocumentCommand = new SyncCommand<IDocument>(CloseDocument, CanCloseDocument);
		}

		private void OpenHome()
		{
			Documents.ActiveDocument = _homeDocument;
		}

		private void OpenOptions()
		{
			Documents.ActiveDocument = _optionsDocument;
		}

		private void CloseDocument(IDocument document)
		{
			document.Close();
		}

        private bool CanCloseDocument(IDocument document)
        {
            return document != null && !document.ReadOnly;
        }
	}
}
