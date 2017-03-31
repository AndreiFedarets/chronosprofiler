using Chronos.Core;
using Chronos.Host;
using Rhiannon.Unity;
using Rhiannon.Windows.Presentation;
using Rhiannon.Windows.Presentation.Documents;

namespace Chronos.Client.Win.Views.ProcessShadow
{
	public class ViewActivator<TViewInterface, TViewImplementation, TViewModelInterface, TViewModelImplementation>
		: ViewActivatorBase<TViewInterface, TViewImplementation, TViewModelInterface, TViewModelImplementation>
		where TViewImplementation : TViewInterface
		where TViewModelImplementation : TViewModelInterface
		where TViewInterface : IViewBase
		where TViewModelInterface : IViewModelBase
	{
		public ViewActivator(string viewName)
			: base(viewName)
		{
		}

		public override IViewBase Activate(IContainer container, params object[] args)
		{
			IProcessShadow processShadow = (IProcessShadow) args[0];
			ISession session = (ISession)args[1];
			IViewsManager viewsManager = container.Resolve<IViewsManager>();
			IEventNameFormatter eventNameFormatter = new EventNameFormatter(processShadow);
			IDocumentCollection documents = new DocumentCollection();
			IProcessShadowNavigator processShadowNavigator = new ProcessShadowNavigator(processShadow);
			IContainer childContainer = container.CreateChildContainer();
			IViewsManager childViewsManager = viewsManager.CreateChildViewsManager(childContainer);
			childContainer.RegisterInstance(childViewsManager);
			childContainer.RegisterInstance(processShadow);
			childContainer.RegisterInstance(eventNameFormatter);
            childContainer.RegisterInstance(documents);
            childContainer.RegisterInstance(processShadowNavigator);
			childContainer.RegisterInstance(session);
			return base.Activate(childContainer, args);
		}
	}
}
