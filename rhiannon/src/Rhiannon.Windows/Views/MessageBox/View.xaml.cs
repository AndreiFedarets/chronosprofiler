using System.Windows;

namespace Rhiannon.Windows.Views.MessageBox
{
	public partial class View : IView
	{
		public View(IViewModel viewModel)
			: base(viewModel)
		{
			InitializeComponent();
		}

        public MessageBoxImage Image
        {
            get { return ((IViewModel) ViewModel).Image; }
            set { ((IViewModel) ViewModel).Image = value; }
        }

        public string Message
        {
            get { return ((IViewModel)ViewModel).Message; }
            set { ((IViewModel)ViewModel).Message = value; }
        }
    }
}
