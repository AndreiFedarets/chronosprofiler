using System;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Chronos.Client.Win
{
    internal class ChronosCoreLocator
    {
        private const string ChronosCoreProbingPathKey = "Chronos.Core.ProbingPath";
        private const string ChronosCoreDllName = "Chronos.dll";
        private const string ChronosCoreAssemblyName = "Chronos";
        private Assembly _chronosCoreAssembly;

        private string FindCoreDirectory()
        {
            string value = ConfigurationManager.AppSettings[ChronosCoreProbingPathKey];
            if (string.IsNullOrEmpty(value))
            {
                throw new Exception();
            }
            string[] items = value.Split(new [] { ";" }, StringSplitOptions.RemoveEmptyEntries);
            foreach (string item in items)
            {
                string path = Environment.ExpandEnvironmentVariables(item);
                DirectoryInfo directoryInfo = new DirectoryInfo(path);
                if (!directoryInfo.Exists)
                {
                    continue;
                }
                FileInfo fileInfo = directoryInfo.GetFiles(ChronosCoreDllName, SearchOption.AllDirectories).FirstOrDefault();
                if (fileInfo == null)
                {
                    continue;
                }
                return fileInfo.DirectoryName;
            }
            throw new Exception();
        }

        public void Initialize()
        {
            string chronosCorePath = FindCoreDirectory();
            SetupAssemblyResolver(chronosCorePath);
        }

        private void SetupAssemblyResolver(string chronosCorePath)
        {
            string chronosCoreDll = Path.Combine(chronosCorePath, ChronosCoreDllName);
            _chronosCoreAssembly = Assembly.LoadFrom(chronosCoreDll);
            AppDomain.CurrentDomain.AssemblyResolve += OnAssemblyResolve;
        }

        Assembly OnAssemblyResolve(object sender, ResolveEventArgs args)
        {
            AssemblyName assemblyName = new AssemblyName(args.Name);
            if (string.Equals(assemblyName.Name, ChronosCoreAssemblyName))
            {
                return _chronosCoreAssembly;
            }
            return null;
        }
    }
}
