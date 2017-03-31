using Chronos.Core;

namespace Chronos.Client.Win.Views.ThreadTrace
{
	public partial class View : IView
	{
		public View(IViewModel viewModel)
			: base(viewModel)
		{
			InitializeComponent();
		}


	    public IEvent SelectedEvent
	    {
            get { return ((IViewModel) ViewModel).SelectedEvent; }
            set { ((IViewModel) ViewModel).SelectedEvent = value; }
	    }
	}
}
