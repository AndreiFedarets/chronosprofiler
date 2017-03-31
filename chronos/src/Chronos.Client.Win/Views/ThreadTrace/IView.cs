using Rhiannon.Windows.Presentation;
using Chronos.Core;

namespace Chronos.Client.Win.Views.ThreadTrace
{
	public interface IView : IViewBase
	{
        IEvent SelectedEvent { get; set; }
	}
}
