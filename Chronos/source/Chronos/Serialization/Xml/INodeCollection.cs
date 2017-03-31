using System.Collections.Generic;

namespace Chronos.Serialization.Xml
{
    public interface INodeCollection : IEnumerable<INode>
    {
        INode Add(string nodeName);

        INode Add(string nodeName, string @namespace);

        INode FindFirstByName(string nodeName);

        INode FindFirstByFullName(string nodeFullName);
    }
}
