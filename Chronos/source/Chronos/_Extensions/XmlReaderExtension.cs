using System;
using System.Globalization;
using System.Xml;

namespace Chronos
{
    public static class XmlReaderExtension
    {
        public static Guid ReadContentAsGuid(this XmlReader reader)
        {
            string value = reader.Value;
            return new Guid(value);
        }

        public static T ReadContentAsEnum<T>(this XmlReader reader) where T : struct 
        {
            string value = reader.Value;
            return (T)Enum.Parse(typeof (T), value);
        }

        public static CultureInfo ReadContentAsCultureInfo(this XmlReader reader)
        {
            string value = reader.Value;
            return CultureInfo.GetCultureInfo(value);
        }

        public static Uri ReadContentAsUri(this XmlReader reader)
        {
            string value = reader.Value;
            Uri uri = null;
            if (!string.IsNullOrWhiteSpace(value))
            {
                uri = new Uri(value, UriKind.RelativeOrAbsolute);
            }
            return uri;
        }
    }
}
