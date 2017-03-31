namespace Rhiannon.Windows.Views.EnterName
{
	public partial class View : IView
	{
		public View(IViewModel viewModel)
			:base(viewModel)
		{
			InitializeComponent();
		}

		public string Value
		{
			get { return ((IViewModel)ViewModel).Value; }
		}
	}
}
