using System.IO;
using System.Linq;

namespace Rhiannon.Extensions
{
	public static class DirectoryInfoExtensions
	{
		public static long GetFullSize(this DirectoryInfo directoryInfo)
		{
			long result = directoryInfo.GetFiles().Sum(file => file.Length) + directoryInfo.GetDirectories().Sum(directory => directory.GetFullSize());
			return result;
		}
	}
}
