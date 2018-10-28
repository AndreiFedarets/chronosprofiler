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
                IActivator activator = layout.Activator;
                IViewModel childViewModel = (IViewModel)activator.Resolve(childViewModelType);
                viewModel.ActivateItem(childViewModel);
            }
        }

        public static void Build(IContainerViewModel viewModel)
        {
            ViewModelLayout layout = CompositeLayoutProvider.GetLayout(viewModel);
            if (layout != null)
            {
                Build(viewModel, layout);   
            }
        }
    }
}
