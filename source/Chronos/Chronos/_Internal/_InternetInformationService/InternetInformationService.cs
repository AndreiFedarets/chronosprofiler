using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.DirectoryServices;
using Microsoft.Win32;

namespace Chronos
{
    internal sealed class InternetInformationService : IInternetInformationService
    {
        private readonly RegistryKey _serviceKey;
        private readonly IWindowsService _w3SvcService;
        private readonly IWindowsService _wasService;

        public InternetInformationService(IWindowsServiceCollection services)
        {
            if (HasPermissions)
            {
                _serviceKey = GetServiceLocalMachineKey();   
            }
            if (IsAvailable)
            {
                _w3SvcService = services[Constants.InternetInformationService.W3svcServiceName];
                _wasService = services[Constants.InternetInformationService.WasServiceName];
            }
        }
        
        public bool IsAvailable
        {
            get { return _serviceKey != null; }
        }

        public bool HasPermissions
        {
            get { return SecurityExtensions.HasAdministratorPermissions(); }
        }

        public void Start()
        {
            if (!HasPermissions)
            {
                throw new TempException();
            }
            _wasService.Start();
            _w3SvcService.Start();
        }

        public void Stop()
        {
            if (!HasPermissions)
            {
                throw new TempException();
            }
            Process[] hosts = Process.GetProcessesByName(Constants.InternetInformationService.HostProcessName);
            foreach (Process host in hosts)
            {
                host.Kill();
            }
            _w3SvcService.Stop();
            _wasService.Stop();
        }

        public List<string> GetApplicationPools()
        {
            List<string> collection = new List<string>();
            if (!HasPermissions || !IsAvailable)
            {
                return collection;
            }
            DirectoryEntries appPools = new DirectoryEntry("IIS://localhost/W3SVC/AppPools").Children;
            foreach (DirectoryEntry appPool in appPools)
            {
                collection.Add(appPool.Name);
            }
            return collection;
        }

        public void SetEnvironmentVariables(StringDictionary variables)
        {
            if (!HasPermissions)
            {
                throw new TempException();
            }
            _wasService.AppendEnvironmentVariables(variables);
            _wasService.AppendEnvironmentVariables(variables);
        }

        public void RemoveEnvironmentVariables()
        {
            if (!HasPermissions)
            {
                throw new TempException();
            }
            _w3SvcService.RemoveEnvironmentVariables();
            _wasService.RemoveEnvironmentVariables();
        }

        private RegistryKey GetServiceLocalMachineKey()
        {
            string serviceKeyPath = Constants.InternetInformationService.RegisteryPath;
            RegistryKey localMachine = Microsoft.Win32.Registry.LocalMachine;
            RegistryKey key = localMachine.OpenSubKey(serviceKeyPath, true);
            return key;
        }
    }
}
