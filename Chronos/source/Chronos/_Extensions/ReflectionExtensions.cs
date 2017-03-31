using System;
using System.Linq.Expressions;

namespace Chronos
{
    public class ReflectionExtensions
    {
        public static string GetEventName<T>(Expression<Func<T>> expression)
        {
            string name = ((MemberExpression)expression.Body).Member.Name;
            return name;
        }
    }
}
