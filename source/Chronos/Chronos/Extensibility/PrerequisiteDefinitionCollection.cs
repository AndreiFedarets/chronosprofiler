using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Chronos.Extensibility
{
    public sealed class PrerequisiteDefinitionCollection : ReadOnlyCollection<PrerequisiteDefinition>
    {
        internal PrerequisiteDefinitionCollection(IList<PrerequisiteDefinition> list)
            : base(list)
        {
        }
    }
}
