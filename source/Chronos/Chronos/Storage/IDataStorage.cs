namespace Chronos.Storage
{
    public interface IDataStorage
    {
        IDataTable<T> OpenTable<T>();
    }
}
