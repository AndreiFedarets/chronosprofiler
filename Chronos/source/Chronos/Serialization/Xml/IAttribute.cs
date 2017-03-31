namespace Chronos.Serialization.Xml
{
    public interface IAttribute
    {
        string Value { get; set; }

        string Name { get; }

        string LocalName { get; }

        string Namespace { get; }

        string FullName { get; }

        T GetValueAs<T>();

        void SetValueAs<T>(T value);
    }
}
