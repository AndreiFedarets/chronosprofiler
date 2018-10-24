using System;
using System.Collections.Generic;
using System.IO;

namespace Adenium.Layouting
{
    internal sealed class DefaultLayoutPathResolver : ILayoutPathResolver
    {
        private const string LayoutFileExtension = ".layout";

        public List<string> Resolve(Type viewModelType)
        {
            List<string> paths = new List<string>();
            string assemblyPath = Path.GetDirectoryName(viewModelType.Assembly.CodeBase);
            if (string.IsNullOrWhiteSpace(assemblyPath))
            {
                return paths;
            }
            string layoutFullName = Path.Combine(assemblyPath, viewModelType.Name);
            layoutFullName += LayoutFileExtension;
            if (File.Exists(layoutFullName))
            {
                paths.Add(layoutFullName);
            }
            return paths;
        }
    }
}
