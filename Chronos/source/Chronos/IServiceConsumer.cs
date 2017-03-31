namespace Chronos
{
    public interface IServiceConsumer
    {
        void ExportServices(IServiceContainer container);

        void ImportServices(IServiceContainer container);
    }
}
