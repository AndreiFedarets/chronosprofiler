using Chronos.Client;
using Chronos.Core;
using Rhiannon.Windows.Presentation;

namespace Chronos.Extension.ProfilingStrategy.DotNetPerformance.Win
{
	public interface IViewModel : IViewModelBase
	{
		ClrEventsMask EventsMask { get; }

        bool ProfileSqlQueries { get; }

        ProfilingFilter SelectedFilter { get; }
	}
}
