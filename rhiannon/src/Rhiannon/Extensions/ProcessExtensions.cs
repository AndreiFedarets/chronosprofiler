using System;
using System.Diagnostics; 
using System.Drawing;
using System.IO;
using System.Drawing.Imaging;

namespace Rhiannon.Extensions
{
	public static class ProcessExtensions
	{
		private static string FindIndexedProcessName(int pid)
		{
			var processName = Process.GetProcessById(pid).ProcessName;
			var processesByName = Process.GetProcessesByName(processName);
			string processIndexdName = null;

			for (var index = 0; index < processesByName.Length; index++)
			{
				processIndexdName = index == 0 ? processName : processName + "#" + index;
				var processId = new PerformanceCounter("Process", "ID Process", processIndexdName);
				if ((int)processId.NextValue() == pid)
				{
					return processIndexdName;
				}
			}

			return processIndexdName;
		}

		private static Process FindPidFromIndexedProcessName(string indexedProcessName)
		{
			var parentId = new PerformanceCounter("Process", "Creating Process ID", indexedProcessName);
			return Process.GetProcessById((int)parentId.NextValue());
		}

		public static Process Parent(this Process process)
		{
			return FindPidFromIndexedProcessName(FindIndexedProcessName(process.Id));
		}

		public static Bitmap GetIcon(this Process process)
		{
			try
			{
				string executableFullName = process.MainModule.FileName;
				Icon icon = Icon.ExtractAssociatedIcon(executableFullName);
				if (icon == null)
				{
					return null;
				}
				Bitmap bitmap = icon.ToBitmap();
				return bitmap;
			}
			catch (Exception)
			{
				return null;
			}
		}

		public static byte[] GetIconBytes(this Process process)
		{
			Bitmap bitmap = process.GetIcon();
			if (bitmap == null)
			{
				return new byte[0];
			}
			using (MemoryStream memoryStream = new MemoryStream())
			{
				bitmap.Save(memoryStream, ImageFormat.Png);
				return memoryStream.ToArray();
			}
		}

		public static string GetExecutableFullName(this Process process)
		{
			try
			{
				return process.MainModule.FileName;
			}
			catch (Exception)
			{
				return process.ProcessName;
			}
		}
	}
}
