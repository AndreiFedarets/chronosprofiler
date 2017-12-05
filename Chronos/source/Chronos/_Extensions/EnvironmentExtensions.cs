using System;
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
    }
}
