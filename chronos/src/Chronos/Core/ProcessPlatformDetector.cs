using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;

namespace Chronos.Core
{
	public class ProcessPlatformDetector
	{
		private const int PePointerOffset = 60;
		private const int MachineOffset = 4;

		[DllImport("kernel32.dll", SetLastError = true, CallingConvention = CallingConvention.Winapi)]
		[return: MarshalAs(UnmanagedType.Bool)]
		private static extern bool IsWow64Process([In] IntPtr hProcess, [Out] out bool wow64Process
		);

		public ProcessPlatform DetectProcessPlatform(string fileFullName)
		{
			byte[] data = new byte[4096];
			using (Stream stream = new FileStream(fileFullName, FileMode.Open, FileAccess.Read))
			{
				stream.Read(data, 0, data.Length);
			}
			// dos header is 64 bytes, last element, long (4 bytes) is the address of the PE header
			int peHeaderAddress = BitConverter.ToInt32(data, PePointerOffset);
			int machineUint = BitConverter.ToUInt16(data, peHeaderAddress + MachineOffset);
			return (ProcessPlatform)machineUint;
		}

		public ProcessPlatform DetectSystemPlatform()
		{
			try
			{
				using (Process process = Process.GetCurrentProcess())
				{
					bool isWow64;
					IsWow64Process(process.Handle, out isWow64);
				}
			}
			catch (Exception)
			{
				return ProcessPlatform.I386;
			}
			return ProcessPlatform.X64;
		}
	}
}
