namespace Chronos.Java
{
    public static class Constants
    {
        public class EnvironmentVariablesNames
        {
            public const string ProfilerSettingName = "JAVA_TOOL_OPTIONS";
        }

        public static class EnvironmentVariablesValues
        {
            public const string ProfilerAgent = "-agentpath:\"{0}\"";
        }

        public const string AgentLibraryName = "Chronos.Java.Agent.dll";
    }
}
