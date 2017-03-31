
namespace Chronos.Client.Win.Views.ProcessShadow.WinApplication
{
	public class ViewActivator : ViewActivator<IView, View, IViewModel, ViewModel>
	{
		public ViewActivator()
			: base(ViewNames.ProcessShadow.WinApplication)
		{
		}

		//public override IViewBase Activate(IContainer container, params object[] args)
		//{
		//    IProcessShadow processShadow = (IProcessShadow) args[0];
		//    IViewsManager viewsManager = container.Resolve<IViewsManager>();
		//    IEventNameFormatter eventNameFormatter = new EventNameFormatter(processShadow);
		//    IDocumentCollection documents = new DocumentCollection();
		//    IContainer childContainer = container.CreateChildContainer();
		//    IViewsManager childViewsManager = viewsManager.CreateChildViewsManager(childContainer);
		//    childContainer.RegisterInstance(childViewsManager);
		//    childContainer.RegisterInstance(processShadow);
		//    childContainer.RegisterInstance(eventNameFormatter);
		//    childContainer.RegisterInstance(documents);
		//    return base.Activate(childContainer, args);
		//}
	}
}
