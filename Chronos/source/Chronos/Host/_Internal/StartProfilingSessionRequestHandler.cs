using System;
using System.Linq;
using Chronos.Communication.Native;
using System.Diagnostics;

namespace Chronos.Host
{
    internal class StartProfilingSessionRequestHandler : IRequestServerHandler
    {
        private readonly ConfigurationCollection _configurations;
        private readonly SessionCollection _sessions;
        private readonly FrameworkCollection _frameworks;
        private readonly ProfilingTypeCollection _profilingTypes;
        private readonly ProfilingTargetCollection _profilingTargets;

        public StartProfilingSessionRequestHandler(ConfigurationCollection configurations, SessionCollection sessions,
            FrameworkCollection frameworks, ProfilingTypeCollection profilingTypes, ProfilingTargetCollection profilingTargets)
        {
            _configurations = configurations;
            _sessions = sessions;
            _frameworks = frameworks;
            _profilingTypes = profilingTypes;
            _profilingTargets = profilingTargets;
            Uid = new Guid("B68D7CDC-E999-416A-A9D0-E4A22D243E5F");
        }

        public Guid Uid { get; private set; }

        public SessionSettings Handle(Guid configurationUid, int processId, ProcessPlatform processPlatform, uint profilingBeginTime, Guid agentApplicationUid)
        {
            Configuration configuration = (Configuration)_configurations[configurationUid];
            if (configuration == null)
            {
                throw new ConfigurationNotFoundException(configurationUid);
            }
            ConfigurationSettings configurationSettings = configuration.ConfigurationSettings;
            IProfilingTarget profilingTarget = _profilingTargets[configurationSettings.ProfilingTargetSettings.Uid];
            IProfilingTargetAdapter profilingTargetAdapter = profilingTarget.GetSafeAdapter();
            bool canStartProfiling = profilingTargetAdapter.CanStartProfiling(configurationSettings, processId);
            if (!canStartProfiling)
            {
                throw new TempException("Target process is not supported");
            }
            ActualizeConfigurationSettings(configurationSettings, processPlatform);
            Session session = (Session)_sessions.Create(configuration);
            session.StartProfiling(processId, agentApplicationUid, profilingBeginTime);
            SessionSettings sessionSettings = new SessionSettings(session.Uid, configurationSettings.ProfilingTargetSettings,
                configurationSettings.FrameworksSettings, configurationSettings.ProfilingTypesSettings, configurationSettings.GatewaySettings);
            sessionSettings.Validate();
            profilingTarget.GetSafeAdapter().ProfilingStarted(configurationSettings, sessionSettings, processId);
            return sessionSettings;
        }

        private void ActualizeConfigurationSettings(ConfigurationSettings configurationSettings, ProcessPlatform processPlatform)
        {
            //Actualize profiling target settings
            IProfilingTarget profilingTarget = _profilingTargets[configurationSettings.ProfilingTargetSettings.Uid];
            if (profilingTarget.HasAgent)
            {
                configurationSettings.ProfilingTargetSettings.AgentDll = profilingTarget.GetAgentDll(processPlatform);
            }
            //Actualize frameworks settings
            foreach (FrameworkSettings frameworkSettings in configurationSettings.FrameworksSettings)
            {
                IFramework framework = _frameworks[frameworkSettings.Uid];
                if (framework.HasAgent)
                {
                    frameworkSettings.AgentDll = framework.GetAgentDll(processPlatform);
                }
            }
            //Actualize profiling types settings
            foreach (ProfilingTypeSettings profilingTypeSettings in configurationSettings.ProfilingTypesSettings)
            {
                IProfilingType profilingType = _profilingTypes[profilingTypeSettings.Uid];
                if (profilingType.HasAgent)
                {
                    profilingTypeSettings.AgentDll = profilingType.GetAgentDll(processPlatform);
                }
                profilingTypeSettings.Dependencies = profilingType.Definition.Dependencies.Select(x => x.Uid).ToArray();
                profilingTypeSettings.FrameworkUid = profilingType.Definition.FrameworkUid;
            }
        }

    }
}
