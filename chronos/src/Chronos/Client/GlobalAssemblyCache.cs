using System;
using System.Collections.Generic;
using System.IO;

namespace Chronos.Client
{
	public sealed class GlobalAssemblyCache : IEnumerable<System.Reflection.AssemblyName>
	{
		private const string AssemblyDirectoryName = "assembly";
		private const string WindowsDirectoryVariableName = "windir";
		private const string AssemblyFileSearchPattern = "*.dll";
		private readonly Dictionary<string, System.Reflection.AssemblyName> _assemblies;

		public GlobalAssemblyCache()
		{
			string windowsDir = Environment.GetEnvironmentVariable(WindowsDirectoryVariableName);
			string assemblyDir = Path.Combine(windowsDir, AssemblyDirectoryName);
			DirectoryInfo directoryInfo = new DirectoryInfo(assemblyDir);
			_assemblies = new Dictionary<string, System.Reflection.AssemblyName>();
			LoadRecursive(directoryInfo, _assemblies);
		}

		public GlobalAssemblyCache(string path)
		{
			DirectoryInfo directoryInfo = new DirectoryInfo(path);
			_assemblies = new Dictionary<string, System.Reflection.AssemblyName>();
			LoadRecursive(directoryInfo, _assemblies);
		}

		private void LoadRecursive(DirectoryInfo directoryInfo, Dictionary<string, System.Reflection.AssemblyName> dictionary)
		{
			foreach (FileInfo fileInfo in directoryInfo.GetFiles(AssemblyFileSearchPattern))
			{
				System.Reflection.AssemblyName assemblyName = CreateAssemblyNameSafe(fileInfo.Name);
				if (assemblyName != default(System.Reflection.AssemblyName) &&
					!dictionary.ContainsKey(assemblyName.FullName))
				{
					dictionary.Add(assemblyName.FullName, assemblyName);
				}
			}
			foreach (DirectoryInfo childDirectoryInfo in directoryInfo.GetDirectories())
			{
				LoadRecursive(childDirectoryInfo, dictionary);
			}
		}

		private System.Reflection.AssemblyName CreateAssemblyNameSafe(string assemblyName)
		{
			System.Reflection.AssemblyName result;
			try
			{
				result = new System.Reflection.AssemblyName(assemblyName);
			}
			catch
			{
				result = null;
			}
			return result;
		}

		public IEnumerator<System.Reflection.AssemblyName> GetEnumerator()
		{
			return _assemblies.Values.GetEnumerator();
		}

		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}
	}
}