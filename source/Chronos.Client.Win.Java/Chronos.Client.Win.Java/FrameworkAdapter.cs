using Chronos.Client.Win.Java.Properties;
using Chronos.Client.Win.Menu;
using Chronos.Client.Win.ViewModels;
using Chronos.Client.Win.ViewModels.Profiling;

namespace Chronos.Client.Win.Java
{
    public class FrameworkAdapter : IFrameworkAdapter, IMenuSource
    {
        public IMenu GetMenu(PageViewModel pageViewModel)
        {
            if (pageViewModel is ProfilingViewModel)
            {
                return MenuReader.ReadMenu(Resources.Menu);
            }
            return null;
        }
    }
}
