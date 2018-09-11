namespace Chronos.Extensibility
{
    internal interface IExtensionReader
    {
        ExtensionDefinition ReadExtension(string extensionPath);
    }
}
