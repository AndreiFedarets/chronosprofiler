using System;

namespace Chronos.Core
{
	[Serializable]
	public class ActivationSettings
	{
		public ActivationSettings()
		{
			SessionActivatorCode = 0;
			ProcessFullName = string.Empty;
			ProcessArguments = string.Empty;
			AppPoolName = string.Empty;
			WindowsServiceName = string.Empty;
			ProcessPlatform = ProcessPlatform.I386;
		}

		public byte SessionActivatorCode { get; set; }

		public string ProcessFullName { get; set; }

		public string ProcessArguments { get; set; }

		public string AppPoolName { get; set; }

		public string WindowsServiceName { get; set; }

		public int ConsoleSession { get; set; }

		public ProcessPlatform ProcessPlatform { get; set; }
	}
}
