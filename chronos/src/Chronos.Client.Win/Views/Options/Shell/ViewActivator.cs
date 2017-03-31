using Rhiannon.Windows.Presentation;

namespace Chronos.Client.Win.Views.Options.Shell
{
	public class ViewActivator : ViewActivatorBase<IView, View, IViewModel, ViewModel>
	{
		public ViewActivator()
			: base(ViewNames.OptionViews.Shell)
		{
		}
	}
}
