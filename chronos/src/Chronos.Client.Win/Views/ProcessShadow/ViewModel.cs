using System.Windows.Input;
using Chronos.Client.Win.Commands;
using Chronos.Host;
using Rhiannon.Threading;
using Rhiannon.Windows.Presentation;
using Rhiannon.Windows.Presentation.Commands;
using Rhiannon.Windows.Presentation.Documents;
using Chronos.Core;
using Chronos.Daemon;
using System;

namespace Chronos.Client.Win.Views.ProcessShadow
{
	public class ViewModel : ViewModelBase, IViewModel
	{
		private readonly IViewsManager _viewsManager;
	    private readonly IProcessShadowNavigator _processShadowNavigator;
		private readonly ISession _session;
	    private readonly ITaskFactory _taskFactory;
	    private readonly IProcessShadow _processShadow;
	    private readonly IDaemonApplication _daemonApplication;
	    private SessionState _sessionState;

		public ViewModel(IViewsManager viewsManager, IDocumentCollection documents, IProcessShadowNavigator processShadowNavigator,
            ITaskFactory taskFactory, ISession session, IProcessShadow processShadow)
		{
			Documents = documents;
			_viewsManager = viewsManager;
		    _processShadowNavigator = processShadowNavigator;
		    _taskFactory = taskFactory;
		    _processShadow = processShadow;
			_session = session;
		    _daemonApplication = session.StartDecoding();
		}

		public IDocumentCollection Documents { get; private set; }

	    public SessionState SessionState
	    {
            get { return _sessionState; }
            private set { SetPropertyAndNotifyChanged(() => SessionState, ref _sessionState, value); }
	    }

		public ICommand CloseDocumentCommand { get; private set; }
        public ICommand ReloadProcessShadowCommand { get; private set; }
        public ICommand StopProfilingCommand { get; private set; }
        public ICommand ContinueProfilingCommand { get; private set; }
        public ICommand PauseProfilingCommand { get; private set; }

		protected override void InitializeInternal()
		{
			base.InitializeInternal();
		    CloseDocumentCommand = new CloseDocumentCommand();
            ReloadProcessShadowCommand = new ReloadProcessShadowCommand(_processShadow, _taskFactory);
            StopProfilingCommand = new StopProfilingCommand(_daemonApplication, _taskFactory);
            ContinueProfilingCommand = new ContinueProfilingCommand(_daemonApplication);
            PauseProfilingCommand = new PauseProfilingCommand(_daemonApplication);
			IViewBase view;
		    IDocument document;
            //view = _viewsManager.Resolve(ViewNames.BaseViews.PerformanceCounters);
            //Documents.Add(view, true, false);
            view = _viewsManager.Resolve(ViewNames.BaseViews.ThreadTrace);
            document = Documents.Add(view, true, true);
            _processShadowNavigator.Initialize(document);
            view = _viewsManager.Resolve(ViewNames.UnitViews.Exceptions);
            Documents.Add(view, true, false);
			view = _viewsManager.Resolve(ViewNames.UnitViews.AppDomains);
			Documents.Add(view, true, false);
			view = _viewsManager.Resolve(ViewNames.UnitViews.Assemblies);
			Documents.Add(view, true, false);
			view = _viewsManager.Resolve(ViewNames.UnitViews.Modules);
			Documents.Add(view, true, false);
			view = _viewsManager.Resolve(ViewNames.UnitViews.Classes);
			Documents.Add(view, true, false);
			view = _viewsManager.Resolve(ViewNames.UnitViews.Functions);
			Documents.Add(view, true, false);
            view = _viewsManager.Resolve(ViewNames.UnitViews.SqlRequests);
            Documents.Add(view, true, false);
			view = _viewsManager.Resolve(ViewNames.UnitViews.Threads);
            Documents.Add(view, true, false);
            //view = _viewsManager.Resolve(ViewNames.UnitViews.Callstacks);
            //Documents.Add(view, true, false);
		}

		public override void Dispose()
		{
			base.Dispose();
			Action close = _session.Close;
		    close.BeginInvoke(null, null);
		}
	}
}
