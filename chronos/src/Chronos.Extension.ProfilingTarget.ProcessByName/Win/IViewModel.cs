using Chronos.Core;
using Rhiannon.Windows.Presentation;

namespace Chronos.Extension.ProfilingTarget.ProcessByName.Win
{
	public interface IViewModel : IViewModelBase
	{
		string ProcessName { get; set; }

		ProcessPlatform ProcessPlatform { get; }
	}
}
