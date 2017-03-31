using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Rhiannon.Extensions
{
	public static class IsNullOrEmptyExtension
	{
		public static bool IsNullOrEmpty(this string value)
		{
			return string.IsNullOrEmpty(value);
		}

		public static bool IsNullOrEmpty<T>(this IEnumerable<T> value)
		{
			return value == null || !value.Any();
		}

		public static bool IsNullOrEmpty(this IEnumerable value)
		{
			return value == null || !value.Cast<object>().Any();
		}

		public static bool IsNullOrEmpty(this object value)
		{
			return value == null;
		}
	}
}
