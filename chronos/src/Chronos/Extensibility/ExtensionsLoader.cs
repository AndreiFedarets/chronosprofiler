using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Chronos.Configuration;
using Rhiannon.Extensions;
using Rhiannon.Logging;
using Rhiannon.Unity;
using System;

namespace Chronos.Extensibility
{
	public class ExtensionsLoader
	{
		private readonly IContainer _container;
		private readonly IList<string> _probingPaths;

		public ExtensionsLoader(IContainer container)
		{
			_container = container;
			_probingPaths = new List<string>();
		}

		public void LoadAndInitialize(IList<ExtensionDescription> extensions)
		{
			InitializeProbingPaths(extensions);
			AppDomain.CurrentDomain.AssemblyResolve += OnCurrentDomainAssemblyResolve;
			foreach (ExtensionDescription extension in extensions)
			{
				LoadAndInitialize(extension);
			}
			AppDomain.CurrentDomain.AssemblyResolve -= OnCurrentDomainAssemblyResolve;
		}

		public void LoadAndInitialize(ExtensionDescription extension)
		{
			try
			{
				Type extensionType = Type.GetType(extension.Type);
				IExtension extensionObject = (IExtension)_container.Resolve(extensionType);
				extensionObject.Initialize();
			}
			catch (Exception exception)
			{
				LoggingProvider.Log(exception, Policy.Core);
			}
		}

		private void InitializeProbingPaths(IEnumerable<ExtensionDescription> extensions)
		{
			string baseDirectory = AppDomain.CurrentDomain.GetEntryDirectory().ToLowerInvariant();
			foreach (ExtensionDescription extension in extensions)
			{
				string probingPath = Path.Combine(baseDirectory, extension.SubPath);
				if (!_probingPaths.Contains(probingPath))
				{
					_probingPaths.Add(probingPath);
				}
			}
		}

		public Assembly Resolve(string probingPath, string assemblyNameString)
		{
			AssemblyName assemblyName;
			try
			{
				assemblyName = new AssemblyName(assemblyNameString);
			}
			catch (Exception exception)
			{
				LoggingProvider.Log(exception, Policy.Core);
				return null;
			}

			string simpleName = assemblyName.Name;
			Assembly assembly;
			try
			{
				string assemblyPath = Path.Combine(probingPath, simpleName + ".dll");
				assembly = Assembly.LoadFrom(assemblyPath);
			}
			catch (ArgumentException exception)
			{
				LoggingProvider.Log(exception, Policy.Core);
				return null;
			}
			return assembly;
		}

		Assembly OnCurrentDomainAssemblyResolve(object sender, ResolveEventArgs args)
		{
			foreach (string probingPath in _probingPaths)
			{
				Assembly assembly = Resolve(probingPath, args.Name);
				if (assembly != null)
				{
					return assembly;
				}
			}
			return null;
		}
	}
}
