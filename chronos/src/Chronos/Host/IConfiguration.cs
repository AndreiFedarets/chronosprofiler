using System;
using Chronos.Core;

namespace Chronos.Host
{
	public interface IConfiguration
	{
		Guid Token { get; }

		string Name { get; }

		ConfigurationSettings ConfigurationSettings { get; }

		ActivationSettings ActivationSettings { get; }

		ConfigurationState State { get; }

		void Activate();

		void Deactivate();

		void Remove();
	}
}
