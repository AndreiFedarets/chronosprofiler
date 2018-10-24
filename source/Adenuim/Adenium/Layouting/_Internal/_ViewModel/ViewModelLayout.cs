using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Adenium.Layouting
{
    internal class ViewModelLayout 
    {
        public ViewModelLayout(List<ViewModelReference> viewModels, MenuCollection menus, IActivator activator)
        {
            ViewModels = new ReadOnlyCollection<ViewModelReference>(viewModels);
            Menus = menus;
            Activator = activator;
        }

        public ReadOnlyCollection<ViewModelReference> ViewModels { get; private set; }

        public MenuCollection Menus { get; private set; }

        public IActivator Activator { get; private set; }
    }
}
