using Rhiannon.Windows.Presentation;

namespace Chronos.Client.Win.Views.ThreadTrace
{
	public class ViewActivator : ViewActivatorBase<IView, View, IViewModel, ViewModel>
	{
		public ViewActivator()
			: base(ViewNames.BaseViews.ThreadTrace)
		{
		}

		//public override IViewBase Activate(IContainer container, params object[] args)
		//{
		//    IThreadTrace threadTrace = (IThreadTrace)(args[0]);
		//    container.RegisterInstance(threadTrace);
		//    return base.Activate(container, args);
		//}
	}
}
