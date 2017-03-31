using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace Chronos.Extensibility
{
    internal sealed class ExtensionAssemblyResolver : IExtensionAssemblyResolver, IDisposable
    {
        private readonly List<string> _path;

        public ExtensionAssemblyResolver()
        {
            _path = new List<string>();
            AppDomain.CurrentDomain.AssemblyResolve += OnAssemblyResolve;
        }

        public event EventHandler<AssemblyLoadEventArgs> AssemblyLoaded;

        public void RegisterPath(string path)
        {
            if (string.IsNullOrEmpty(path))
            {
                throw new TempException();
            }
            path = path.ToLowerInvariant();
            lock (_path)
            {
                if (!_path.Contains(path))
                {
                    _path.Add(path);
                }
            }
        }

        private Assembly OnAssemblyResolve(object sender, ResolveEventArgs args)
        {
            return Resolve(args.Name);
        }

        private Assembly Resolve(string assemblyFullName)
        {
            AssemblyName assemblyName = new AssemblyName(assemblyFullName);
            Assembly assembly = AssemblyCache.GetAssembly(assemblyName);
            if (assembly != null)
            {
                return assembly;
            }
            lock (_path)
            {
                foreach (string path in _path)
                {
                    assembly = Resolve(path, assemblyName);
                    if (assembly != null)
                    {
                        break;
                    }
                }
            }
            if (assembly != null)
            {
                AssemblyCache.AddAssembly(assemblyName, assembly);
                OnAssemblyLoaded(assembly);
            }
            return assembly;
        }

        private void OnAssemblyLoaded(Assembly loadedAssembly)
        {
            EventHandler<AssemblyLoadEventArgs> handler = AssemblyLoaded;
            if (handler != null)
            {
                handler(this, new AssemblyLoadEventArgs(loadedAssembly));
            }
        }

        private Assembly Resolve(string path, AssemblyName assemblyName)
        {
            string simpleName = assemblyName.Name;
            string assemblyPath = Path.Combine(path, simpleName + ".dll");
            if (!File.Exists(assemblyPath))
            {
                return null;
            }
            Assembly assembly;
            try
            {
                assembly = Assembly.LoadFrom(assemblyPath);
            }
            catch (Exception exception)
            {
                throw new TempException(exception);
            }
            return assembly;
        }

        public void Dispose()
        {
            AppDomain.CurrentDomain.AssemblyResolve -= OnAssemblyResolve;
        }
    }
}
