using System.Collections.Generic;
using Rhiannon.Windows.Presentation;

namespace Chronos.Client.Win.Views.Options.AddAssembly
{
	public interface IViewModel : IViewModelBase
	{
		IEnumerable<AssemblyName> Assemblies { get; }
	}
}
