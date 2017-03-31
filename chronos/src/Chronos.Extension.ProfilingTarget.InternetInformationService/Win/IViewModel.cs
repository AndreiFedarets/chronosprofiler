using Rhiannon.Windows.Presentation;

namespace Chronos.Extension.ProfilingTarget.InternetInformationService.Win
{
	public interface IViewModel : IViewModelBase
	{
        IApplicationPool SelectedApplicationPool { get; set; }
	}
}
