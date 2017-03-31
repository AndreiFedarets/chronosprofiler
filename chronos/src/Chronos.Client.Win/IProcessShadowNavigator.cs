using Chronos.Core;
using Rhiannon.Windows.Presentation.Documents;

namespace Chronos.Client.Win
{
    public interface IProcessShadowNavigator
    {
        void Initialize(IDocument threadTraceDocument);

        void Navigate(IEvent @event);

        void NavigateLoaded(AssemblyInfo unit);

        void NavigateFirst(EventType eventType, uint unit);
    }
}
