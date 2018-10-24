using System.Collections.Generic;
using Adenium.Menu;
using Chronos.Client.Win.ViewModels.Profiling;

namespace Chronos.Client.Win
{
    internal sealed class MenuBuilder
    {
        public IMenu BuildProfilingViewMenu(ProfilingViewModel viewModel)
        {
            List<IMenu> menus = new List<IMenu>();
            ClientMessageBus.Current.SendMessage(viewModel, Constants.Message.BuildProfilingViewMenu, menus);
            CompositeMenu compositeMenu = new CompositeMenu(string.Empty, menus);
            return compositeMenu;
        }
    }
}
