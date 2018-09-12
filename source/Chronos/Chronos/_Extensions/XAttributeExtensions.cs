using System;
using System.Globalization;
using System.Xml.Linq;

namespace Chronos
{
    public static class XAttributeExtensions
    {
        public static bool ValueAsBoolean(this XAttribute attribute)
        {
            string value = attribute.Value;
            return bool.Parse(value);
        }

        public static ushort ValueAsUshort(this XAttribute attribute)
        {
            string value = attribute.Value;
            return ushort.Parse(value);
        }

        public static Guid ValueAsGuid(this XAttribute attribute)
        {
            string value = attribute.Value;
            return new Guid(value);
        }

        public static T ValueAsEnum<T>(this XAttribute attribute) where T : struct
        {
            string value = attribute.Value;
            return (T)Enum.Parse(typeof(T), value);
        }

        public static CultureInfo ValueAsCultureInfo(this XAttribute attribute)
        {
            string value = attribute.Value;
            return CultureInfo.GetCultureInfo(value);
        }

        public static Uri ValueAsUri(this XAttribute attribute)
        {
            string value = attribute.Value;
            Uri uri = null;
            if (!string.IsNullOrWhiteSpace(value))
            {
                uri = new Uri(value, UriKind.RelativeOrAbsolute);
            }
            return uri;
        }
    }
}
