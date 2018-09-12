namespace Chronos.Serialization.Xml
{
    public interface IDocument : INode
    {
        void Save(string fileFullName);

        void Load(string fileFullName);

        void LoadXml(string xml);

        INode DocumentNode { get; }
    }
}
