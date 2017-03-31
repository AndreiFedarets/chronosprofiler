using System.Collections.Generic;
using Chronos.Client.Win.ViewModels;

namespace Chronos.Client.Win.Menu
{
    internal sealed class MenuBuilder
    {
        public IMenu BuildMenuForApplication(IApplicationBase application, PageViewModel viewModel)
        {
            List<IMenuSource> menus = new List<IMenuSource>();
            foreach (IFramework framework in application.Frameworks)
            {
                IFrameworkAdapter adapter = framework.GetWinAdapter();
                if (adapter is IMenuSource)
                {
                    menus.Add((IMenuSource)adapter);
                }
            }
            foreach (IProfilingType profilingType in application.ProfilingTypes)
            {
                IProfilingTypeAdapter adapter = profilingType.GetWinAdapter();
                if (adapter is IMenuSource)
                {
                    menus.Add((IMenuSource)adapter);
                }
            }
            //foreach (IProductivity productivity in application.Productivities)
            //{
            //    IProductivityAdapter adapter = productivity.GetWinAdapter();
            //    if (adapter is IMenuSource)
            //    {
            //        menus.Add((IMenuSource)adapter);
            //    }
            //}
            return BuildMenu(menus, viewModel);
        }

        public IMenu BuildMenu(List<IMenuSource> haveMenus, PageViewModel viewModel)
        {
            List<IMenu> menus = new List<IMenu>();
            foreach (IMenuSource haveMenu in haveMenus)
            {
                IMenu menu = haveMenu.GetMenu(viewModel);
                if (menu != null)
                {
                    menus.Add(menu);
                }
            }
            return BuildMenu(menus);
        }

        public IMenu BuildMenu(List<IMenu> menus)
        {
            CompositeMenu compositeMenu = new CompositeMenu();
            foreach (IMenu menu in menus)
            {
                compositeMenu.MergeWith(menu);
            }
            return compositeMenu;
        }
    }
}
