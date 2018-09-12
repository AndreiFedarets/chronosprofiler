using System;
using System.IO;
using System.Reflection;
using Chronos.Registry;
using System.Security;

namespace Chronos.DotNet
{
    public class AgentInstaller
    {
        private RegistryRoot _registry;

        public void Install()
        {
            VariableCollection variables = PrepareVariables();
            RegistryRoot registry = RegistryRoot.Parse(Properties.Resources.HKLMRegistry);
            if (!TryRegister(registry, variables))
            {
                registry = RegistryRoot.Parse(Properties.Resources.HKCURegistry);
                if (!TryRegister(registry, variables))
                {
                    throw new Exception("Unable to register .NET Profiler");
                }
            }
            _registry = registry;
        }

        public void Uninstall()
        {
            _registry.Remove();
        }

        private bool TryRegister(RegistryRoot registry, VariableCollection variables)
        {
            try
            {
                registry.Remove();
                registry.Import(variables);
                return true;
            }
            catch (Exception)
            {
                try
                {
                    registry.Remove();
                }
                catch (Exception)
                {
                }
                return false;
            }
        }

        private VariableCollection PrepareVariables()
        {
            VariableCollection variables = new VariableCollection();
            string location = Assembly.GetCallingAssembly().Location;
            location = Path.GetDirectoryName(location);
            variables["ASSEMBLY_PATH"] = location;
            return variables;
        }
    }
}
