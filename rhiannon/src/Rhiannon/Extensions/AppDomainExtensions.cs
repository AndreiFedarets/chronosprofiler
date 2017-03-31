using System;
using System.IO;
using System.Reflection;

namespace Rhiannon.Extensions
{
	public static class AppDomainExtensions
	{
		public static string GetEntryDirectory(this AppDomain appDomain)
		{
			Assembly assembly = Assembly.GetEntryAssembly();
			FileInfo fileInfo = new FileInfo(assembly.Location);
			if (fileInfo.Directory == null)
			{
				return AppDomain.CurrentDomain.BaseDirectory;
			}
			return fileInfo.Directory.FullName;
		}
	}
}
