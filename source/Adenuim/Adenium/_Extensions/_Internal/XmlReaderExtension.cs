using System;
using System.Xml;

namespace Adenium
{
    internal static class XmlReaderExtension
    {
        public static T ReadContentAsEnum<T>(this XmlReader reader) where T : struct
        {
            string value = reader.Value;
            return (T)Enum.Parse(typeof(T), value);
        }
    }
}
