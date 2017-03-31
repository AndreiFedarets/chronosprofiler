using System;
using System.Collections.Generic;
using Chronos.Core;

namespace Chronos.Host
{
	public interface IConfigurationCollection : IEnumerable<IConfiguration>, IDisposable
	{
		IConfiguration this[Guid configurationToken] { get; }

		event Action<IConfiguration> ConfigurationCreated;

		event Action<IConfiguration> ConfigurationRemoved;

		IConfiguration Create(ConfigurationSettings configurationSettings, ActivationSettings activationSettings);

		bool Contains(Guid configurationToken);
	}
}
