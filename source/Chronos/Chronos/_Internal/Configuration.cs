using System;
using System.Collections.Generic;
using System.Linq;

namespace Chronos
{
    internal sealed class Configuration : RemoteBaseObject, IConfiguration
    {
        private readonly Host.IApplication _application;
        private readonly ConfigurationCollection _configurations;
        private readonly ConfigurationSettings _configurationSettings;
        private readonly IProfilingTarget _profilingTarget;
        private readonly IFrameworkCollection _frameworks;
        private readonly List<IProfilingTargetController> _controllers;

        public Configuration(ConfigurationSettings configurationSettings, ConfigurationCollection configurations,
            IProfilingTarget profilingTarget, IFrameworkCollection frameworks, Host.IApplication application)
        {
            _configurations = configurations;
            _configurationSettings = configurationSettings;
            _profilingTarget = profilingTarget;
            _frameworks = frameworks;
            _application = application;
            _controllers = new List<IProfilingTargetController>();
        }

        public Guid Uid
        {
            get
            {
                VerifyDisposed();
                return _configurationSettings.ConfigurationUid;
            }
        }

        public string Name
        {
            get
            {
                VerifyDisposed();
                return _configurationSettings.ConfigurationName;
            }
        }

        public bool IsActive
        {
            get
            {
                lock (_controllers)
                {
                    VerifyDisposed();
                    return _controllers.Any(x => x.IsActive);
                }
            }
        }

        public Host.IApplication Application
        {
            get
            {
                VerifyDisposed();
                return _application;
            }
        }

        public ConfigurationSettings ConfigurationSettings
        {
            get
            {
                VerifyDisposed();
                return _configurationSettings.Clone();
            }
        }

        public void Activate()
        {
            VerifyDisposed();
            IProfilingTargetAdapter adapter = _profilingTarget.GetSafeAdapter();
            IProfilingTargetController controller = adapter.CreateController(_configurationSettings);
            lock (controller)
            {
                bool controllerExists;
                lock (_controllers)
                {
                    controllerExists = _controllers.Contains(controller);
                    //We are already profiling this application
                    if (controllerExists && controller.IsActive)
                    {
                        return;
                    }
                }
                //Select all Frameworks that involved into profiling and notify them
                foreach (FrameworkSettings frameworkSettings in _configurationSettings.FrameworksSettings)
                {
                    IFramework framework = _frameworks[frameworkSettings.Uid];
                    IFrameworkAdapter frameworkAdapter = framework.GetSafeAdapter();
                    frameworkAdapter.ConfigureForProfiling(_configurationSettings);
                }
                //Add correct path to Chronos.Agent.dll (native)
                Agent.AgentResolver.SetupAgentPath(_configurationSettings);
                lock (_controllers)
                {
                    if (!controllerExists)
                    {
                        controller.TargetStopped += OnControllerTargetStopped;
                        _controllers.Add(controller);
                    }
                }
                //Start profiling
                controller.Start();
            }
        }

        private void OnControllerTargetStopped(object sender, ProfilingTargetControllerEventArgs e)
        {
            VerifyDisposed();
            IProfilingTargetController controller = (IProfilingTargetController)sender;
            lock (_controllers)
            {
                _controllers.Remove(controller);
            }
            controller.TargetStopped -= OnControllerTargetStopped;
        }

        public void Deactivate()
        {
            VerifyDisposed();
            List<IProfilingTargetController> controllers;
            lock (_controllers)
            {
                controllers = new List<IProfilingTargetController>(_controllers);
                _controllers.Clear();
            }
            foreach (IProfilingTargetController controller in controllers)
            {
                controller.TargetStopped -= OnControllerTargetStopped;
                controller.Stop();
            }
        }

        public void Remove()
        {
            VerifyDisposed();
            _configurations.OnConfigurationRemoved(this);
            throw new NotImplementedException();
        }

        public override void Dispose()
        {
            VerifyDisposed();
            Deactivate();
            lock (_controllers)
            {
                _controllers.Clear();
            }
            base.Dispose();
        }
    }
}
