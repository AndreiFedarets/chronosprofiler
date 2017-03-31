using System;

namespace Chronos.Core
{
	[Serializable]
	public class ProcessInfo
	{
		static ProcessInfo()
		{
			Empty = new ProcessInfo();
		}

        public ProcessInfo(int id, string processName, string executableFullName, byte[] icon, DateTime startTime, uint syncTime)
		{
			ProcessId = id;
			ProcessName = processName;
			ExecutableFullName = executableFullName;
			StartTime = startTime;
			ProcessIcon = icon;
            SyncTime = syncTime;
		}

		public ProcessInfo()
		{
			ProcessId = 0;
			ProcessName = string.Empty;
			ExecutableFullName = string.Empty;
			StartTime = DateTime.MinValue;
			ProcessIcon = new byte[0];
		}

		public byte[] ProcessIcon { get; set; }

		public static ProcessInfo Empty;

		public int ProcessId { get; set; }

		public string ProcessName { get; set; }

		public string ExecutableFullName { get; set; }

		public DateTime StartTime { get; set; }

        public uint SyncTime { get; set; }

		public uint EndTime { get; set; }
	}
}
