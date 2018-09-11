using System;

namespace Chronos
{
    public static class ValueParser
    {
        public static T Parse<T>(string value)
        {
            return (T) Convert.ChangeType(value, typeof (T));
        }

        public static T Parse<T>(object value)
        {
            if (value == null)
            {
                return default(T);
            }
            if (value is T)
            {
                return (T)value;
            }
            return (T)Convert.ChangeType(value, typeof(T));
        }
    }
}
