namespace Chronos.Extensibility
{
    public interface IExportLoader
    {
        T Load<T>(ExportDefinition definition);

        T Load<T>(ExportDefinition definition, ProcessPlatform platform);
    }
}
