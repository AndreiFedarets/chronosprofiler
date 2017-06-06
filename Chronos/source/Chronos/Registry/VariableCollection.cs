using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Microsoft.Win32;

namespace Chronos.Registry
{
    public sealed class VariableCollection
    {
        public static readonly VariableCollection Empty;

        private readonly Dictionary<string, string> _variables;

        static VariableCollection()
        {
            Empty = new VariableCollection();
        }

        public VariableCollection()
        {
            _variables = new Dictionary<string, string>();
        }

        public string this[string name]
        {
            get
            {
                name = name.ToUpperInvariant();
                string value;
                _variables.TryGetValue(name, out value);
                return value;
            }
            set
            {
                name = name.ToUpperInvariant();
                _variables[name] = value;
            }
        }

        public object ReplaceVariables(object value, RegistryValueKind valueKind)
        {
            if (valueKind != RegistryValueKind.String && valueKind != RegistryValueKind.ExpandString && valueKind != RegistryValueKind.MultiString)
            {
                return value;
            }
            if (value == null)
            {
                return null;
            }
            string stringValue = (string)value;
            foreach (KeyValuePair<string, string> variable in _variables)
            {
                string variableName = "%" + variable.Key + "%";
                string variableValue = variable.Value;
                stringValue = Regex.Replace(stringValue, variableName, variableValue, RegexOptions.IgnoreCase);
            }
            return stringValue;
        }
    }
}
