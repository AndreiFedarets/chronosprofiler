using System.Collections.Generic;

namespace Chronos.Client.Win.Views.Options.AddAssembly
{
	public partial class View : IView
	{
		public View(IViewModel viewModel)
			: base(viewModel)
		{
			InitializeComponent();
		}

		public IEnumerable<AssemblyName> Assemblies
		{
			get { return ((IViewModel)ViewModel).Assemblies; }
		}
	}
}
