using System.IO;
using System.Reflection;

namespace Adenium.Layouting
{
    public static class LayoutFileReader
    {
        public const string LayoutFileExtension = ".layout";

        public static string ReadViewModelLayout(IViewModel viewModel)
        {
            string viewModelLayout = string.Empty;
            Assembly callingAssembly = Assembly.GetCallingAssembly();
            string assemblyFilePath = callingAssembly.GetAssemblyPath();
            if (string.IsNullOrEmpty(assemblyFilePath))
            {
                return viewModelLayout;
            }
            string viewModelUid = viewModel.ViewModelUid;
            string layoutFileFullName = Path.Combine(assemblyFilePath, viewModelUid);
            layoutFileFullName += LayoutFileExtension;
            if (File.Exists(layoutFileFullName))
            {
                viewModelLayout = File.ReadAllText(layoutFileFullName);
            }
            return viewModelLayout;
        }
    }
}
