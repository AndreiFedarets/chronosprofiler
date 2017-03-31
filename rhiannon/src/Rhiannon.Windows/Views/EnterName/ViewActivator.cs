using Rhiannon.Windows.Presentation;

namespace Rhiannon.Windows.Views.EnterName
{
	public class ViewActivator : ViewActivatorBase<IView, View, IViewModel, ViewModel>
	{
		public ViewActivator()
			: base(ViewNames.BaseName.EnterName)
		{
		}
	}
}
