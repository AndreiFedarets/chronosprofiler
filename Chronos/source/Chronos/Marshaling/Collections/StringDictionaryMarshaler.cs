using System.Collections;
using System.Collections.Specialized;
using System.IO;

namespace Chronos.Marshaling
{
    public class StringDictionaryMarshaler : GenericMarshaler<StringDictionary>
    {
        protected override void MarshalInternal(StringDictionary value, Stream stream)
        {
            Marshal(value, stream);
        }

        protected override StringDictionary DemarshalInternal(Stream stream)
        {
            return Demarshal(stream);
        }

        public static void Marshal(StringDictionary value, Stream stream)
        {
            Int32Marshaler.Marshal(value.Count, stream);
            foreach (DictionaryEntry entry in value)
            {
                string entryKey = entry.Key.ToString();
                string entryValue = entry.Value == null ? string.Empty : entry.Value.ToString();
                StringMarshaler.Marshal(entryKey, stream);
                StringMarshaler.Marshal(entryValue, stream);
            }
        }

        public static StringDictionary Demarshal(Stream stream)
        {
            StringDictionary dictionary = new StringDictionary();
            int count = Int32Marshaler.Demarshal(stream);
            for (int i = 0; i < count; i++)
            {
                string key = StringMarshaler.Demarshal(stream);
                string value = StringMarshaler.Demarshal(stream);
                dictionary.Add(key, value);
            }
            return dictionary;
        }
    }
}
