using System;
using System.Linq.Expressions;
using System.Reflection;

namespace Rhiannon.Extensions
{
	public static class MethodBaseExtensions
	{
		public static string GetPropertyName(this MethodBase methodBase)
		{
			string propertyName;
			propertyName = methodBase.IsSpecialName ? methodBase.Name.Substring(4, methodBase.Name.Length - 4) : methodBase.Name;
			return propertyName;
		}

		public static string GetPropertyName<T>(this Expression<Func<T>> expression)
		{
			string propertyName = ((MemberExpression)expression.Body).Member.Name;
			return propertyName;
		}

		public static string GetLocalizationKey<T>(this Expression<Func<T>> expression)
		{
			string propertyName = ((MemberExpression)expression.Body).Member.Name;
			return propertyName;
		}
	}
}
