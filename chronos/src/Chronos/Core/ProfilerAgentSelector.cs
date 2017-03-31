using System;

namespace Chronos.Core
{
	public class ProfilerAgentSelector : IProfilerAgentSelector
	{
		private readonly ProcessPlatformDetector _processPlatformDetector;

		public ProfilerAgentSelector()
		{
			_processPlatformDetector = new ProcessPlatformDetector();
		}

		public string SelectProcessAgent(string processFullName)
		{
			ProcessPlatform processPlatform = _processPlatformDetector.DetectProcessPlatform(processFullName);
			return SelectProcessAgent(processPlatform);
		}

		public string SelectProcessAgent(ProcessPlatform processPlatform)
		{
			switch (processPlatform)
			{
				case ProcessPlatform.X64:
					return Constants.EnvironmentVariablesValues.Profilier64Guid;
				case ProcessPlatform.I386:
					return Constants.EnvironmentVariablesValues.Profilier32Guid;
				//case ProcessPlatform.Itanium:
				//case ProcessPlatform.Native:
				default:
					throw new ArgumentException();
			}
		}

		public string GetCurrentSystemProcessAgent()
		{
			ProcessPlatform processPlatform = _processPlatformDetector.DetectSystemPlatform();
			return SelectProcessAgent(processPlatform);
		}
	}
}
