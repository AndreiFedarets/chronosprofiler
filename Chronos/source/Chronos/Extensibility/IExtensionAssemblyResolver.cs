using System;

namespace Chronos.Extensibility
{
    public interface IExtensionAssemblyResolver
    {
        event EventHandler<AssemblyLoadEventArgs> AssemblyLoaded;

        void RegisterPath(string path);
    }
}
