using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Linq;

namespace Chronos
{
    public sealed class WindowsServiceCollection : IWindowsServiceCollection
    {
        private readonly Lazy<List<IWindowsService>> _controllers;

        public WindowsServiceCollection()
        {
            _controllers = new Lazy<List<IWindowsService>>(GetServiceControllers);
        }

        public IWindowsService this[string serviceName]
        {
            get
            {
                return _controllers.Value.FirstOrDefault(x => string.Equals(x.ServiceName, serviceName, StringComparison.OrdinalIgnoreCase));
            }
        }

        public StringDictionary GetEnvironmentVariables()
        {
            Process process = Process.GetProcessesByName(Constants.WindowsService.ServicesProcessName).FirstOrDefault();
            if (process == null)
            {
                throw new TempException("Services root process was not found");
            }
            StringDictionary variables = process.StartInfo.EnvironmentVariables;
            return variables;
        }

        public bool IsServiceHostProcess(Process process, string serviceName)
        {
            IWindowsService service = this[serviceName];
            if (service == null)
            {
                return false;
            }
            Process serviceProcess = null;
            try
            {
                serviceProcess = service.GetServiceProcess();
                bool isServiceHost = Equals(process, serviceProcess);
                return isServiceHost;
            }
            finally
            {
                if (serviceProcess != null)
                {
                    serviceProcess.Dispose();   
                }
            }
        }

        public IEnumerator<IWindowsService> GetEnumerator()
        {
            return _controllers.Value.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        private List<IWindowsService> GetServiceControllers()
        {
            System.ServiceProcess.ServiceController[] controllers = System.ServiceProcess.ServiceController.GetServices();
            return controllers.Select(x => (IWindowsService)new WindowsService(x, this)).ToList();
        }
    }
}
