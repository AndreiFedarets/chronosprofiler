using Chronos.Client.Win;
using Chronos.Client.Win.Base.Profiling;

namespace Chronos.DotNet.BasicProfiler.Client.Win
{
    public class OpenUnitsWidgetMenuItem<T> : MenuItemWidget where T : Widget, new()
    {
        protected override void Execute()
        {
            ContainerWidget container = FindParent<ProfilingPage>().FindChild<ContainerWidget>();
            T widget = container.FindChild<T>();
            if (widget == null)
            {
                widget = new T();
                container.AttachChild(widget);
            }
            container.CurrentChild = widget;
        }
    }
}
