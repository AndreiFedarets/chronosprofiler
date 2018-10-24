using System;
using System.Collections.Generic;

namespace Adenium.Layouting
{
    internal static class LayoutProvider
    {
        public static ViewModelLayout GetViewModelLayout(Type viewModelType)
        {
            List<string> paths = LayoutPathResolver.ResolveLayouts(viewModelType);

        }

        public static ViewModelLayout GetViewModelLayout(IViewModel viewModel)
        {
            return GetViewModelLayout(viewModel.GetType());
        }
    }
}
