using System;
using System.Collections.Generic;
using Chronos.Core;

namespace Chronos.Extensibility
{
	public class SessionActivatorProvider : ISessionActivatorProvider
	{
		private readonly IDictionary<byte, Type> _activators;

		public SessionActivatorProvider()
		{
			_activators = new Dictionary<byte, Type>();
		}

		public bool TryCreate(ActivationSettings settings, Guid configurationToken, out ISessionActivator activator)
		{
			Type activatorType;
			if (!_activators.TryGetValue(settings.SessionActivatorCode, out activatorType))
			{
				activator = null;
				return false;
			}
			activator = (ISessionActivator)Activator.CreateInstance(activatorType, settings, configurationToken);
			return activator.Validate();
		}


		public void Register(Type activatorType, byte activatorCode)
		{
			_activators.Add(activatorCode, activatorType);
		}
	}
}
