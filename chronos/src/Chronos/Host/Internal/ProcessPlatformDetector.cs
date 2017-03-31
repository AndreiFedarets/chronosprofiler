using System;
using System.IO;
using Chronos.Core;

namespace Chronos.Host.Internal
{
	internal class ProcessPlatformDetector
	{
		private const int PePointerOffset = 60;
		private const int MachineOffset = 4;

		public ProcessPlatform Detect(string fileFullName)
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
	}
}
