using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Adenium.Layouting
{
    internal class ViewModelLayout 
    {
        public ViewModelLayout(List<ViewModelReference> viewModels, MenuCollection menus)
        {
            ViewModels = new ReadOnlyCollection<ViewModelReference>(viewModels);
            Menus = menus;
        }

        public ReadOnlyCollection<ViewModelReference> ViewModels { get; private set; }

        public MenuCollection Menus { get; private set; }
    }
}
