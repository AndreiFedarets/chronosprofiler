using Chronos.Core;
using Rhiannon.Threading;
using Rhiannon.Windows.Presentation;
using Rhiannon.Windows.Presentation.Documents;
using Chronos.Host;

namespace Chronos.Client.Win.Views.ProcessShadow.WinApplication
{
	public class ViewModel : ProcessShadow.ViewModel, IViewModel
	{
        public ViewModel(IViewsManager viewsManager, IDocumentCollection documents, IProcessShadowNavigator processShadowNavigator,
            ITaskFactory taskFactory, ISession session, IProcessShadow processShadow)
            : base(viewsManager, documents, processShadowNavigator, taskFactory, session, processShadow)
        {
        }
	}
}
