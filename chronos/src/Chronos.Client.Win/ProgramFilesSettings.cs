using System.IO;

namespace Chronos.Client.Win
{
	public class ProgramFilesSettings : IProgramFilesSettings
	{
		public ProgramFilesSettings()
		{
			Initialize();
		}

		public string ProgramFilesDirectory
		{
			get { return @"C:\ChronosProfiler\"; }
		}

		private void Initialize()
		{
			if (!Directory.Exists(ProgramFilesDirectory))
			{
				Directory.CreateDirectory(ProgramFilesDirectory);
			}
		}

		public string ProfilingEventsConfigurationFile
		{
			get { return Path.Combine(ProgramFilesDirectory, "events.cfg"); }
		}

		public string ProfilingFiltersFile
		{
			get { return Path.Combine(ProgramFilesDirectory, "filters.cfg"); }
		}
	}
}
