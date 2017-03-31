using System.Collections.Generic;

namespace Chronos.Serialization.Xml
{
    public interface IAttributeCollection : IEnumerable<IAttribute>
    {
        IAttribute Add(string name);

        IAttribute Add(string name, string @namespace);

        IAttribute FindByName(string attributeName);

        IAttribute FindByFullName(string attributeFullName);

        bool Exists(string name);
    }
}
