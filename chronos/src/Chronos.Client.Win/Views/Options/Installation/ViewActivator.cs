using Rhiannon.Windows.Presentation;

namespace Chronos.Client.Win.Views.Options.Installation
{
	public class ViewActivator : ViewActivatorBase<IView, View, IViewModel, ViewModel>
	{
		public ViewActivator()
			: base(ViewNames.OptionViews.Installation)
		{
		}
	}
}
