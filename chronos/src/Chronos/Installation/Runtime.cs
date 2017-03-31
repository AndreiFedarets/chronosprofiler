using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;

namespace Chronos.Installation
{
	public static class Runtime
	{
		[Flags]
		private enum RuntimeInfo
		{
			UpgradeVersion = 0x01,
			RequestIA64 = 0x02,
			RequestAmd64 = 0x04,
			RequestX86 = 0x08,
			DontReturnDirectory = 0x10,
			DontReturnVersion = 0x20,
			DontShowErrorDialog = 0x40
		}

		[DllImport("mscoree.dll", CharSet = CharSet.Unicode, ExactSpelling = true)]
		static extern int GetRequestedRuntimeInfo(
			string pExe,
			string pwszVersion,
			string pConfigurationFile,
			uint startupFlags,
			RuntimeInfo runtimeInfoFlags,
			StringBuilder pDirectory,
			uint dwDirectory,
			out uint dwDirectoryLength,
			StringBuilder pVersion,
			uint cchBuffer,
			out uint dwLength);

		public static string GetDotNetInstallationFolder()
		{
			Version envVersion = Environment.Version;
			StringBuilder directory = new StringBuilder(0x200);
			StringBuilder version = new StringBuilder(0x20);
			uint directoryLength;
			uint versionLength;
			int hr = GetRequestedRuntimeInfo(
				null,
				"v" + envVersion.ToString(3),
				null,
				0,
				RuntimeInfo.DontShowErrorDialog | RuntimeInfo.UpgradeVersion,
				directory,
				(uint)directory.Capacity,
				out directoryLength,
				version,
				(uint)version.Capacity,
				out versionLength);
			Marshal.ThrowExceptionForHR(hr);
			return Path.Combine(directory.ToString(), version.ToString());
		}
	}
}
