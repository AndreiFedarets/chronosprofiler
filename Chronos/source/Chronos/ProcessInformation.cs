using System;
using System.IO;

namespace Chronos
{
    /// <summary>
    /// Represents information about profiled process.
    /// </summary>
    [Serializable]
    public class ProcessInformation
    {
        /// <summary>
        /// Create new instance of ProcessInfo
        /// </summary>
        /// <param name="id">Process id.</param>
        /// <param name="processFullName">Process name including path</param>
        /// <param name="icon">Icon of executable</param>
        /// <param name="startTime">Profiled process start date and time</param>
        public ProcessInformation(int id, string processFullName, byte[] icon, DateTime startTime)
        {
            ProcessId = id;
            ProcessName = Path.GetFileName(processFullName);
            ProcessFullName = processFullName;
            StartTime = startTime;
            ProcessIcon = icon;
        }

        /// <summary>
        /// Create new instance of ProcessInfo
        /// </summary>
        public ProcessInformation()
        {
            ProcessId = 0;
            ProcessName = string.Empty;
            ProcessFullName = string.Empty;
            StartTime = DateTime.MinValue;
            ProcessIcon = new byte[0];
        }

        /// <summary>
        /// Icon of executable
        /// </summary>
        public byte[] ProcessIcon { get; set; }

        /// <summary>
        /// Process id
        /// </summary>
        public int ProcessId { get; set; }

        /// <summary>
        /// Process name (without path)
        /// </summary>
        public string ProcessName { get; set; }

        /// <summary>
        /// Process name including path.
        /// </summary>
        public string ProcessFullName { get; set; }

        /// <summary>
        /// Process start date and time.
        /// </summary>
        public DateTime StartTime { get; set; }
    }
}
