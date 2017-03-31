namespace Chronos.Client.Win.Views
{
	public class ViewNames
	{
		public const string Main = "Profiler_MainView";

		public static class Configurations
		{
			public const string ConfigurationTemplates = "Profiler_Configurations_ConfigurationTemplatesView";
			public const string RecentConfigurations = "Profiler_Configurations_RecentConfigurationsView";
			public const string CreateConfiguration = "Profiler_Configurations_CreateConfigurationView";
		}
		public static class Sessions
		{
			public const string SavedSessions = "Profiler_Sessions_SavedSessionsView";
			public const string ActiveSessions = "Profiler_Sessions_ActiveSessionsView";
		}
		public static class Pages
		{
			public const string ConfigurationsPage = "Profiler_Pages_ConfigurationsPageView";
			public const string SessionsPage = "Profiler_Pages_SessionsPageView";
		}
		public static class Groups
		{
			public const string Home = "Profiler_Groups_HomeView";
		}
		public static class BaseViews
		{
			public const string Options = "Profiler_OptionsView";
			public const string ThreadTrace = "Profiler_ThreadTraceView";
			public const string PerformanceCounters = "Profiler_PerformanceCountersView";
		}
		public static class ProcessShadow
		{
			public const string WinApplication = "Profiler_ProcessShadow_WinApplicationView";
		}
        public static class References
        {
            public const string Assembly = "Profiler_References_AssemblyView";
            public const string Class = "Profiler_References_ClassView";
            public const string Function = "Profiler_References_FunctionView";
        }
		public static class UnitViews
		{
			public const string AppDomains = "Profiler_Unit_AppDomainsView";
			public const string Assemblies = "Profiler_Unit_AssembliesView";
			public const string Classes = "Profiler_Unit_ClassesView";
			public const string Exceptions = "Profiler_Unit_ExceptionsView";
			public const string Functions = "Profiler_Unit_FunctionsView";
			public const string Modules = "Profiler_Unit_ModulesView";
			public const string Threads = "Profiler_Unit_ThreadsView";
            public const string SqlRequests = "Profiler_Unit_SqlRequestsView";
            public const string Callstacks = "Profiler_Unit_CallstacksView";
		}
		public static class OptionViews
		{
			public const string Installation = "Profiler_Options_InstallationView";
            public const string ProfilingFilter = "Profiler_Options_ProfilingFilterView";
			public const string AddAssembly = "Profiler_Options_AddAssemblyView";
			public const string PerformanceCounters = "Profiler_Options_PerformanceCountersView";
			public const string Shell = "Profiler_Options_ShellView";
		}
	}
}
