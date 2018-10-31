using System.Linq;

namespace Adenium.Layouting
{
    internal static class ViewModelBuilder
    {
        public static void Build(object viewModelObject)
        {
            IViewModel viewModel = viewModelObject as IViewModel;
            if (viewModel != null)
            {
                Build(viewModel);
            }
        }

        public static void Build(IViewModel viewModel)
        {
            IContainerViewModel containerViewModel = viewModel as IContainerViewModel;
            if (containerViewModel != null)
            {
                Build(containerViewModel);
            }
        }

        public static void Build(IContainerViewModel viewModel)
        {
            ViewModelLayout layout = CompositeLayoutProvider.GetLayout(viewModel);
            if (layout != null)
            {
                viewModel.RemoveItems();
                foreach (ViewModelReference reference in layout.ViewModels)
                {
                    IViewModel childViewModel = reference.CreateViewModel();
                    viewModel.ActivateItem(childViewModel);
                }
                MenuCollection menus = viewModel.Menus as MenuCollection;
                if (menus != null)
                {
                    foreach (Menu menu in layout.Menus.Cast<Menu>())
                    {
                        menus.Add(menu);
                    }
                }
            }
        }
    }
}
