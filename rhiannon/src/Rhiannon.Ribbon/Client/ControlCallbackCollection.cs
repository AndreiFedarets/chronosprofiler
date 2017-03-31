using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Rhiannon.Ribbon.Client
{
	public class ControlCallbackCollection : IControlCallbackCollection
	{
		private readonly IDictionary<string, IControlCallback> _callbacks;

		public ControlCallbackCollection()
		{
			_callbacks = new Dictionary<string, IControlCallback>();
		}

		public IControlCallback this[string id]
		{
			get 
			{
				IControlCallback controlCallbacks;
				_callbacks.TryGetValue(id, out controlCallbacks);
				return controlCallbacks;
			}
		}

		public void Register(IControlCallback controlCallbacks)
		{
			string id = controlCallbacks.ControlId;
			if (_callbacks.ContainsKey(id))
			{
				_callbacks[id] = controlCallbacks;
			}
			else
			{
				_callbacks.Add(id, controlCallbacks);
			}
		}

		public void RegisterAssembly(Assembly assembly, Func<Type, IControlCallback> activator)
		{
			Type[] types = assembly.GetTypes();
			foreach (Type type in types)
			{
				if (type.GetInterfaces().Any(x => x == typeof(IControlCallback)) && !type.IsAbstract && !type.IsInterface)
				{
					IControlCallback controlCallback = activator(type);
					Register(controlCallback);
				}
			}
		}
	}
}
