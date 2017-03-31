using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Linq.Expressions;

namespace Chronos
{
    public static class ExpandoObjectExtensions
    {
        public static bool ContainsProperty<T>(dynamic dynamic, Expression<Func<T>> expression)
        {
            if (!(dynamic is ExpandoObject))
            {
                return false;
            }
            string propertyName = ((MemberExpression)expression.Body).Member.Name;
            IDictionary<string, object> members = (IDictionary<string, object>)dynamic;
            bool result = members.ContainsKey(propertyName);
            return result;
        }

        public static void Copy(dynamic source, dynamic target, params string[] exclusions)
        {
            if (!(target is ExpandoObject) && !(source is ExpandoObject))
            {
                return;
            }
            IDictionary<string, object> targetDictionary = (IDictionary<string, object>)target;
            IDictionary<string, object> sourceDictionary = (IDictionary<string, object>)source;
            foreach (KeyValuePair<string, object> sourcePair in sourceDictionary)
            {
                if (!exclusions.Any(x => string.Equals(sourcePair.Key, x)))
                {
                    targetDictionary[sourcePair.Key] = sourcePair.Value;
                }
            }
        }

        public static void Clear(dynamic dynamic)
        {
            if (!(dynamic is ExpandoObject))
            {
                return;
            }
            IDictionary<string, object> dynamicDictionary = (IDictionary<string, object>)dynamic;
            dynamicDictionary.Clear();
        }
    }
}
