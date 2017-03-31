using System;
using Chronos.Core;

namespace Chronos.Host.Internal
{
	internal interface IHostRequestServer : IDisposable
	{
		ConfigurationSettings GetConfigurationSettings(Guid configurationToken);

        AgentSettings StartProfilingSession(Guid configurationToken, int processId, uint syncTime);
	}
}
