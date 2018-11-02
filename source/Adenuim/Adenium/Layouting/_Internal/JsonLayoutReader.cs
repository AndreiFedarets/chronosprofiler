using System;

namespace Adenium.Layouting
{
    internal sealed class JsonLayoutReader : ILayoutReader
    {
        public bool SupportsContentType(string layoutContent)
        {
            return false;
        }

        public ViewModelLayout Read(string layoutContent, IViewModel targetViewModel, IContainer scopeContainer)
        {
            throw new NotImplementedException();
        }
    }
}
