using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Chronos.Extensibility;

namespace Chronos
{
    internal sealed class PrerequisiteCollection : RemoteBaseObject, IPrerequisiteCollection
    {
        private readonly List<IPrerequisite> _collection;

        public PrerequisiteCollection(PrerequisiteDefinitionCollection definitions, IExportLoader exportLoader)
        {
            _collection = LoadCollection(definitions, exportLoader);
        }

        public List<PrerequisiteValidationResult> Validate(bool failedOnly)
        {
            IEnumerable<PrerequisiteValidationResult> results = _collection.Select(x => x.Validate());
            if (failedOnly)
            {
                results = results.Where(x => !x.Result);
            }
            return results.ToList();
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
