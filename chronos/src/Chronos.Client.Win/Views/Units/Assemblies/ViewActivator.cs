using Rhiannon.Windows.Presentation;

namespace Chronos.Client.Win.Views.Units.Assemblies
{
	public class ViewActivator : ViewActivatorBase<IView, View, IViewModel, ViewModel>
	{
		public ViewActivator()
			: base(ViewNames.UnitViews.Assemblies)
		{
		}
	}
}
