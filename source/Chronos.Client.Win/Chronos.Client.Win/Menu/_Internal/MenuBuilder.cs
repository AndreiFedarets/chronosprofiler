using System.Collections.Generic;
using Chronos.Client.Win.ViewModels.Profiling;

namespace Chronos.Client.Win.Menu
{
    internal sealed class MenuBuilder
    {
        public IMenu BuildProfilingViewMenu(ProfilingViewModel viewModel)
        {
            List<IMenu> menus = new List<IMenu>();
            ClientMessageBus.Current.SendMessage(viewModel, Constants.Message.BuildProfilingViewMenu, menus);
            CompositeMenu compositeMenu = new CompositeMenu(menus);
            return compositeMenu;
        }
    }
}
