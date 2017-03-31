using Rhiannon.Presentation;

namespace Chronos.Client.Win.Views.Options.ProfilingEvents
{
	public class ViewActivator : ViewActivatorBase<IView, View, IViewModel, ViewModel>
	{
		public ViewActivator()
			: base(ViewNames.OptionViews.ProfilingEvents)
		{
		}
	}
}
