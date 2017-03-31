using Rhiannon.Windows.Presentation;

namespace Chronos.Client.Win.Views.Units.Classes
{
	public class ViewActivator : ViewActivatorBase<IView, View, IViewModel, ViewModel>
	{
		public ViewActivator()
			: base(ViewNames.UnitViews.Classes)
		{
		}
	}
}
