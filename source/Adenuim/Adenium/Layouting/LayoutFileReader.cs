using System.IO;
using System.Reflection;

namespace Adenium.Layouting
{
    public static class LayoutFileReader
    {
        private static readonly string[] SearchDirectories;
        public const string LayoutFileExtension = ".layout";
        public const string LayoutsDirectoryName = "Layouts";

        static LayoutFileReader()
        {
            SearchDirectories = new[] { string.Empty, LayoutsDirectoryName };
        }

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
            foreach (string searchDirectory in SearchDirectories)
            {
                string layoutFileFullName = Path.Combine(assemblyFilePath, searchDirectory, viewModelUid);
                layoutFileFullName += LayoutFileExtension;
                FileInfo fileInfo = new FileInfo(layoutFileFullName);
                //TODO: check size of file to prevent loading of huge files
                if (fileInfo.Exists)
                {
                    viewModelLayout = File.ReadAllText(layoutFileFullName);
                    return viewModelLayout;
                }
            }
            return null;
        }
    }
}
