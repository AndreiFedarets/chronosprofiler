using System.Linq;

namespace Adenium.Layouting
{
    internal sealed class SmartLayoutReader : ILayoutReader
    {
        private readonly ILayoutReader[] _readers;

        public SmartLayoutReader()
        {
            _readers = new ILayoutReader[] { new XmlLayoutReader(), new JsonLayoutReader() };
        }

        public bool SupportsContentType(string layoutContent)
        {
            return FindLayoutReader(layoutContent) != null;
        }

        public ViewModelLayout Read(string layoutContent, IViewModel targetViewModel, IContainer scopeContainer)
        {
            ILayoutReader layoutReader = FindLayoutReader(layoutContent);
            return layoutReader.Read(layoutContent, targetViewModel, scopeContainer);
        }

        private ILayoutReader FindLayoutReader(string layoutContent)
        {
            return _readers.FirstOrDefault(x => x.SupportsContentType(layoutContent));
        }
    }
}
