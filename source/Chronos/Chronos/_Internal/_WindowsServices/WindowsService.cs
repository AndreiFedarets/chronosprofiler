using System;
using System.Collections.Specialized;
using System.Diagnostics;
using System.ServiceProcess;
using Chronos.Win32;
using Microsoft.Win32;

namespace Chronos
{
    internal sealed class WindowsService : IWindowsService, IEquatable<WindowsService>
    {
        private readonly WindowsServiceCollection _services;
        private readonly System.ServiceProcess.ServiceController _controller;

        public WindowsService(System.ServiceProcess.ServiceController controller,  WindowsServiceCollection services)
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

        public bool IsRunning
        {
            get
            {
                ServiceControllerStatus status = _controller.Status;
                bool isRunning = !(status == ServiceControllerStatus.StopPending || status == ServiceControllerStatus.Stopped);
                return isRunning;
            }
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

        private RegistryKey GetServiceLocalMachineKey()
        {
            string serviceKeyPath = Constants.WindowsService.ServicesRegisteryPath + ServiceName;
            RegistryKey localMachine = Microsoft.Win32.Registry.LocalMachine;
            RegistryKey key = localMachine.OpenSubKey(serviceKeyPath, true);
            return key;
        }

        public void AppendEnvironmentVariables(StringDictionary variables)
        {
            RegistryKey key = GetServiceLocalMachineKey();
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
            RegistryKey key = GetServiceLocalMachineKey();
            if (key == null)
            {
                throw new TempException("Key was not found");
            }
            key.DeleteValue(Constants.WindowsService.ServiceEnvironmentKeyName, false);
        }

        public bool Equals(WindowsService other)
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
            return Equals(obj as WindowsService);
        }

        public override int GetHashCode()
        {
            return _controller.GetHashCode();
        }
    }
}
