using System;
using System.Linq;

namespace Rhiannon.Extensions
{
	public static class TypeExtensions
	{
		public static bool ImplementsInterface<T>(this Type type)
		{
			Type targetInterfaceType = typeof (T);
			return type.GetInterfaces().Any(x => x == targetInterfaceType);
		}

		public static Type GetFirstInterface(this Type type)
		{
			if (type.IsInterface)
			{
				return type;
			}
			Type[] interfaces = type.GetInterfaces();
			if (interfaces.Any())
			{
				return interfaces.First();
			}
			return null;
		}
	}
}
