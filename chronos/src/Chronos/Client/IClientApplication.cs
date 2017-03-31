using System;
using Chronos.Extensibility;
using Chronos.Host;

namespace Chronos.Client
{
	public interface IClientApplication : IDisposable
	{
		Guid Token { get; }

		IProfilingTargetCollection ProfilingTargets { get; }

		IProfilingStrategyCollection ProfilingStrategies { get; }

		IHostApplication Host { get; }
	}
}
