using System;

namespace Adenium.Layouting
{
    internal static class ViewModelBuilder
    {
        public static void Build(IContainerViewModel viewModel, ViewModelLayout layout)
        {
            foreach (ViewModelReference reference in layout.ViewModels)
            {
                Type childViewModelType = reference.Type;
                IViewModel childViewModel = (IViewModel)viewModel.Container.Resolve(childViewModelType);
                viewModel.ActivateItem(childViewModel);
            }
        }

        public static void Build(IContainerViewModel viewModel)
        {
            ViewModelLayout layout = LayoutProvider.GetViewModelLayout(viewModel);
            if (layout != null)
            {
                Build(viewModel, layout);   
            }
        }
    }
}
