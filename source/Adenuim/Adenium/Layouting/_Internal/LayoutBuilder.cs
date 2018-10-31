using System.Collections.Generic;
using System.Linq;

namespace Adenium.Layouting
{
    internal class LayoutBuilder
    {
        public ViewModelLayout BuildLayout(List<ViewModelLayout> layouts)
        {
            List<ViewModelReference> viewModels = BuildViewModels(layouts);
            MenuCollection menus = BuildMenus(layouts);
            return new ViewModelLayout(viewModels, menus);
        }

        private List<ViewModelReference> BuildViewModels(List<ViewModelLayout> layouts)
        {
            return layouts.SelectMany(x => x.ViewModels).OrderBy(x => x.Order).ToList();
        }

        private MenuCollection BuildMenus(List<ViewModelLayout> layouts)
        {
            MenuCollection menus = new MenuCollection();
            foreach (ViewModelLayout layout in layouts)
            {
                IEnumerable<Menu> localMenus = layout.Menus.Cast<Menu>();
                foreach (Menu localMenu in localMenus)
                {
                    menus.Add(localMenu);
                }
            }
            return menus;
        }

    }
}
