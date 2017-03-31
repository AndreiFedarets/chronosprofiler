using System.Collections.Generic;
using System.Reflection;

namespace Chronos.Extensibility
{
    internal static class AssemblyCache
    {
        private static readonly Dictionary<string, Assembly> Assemblies;

        static AssemblyCache()
        {
            Assemblies = new Dictionary<string, Assembly>();
        }

        public static void AddAssembly(AssemblyName assemblyName, Assembly assembly)
        {
            lock (Assemblies)
            {
                if (!Assemblies.ContainsKey(assemblyName.Name))
                {
                    Assemblies.Add(assemblyName.Name, assembly);
                }
            }
        }

        public static Assembly GetAssembly(AssemblyName assemblyName)
        {
            lock (Assemblies)
            {
                Assembly assembly;
                Assemblies.TryGetValue(assemblyName.Name, out assembly);
                return assembly;
            }
        }

    }
}
