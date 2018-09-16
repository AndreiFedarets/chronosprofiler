using System;
using System.Collections.Specialized;
using System.Diagnostics;
using System.ServiceProcess;
using Chronos.Win32;
using Microsoft.Win32;

namespace Chronos.Accessibility.WS
{
    internal sealed class ServiceController : IServiceController, IEquatable<ServiceController>
    {
        private readonly ServiceControllerCollection _services;
        private readonly System.ServiceProcess.ServiceController _controller;

        public ServiceController(System.ServiceProcess.ServiceController controller,  ServiceControllerCollection services)
        {
            _controller = controller;
            _services = services;
        }

        public string ServiceName
        {
            get { return _controller.ServiceName; }
        }

        public string DisplayName
        {
            get { return _controller.DisplayName; }
        }

        public Process GetServiceProcess()
        {
            return Advapi32.GetServiceProcessId(_controller);
        }

        public void Stop()
        {
            _controller.Refresh();
            if (_controller.Status != ServiceControllerStatus.Stopped)
            {
                _controller.Stop();
            }
            _controller.WaitForStatus(ServiceControllerStatus.Stopped);
        }

        public void Start()
        {
            _controller.Refresh();
            if (_controller.Status != ServiceControllerStatus.Running)
            {
                _controller.Start();
            }
            _controller.WaitForStatus(ServiceControllerStatus.Running);
        }

        private RegistryKey GetLocalMachineKey()
        {
            string serviceKeyPath = Constants.WindowsService.ServicesRegisteryPath + ServiceName;
            RegistryKey localMachine = Microsoft.Win32.Registry.LocalMachine;
            RegistryKey key = localMachine.OpenSubKey(serviceKeyPath, true);
            return key;
        }

        public void SetEnvironmentVariables(StringDictionary variables)
        {
            RegistryKey key = GetLocalMachineKey();
            if (key == null)
            {
                throw new TempException("Key was not found");
            }
            StringDictionary mainVariables = _services.GetEnvironmentVariables();
            StringDictionary mergedVariables = EnvironmentExtensions.MergeVariables(mainVariables, variables);
            string[] convertedVariables = EnvironmentExtensions.ConvertDictionaryToArray(mergedVariables);
            key.SetValue(Constants.WindowsService.ServiceEnvironmentKeyName, convertedVariables);
        }

        public void RemoveEnvironmentVariables()
        {
            RegistryKey key = GetLocalMachineKey();
            if (key == null)
            {
                throw new TempException("Key was not found");
            }
            key.DeleteValue(Constants.WindowsService.ServiceEnvironmentKeyName, false);
        }

        public bool Equals(ServiceController other)
        {
            if (other == null)
            {
                return false;
            }
            if (ReferenceEquals(this, other))
            {
                return true;
            }
            return _controller.Equals(other._controller);
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as ServiceController);
        }

        public override int GetHashCode()
        {
            return _controller.GetHashCode();
        }
    }
}
