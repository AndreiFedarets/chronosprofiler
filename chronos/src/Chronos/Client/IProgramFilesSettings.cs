namespace Chronos.Client
{
	public interface IProgramFilesSettings
	{
		string ProgramFilesDirectory { get; }

		string ProfilingEventsConfigurationFile { get; }

		string ProfilingFiltersFile { get; }
	}
}
