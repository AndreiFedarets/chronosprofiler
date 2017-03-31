namespace Chronos.Serialization.Xml
{
    public interface INode
    {
        string Value { get; set; }

        string Name { get; }

        string LocalName { get; }

        string FullName { get; }

        string Namespace { get; }

        string InnerXml { get; set; }

        IDocument Owner { get; }

        IAttributeCollection Attributes { get; }

        INodeCollection Children { get; }

        void Delete();

        T GetValueAs<T>();

        void SetValueAs<T>(T value);
    }
}
