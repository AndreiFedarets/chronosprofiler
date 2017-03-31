using System;

namespace Chronos.Extensibility
{
    internal class ExportLoader : IExportLoader
    {
        private readonly IExtensionAssemblyResolver _assemblyResolver;
        private readonly object _application;

        public ExportLoader(IExtensionAssemblyResolver assemblyResolver, object application)
        {
            _assemblyResolver = assemblyResolver;
            _application = application;
        }

        public T Load<T>(ExportDefinition definition)
        {
            _assemblyResolver.RegisterPath(definition.BaseDirectory);
            string entryPoint = definition.GetEntryPoint(ProcessPlatform.AnyCPU, false);
            return LoadAndInitialize<T>(entryPoint);
        }

        public T Load<T>(ExportDefinition definition, ProcessPlatform platform)
        {
            _assemblyResolver.RegisterPath(definition.BaseDirectory);
            string entryPoint = definition.GetEntryPoint(platform, false);
            return LoadAndInitialize<T>(entryPoint);
        }

        private T LoadAndInitialize<T>(string typeName)
        {
            Type type = LoadType(typeName);
            T export;
            try
            {
                export = (T)Activator.CreateInstance(type);
                export.TryInitialize(_application);
            }
            catch (Exception exception)
            {
                throw new ExtensionLoadingException(string.Format("Unable to create instance of type '{0}'", typeName), exception);
            }
            return export;
        }

        private Type LoadType(string typeName)
        {
            Type type;
            try
            {
                type = Type.GetType(typeName, true);
            }
            catch (Exception exception)
            {
                throw new ExtensionLoadingException(string.Format("Unable to load type '{0}'", typeName), exception);
            }
            if (type == null)
            {
                throw new ExtensionLoadingException(string.Format("Unable to load type '{0}'", typeName));
            }
            return type;
        }
    }
}
