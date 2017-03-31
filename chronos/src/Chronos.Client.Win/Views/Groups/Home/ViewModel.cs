using System.Windows.Input;
using Rhiannon.Windows.Presentation;

namespace Chronos.Client.Win.Views.Groups.Home
{
	public class ViewModel : ViewModelBase, IViewModel
	{
		private readonly IViewsManager _viewsManager;

		public ViewModel(IViewsManager viewsManager)
		{
			_viewsManager = viewsManager;
		}

		public ICommand CloseDocumentCommand { get; private set; }
	}
}
