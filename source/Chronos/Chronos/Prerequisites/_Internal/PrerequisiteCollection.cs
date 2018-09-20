using System.Collections;
using System.Collections.Generic;
using Chronos.Extensibility;

namespace Chronos.Prerequisites
{
    internal sealed class PrerequisiteCollection : RemoteBaseObject, IPrerequisiteCollection
    {
        private readonly List<IPrerequisite> _collection;

        public PrerequisiteCollection(PrerequisiteDefinitionCollection definitions, IExportLoader exportLoader)
        {
            _collection = LoadCollection(definitions, exportLoader);
        }

        public IEnumerator<IPrerequisite> GetEnumerator()
        {
            return _collection.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        private static List<IPrerequisite> LoadCollection(PrerequisiteDefinitionCollection definitions, IExportLoader exportLoader)
        {
            List<IPrerequisite> collection = new List<IPrerequisite>();
            foreach (PrerequisiteDefinition definition in definitions)
            {
                IPrerequisite item = new Prerequisite(definition, exportLoader);
                collection.Add(item);
            }
            return collection;
        }
    }
}
