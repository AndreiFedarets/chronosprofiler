namespace Chronos.Communication.Native
{
    public interface IManagedDataHandler : IDataHandler
    {
        bool HandlePackage(NativeArray array);
    }
}
