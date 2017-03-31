using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;

namespace Chronos.Extension.ProfilingTarget.InternetInformationService
{
	public static class EnvironmentVariablesConverter
	{
		public static string[] Convert(StringDictionary variables)
		{
			IList<string> result = new List<string>();
			foreach (DictionaryEntry variable in variables)
			{
				result.Add(string.Format("{0}={1}", variable.Key, variable.Value));
			}
			return result.ToArray();
		}

		public static StringDictionary Merge(StringDictionary variables1, StringDictionary variables2)
		{
			StringDictionary result = new StringDictionary();
			foreach (DictionaryEntry entry in variables1)
			{
				string key = entry.Key.ToString();
				string value = entry.Value.ToString();
				result.Add(key, value);
			}
			foreach (DictionaryEntry entry in variables2)
			{
				string key = entry.Key.ToString();
				if (result.ContainsKey(key))
				{
					result[key] = variables2[key];
				}
				else
				{
					result.Add(key, variables2[key]);
				}
			}
			return result;
		}

		public static StringDictionary Exclude(StringDictionary variables1, StringDictionary variables2)
		{
			StringDictionary result = new StringDictionary();
			foreach (DictionaryEntry entry in variables1)
			{
				string key = entry.Key.ToString();
				if (!variables2.ContainsKey(key))
				{
					string value = entry.Value.ToString();
					result.Add(key, value);
				}
			}
			return result;
		}

		public static IDictionary<string, string> Convert(string[] variables)
		{
			IDictionary<string, string> result = new Dictionary<string, string>();
			foreach (string varable in variables)
			{
				string[] parts = varable.Split('=');
				string key = parts[0];
				string value = string.Empty;
				if (parts.Length > 1)
				{
					value = parts[1];
				}
				result.Add(key, value);
			}
			return result;
		}

		public static bool Contains(StringDictionary what, StringDictionary where)
		{
			foreach (DictionaryEntry entry in what)
			{
				object value = where[entry.Key.ToString()];
				if (value == null)
				{
					return false;
				}
			}
			return true;
		}
	}
}
