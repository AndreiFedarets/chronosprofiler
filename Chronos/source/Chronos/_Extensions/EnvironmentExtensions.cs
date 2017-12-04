using System;

namespace Chronos
{
    public static class EnvironmentExtensions
    {
        private const string PathEnvironmentVariable = "path";

        public static void AddEnvironmentPath(string path)
        {
            string variableValue = Environment.GetEnvironmentVariable(PathEnvironmentVariable, EnvironmentVariableTarget.Process);
            string[] values = variableValue.Split(new[] { ";" }, StringSplitOptions.RemoveEmptyEntries);
            foreach (string value in values)
            {
                if (string.Equals(value, path, StringComparison.OrdinalIgnoreCase))
                {
                    return;
                }
            }
            if (!variableValue.EndsWith(";"))
            {
                variableValue += ";";
            }
            variableValue += path;
            Environment.SetEnvironmentVariable(PathEnvironmentVariable, variableValue, EnvironmentVariableTarget.Process);
        }
    }
}
