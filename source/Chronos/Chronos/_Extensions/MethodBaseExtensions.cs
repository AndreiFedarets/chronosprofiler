using System;
using System.Linq.Expressions;
using System.Reflection;

namespace Chronos
{
    public static class MethodBaseExtensions
    {
        public static string GetFullName(this MethodBase methodBase)
        {
            string fullName = methodBase.DeclaringType + "." + methodBase.Name;
            return fullName;
        }

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

        public static MemberInfo GetMemberInfo(this Expression expression)
        {
            LambdaExpression lambdaExpression = (LambdaExpression)expression;
            MemberExpression memberExpression;
            if (lambdaExpression.Body is UnaryExpression)
            {
                UnaryExpression unaryExpression = (UnaryExpression)lambdaExpression.Body;
                memberExpression = (MemberExpression)unaryExpression.Operand;
            }
            else
            {
                memberExpression = (MemberExpression)lambdaExpression.Body;
            }
            return memberExpression.Member;
        }
    }
}
