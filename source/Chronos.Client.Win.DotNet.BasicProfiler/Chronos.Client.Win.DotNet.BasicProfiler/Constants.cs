namespace Chronos.Client.Win.DotNet.BasicProfiler
{
    public static class Constants
    {
        public static class ViewModels
        {
            public const string AppDomainsViewModel = "Profiling.DotNet.BasicProfiler.AppDomains";
            public const string AssembliesViewModel = "Profiling.DotNet.BasicProfiler.Assemblies";
            public const string ModulesViewModel = "Profiling.DotNet.BasicProfiler.Modules";
            public const string ClassesViewModel = "Profiling.DotNet.BasicProfiler.Classes";
            public const string FunctionsViewModel = "Profiling.DotNet.BasicProfiler.Functions";
            public const string ThreadsViewModel = "Profiling.DotNet.BasicProfiler.Threads";
        }

        public static class Views
        {
            public const string UnitsListView = "Chronos.Client.Win.Common.Views.UnitsListView, Chronos.Client.Win.Common";
        }

        public static class Menus
        {
            public const string AppDomainContentMenu = "ItemContextMenu";
            public const string AssemblyContentMenu = "ItemContextMenu";
            public const string ModuleContentMenu = "ItemContextMenu";
            public const string ClassContentMenu = "ItemContextMenu";
            public const string FunctionContentMenu = "ItemContextMenu";
            public const string ThreadContentMenu = "ItemContextMenu";
        }
    }
}
