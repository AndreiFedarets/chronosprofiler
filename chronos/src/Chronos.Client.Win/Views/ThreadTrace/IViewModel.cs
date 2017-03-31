using Chronos.Core;
using Rhiannon.Windows.Presentation;

namespace Chronos.Client.Win.Views.ThreadTrace
{
	public interface IViewModel : IViewModelBase
	{
		IThreadTraceCollection ThreadTraces { get; }

		IEvent SelectedEvent { get; set; }
	}
}
