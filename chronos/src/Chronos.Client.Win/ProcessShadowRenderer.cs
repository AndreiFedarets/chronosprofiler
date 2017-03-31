using Chronos.Client.Win.Views;
using Chronos.Core;
using Chronos.Host;
using Rhiannon.Windows.Presentation;
using Rhiannon.Windows.Presentation.Documents;
using Chronos.Daemon;

namespace Chronos.Client.Win
{
	public class ProcessShadowRenderer : IProcessShadowRenderer
	{
		private readonly IDocumentCollection _documents;
		private readonly IViewsManager _viewsManager;

		public ProcessShadowRenderer(IDocumentCollection documents, IViewsManager viewsManager)
		{
			_documents = documents;
			_viewsManager = viewsManager;
		}

		public void Render(ISession session)
		{
			string id = session.Token.ToString();
			IDocument document = _documents[id];
			if (document == null)
			{
				IDaemonApplication daemonApplication = session.StartDecoding();
				IProcessShadow processShadow = daemonApplication.ProcessShadow;
				IViewBase view = _viewsManager.Resolve(ViewNames.ProcessShadow.WinApplication, processShadow, session);
				string header = processShadow.ProcessInfo.ProcessName;
				_documents.Add(header, id, view);
			}
			_documents.Activate(id);
		}
	}
}
