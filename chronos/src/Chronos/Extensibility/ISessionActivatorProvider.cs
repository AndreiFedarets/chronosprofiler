using System;
using Chronos.Core;

namespace Chronos.Extensibility
{
	public interface ISessionActivatorProvider
	{
		bool TryCreate(ActivationSettings settings, Guid configurationToken, out ISessionActivator activator);

		void Register(Type activatorType, byte activatorCode);
	}
}
