using System;
using System.Collections.Generic;
using System.Diagnostics;
using Chronos.Accessibility.WS;
using Chronos.Win32;

namespace Chronos.Common.ServiceApplication
{
    public class ProfilingTargetAdapter : IProfilingTargetAdapter
    {
        private readonly IServiceControllerCollection _services;
        private readonly Dictionary<string, IProfilingTargetController> _controllers;

        public ProfilingTargetAdapter()
        {
            _controllers = new Dictionary<string, IProfilingTargetController>();
            _services = new ServiceControllerCollection();
        }

        public IProfilingTargetController CreateController(ConfigurationSettings settings)
        {
            ProfilingTargetSettings profilingTargetSettings = new ProfilingTargetSettings(settings.ProfilingTargetSettings);
            IProfilingTargetController controller;
            lock (_controllers)
            {
                string serviceName = profilingTargetSettings.ServiceName.ToUpperInvariant();
                if (!_controllers.TryGetValue(serviceName, out controller))
                {
                    IServiceController service = _services[serviceName];
                    controller = new ProfilingTargetController(profilingTargetSettings, service);
                    _controllers.Add(serviceName, controller);
                }
            }
            return controller;
        }

        public bool CanStartProfiling(ConfigurationSettings settings, int processId)
        {
            using (Process process = Process.GetProcessById(processId))
            {
                ProfilingTargetSettings profilingTargetSettings = new ProfilingTargetSettings(settings.ProfilingTargetSettings);
                bool targetService = _services.IsServiceHostProcess(process, profilingTargetSettings.ServiceName);
                return targetService;
            }
        }

        public void ProfilingStarted(ConfigurationSettings configurationSettings, SessionSettings sessionSettings, int processId)
        {
        }
    }
}
