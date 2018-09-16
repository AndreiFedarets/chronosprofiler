using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;

namespace Chronos
{
    public static class EnvironmentExtensions
    {
        public static string AppendEnvironmentPath(string variableValue, string path)
        {
            string[] values = variableValue.Split(new[] { ";" }, StringSplitOptions.RemoveEmptyEntries);
            foreach (string value in values)
            {
                if (string.Equals(Path.GetFullPath(value), Path.GetFullPath(path), StringComparison.OrdinalIgnoreCase))
                {
                    return variableValue;
                }
            }
            if (!variableValue.EndsWith(";"))
            {
                variableValue += ";";
            }
            variableValue += path;
            return variableValue;
        }

        public static string[] ConvertDictionaryToArray(StringDictionary variables)
        {
            List<string> result = new List<string>();
            foreach (DictionaryEntry variable in variables)
            {
                result.Add(string.Format("{0}={1}", variable.Key, variable.Value));
            }
            return result.ToArray();
        }

        public static StringDictionary MergeVariables(StringDictionary variables1, StringDictionary variables2)
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

        public static StringDictionary ExcludeVariables(StringDictionary variables1, StringDictionary variables2)
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

        public static Dictionary<string, string> ConvertArrayToDictionary(string[] variables)
        {
            Dictionary<string, string> result = new Dictionary<string, string>();
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
    }
}
