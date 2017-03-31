using System;
using Chronos.Communication.NamedPipe;
using Chronos.Configuration;
using Chronos.Core;

namespace Chronos.Host.Internal
{
	internal class HostRequestServer : ServerCallbacks, IHostRequestServer
	{
		private readonly IHostApplication _hostApplication;
		private readonly IRequestServer _requestServer;
		private readonly ICommonConfiguration _commonConfiguration;

		public HostRequestServer(IHostApplication hostApplication, ICommonConfiguration commonConfiguration)
		{
			_hostApplication = hostApplication;
			_commonConfiguration = commonConfiguration;
			_requestServer = new RequestServer(PipeNameFormatter.GetHostServerAgentPipeName(), this);
		}

		[OperationHandler((long)HostFunctionCode.GetConfigurationSettings)]
		public ConfigurationSettings GetConfigurationSettings(Guid configurationToken)
		{
			IConfiguration configuration = _hostApplication.Configurations[configurationToken];
			if (configuration == null)
			{
				throw new OperationException(ResultCode.ConfigurationNotFound);
			}
			return configuration.ConfigurationSettings;
		}

		[OperationHandler((long)HostFunctionCode.StartProfilingSession)]
		public AgentSettings StartProfilingSession(Guid configurationToken, int processId, uint syncTime)
		{
            Configuration configuration = (Configuration)_hostApplication.Configurations[configurationToken];
			if (configuration == null)
			{
				throw new OperationException(ResultCode.ConfigurationNotFound);
			}
			SessionCollection sessions = (SessionCollection)_hostApplication.Sessions;
			Session session = (Session)sessions.Create(configuration);
			session.StartProfiling(processId, syncTime);
            configuration.OnProcessConnected(processId);
			AgentSettings agentSettings = new AgentSettings(session.Token, _commonConfiguration.Daemon.CallPageSize, _commonConfiguration.Daemon.ThreadStreamsCount);
			return agentSettings;
		}

		public override void Dispose()
		{
			base.Dispose();
			_requestServer.Dispose();
		}
	}
}
