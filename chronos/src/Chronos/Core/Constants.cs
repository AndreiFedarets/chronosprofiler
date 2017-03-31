namespace Chronos.Core
{
	public static class Constants
	{
		public class EnvironmentVariablesNames
		{
			public const string ConfigurationTokenSettingName = "CHRONOS_PROFILER_CONFIGURATION_TOKEN";
			public const string EnableProfilingSettingName = "COR_ENABLE_PROFILING";
			public const string CompatibilitySettingName = "COMPLUS_ProfAPI_ProfilerCompatibilitySetting";
			public const string ProfilerGuidSettingName = "COR_PROFILER";
			public const string SessionTokenSettingName = "CHRONOS_PROFILER_SESSION_TOKEN";
		}

		public static class EnvironmentVariablesValues
		{
			public const string EnableProfiling = "0x1";
			public const string CompatibilitySetting = "EnableV2Profiler";
			public const string Profilier32Guid = "{9C272F43-1A62-4AA3-B707-621D3507FA0A}";
			public const string Profilier64Guid = "{7248CC0A-132A-423F-9565-90F6B6BB64A5}";
		}
	}
}
