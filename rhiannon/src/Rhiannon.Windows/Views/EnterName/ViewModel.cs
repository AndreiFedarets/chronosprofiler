using Rhiannon.Windows.Presentation;

namespace Rhiannon.Windows.Views.EnterName
{
	public class ViewModel : ViewModelBase, IViewModel
	{
		private string _name;

		public string Value
		{
			get { return _name; }
			set { SetPropertyAndNotifyChanged(() => Value, ref _name, value); }
		}
	}
}
