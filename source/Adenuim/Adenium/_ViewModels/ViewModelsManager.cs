using System;
using System.Reflection;

namespace Adenium
{
    internal static class ViewModelsManager
    {
        public static void BuildViewModelLayout(object viewModel)
        {
            Type viewModelType = viewModel.GetType();
            MethodInfo buildLayoutMethod = viewModelType.GetMethod("BuildLayout", BindingFlags.Instance | BindingFlags.NonPublic);
            if (buildLayoutMethod != null)
            {
                buildLayoutMethod.Invoke(viewModel, null);
            }
        }
    }
}
